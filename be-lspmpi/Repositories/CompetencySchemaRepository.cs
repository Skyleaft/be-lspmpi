using be_lspmpi.Models;
using be_lspmpi.Dto;
using be_lspmpi.Data;
using Microsoft.EntityFrameworkCore;

namespace be_lspmpi.Repositories;

public class CompetencySchemaRepository(IDBContext db) : ICompetencySchemaRepository
{
    public async Task<IEnumerable<CompetencySchema>> GetAllAsync()
    {
        return await db.CompetencySchemas.ToListAsync();
    }

    public async Task<CompetencySchema?> GetByIdAsync(int id)
    {
        return await db.CompetencySchemas.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CompetencySchema> CreateAsync(CompetencySchema competencySchema)
    {
        competencySchema.CreatedAt = DateTime.UtcNow;
        await db.CompetencySchemas.AddAsync(competencySchema);
        await db.SaveChangesAsync();
        return competencySchema;
    }

    public async Task<CompetencySchema> UpdateAsync(CompetencySchema competencySchema)
    {
        var existing = await db.CompetencySchemas.FirstOrDefaultAsync(x => x.Id == competencySchema.Id);
        if (existing == null)
        {
            throw new ArgumentException("CompetencySchema not found");
        }

        existing.Name = competencySchema.Name;
        existing.Description = competencySchema.Description;
        existing.Duration = competencySchema.Duration;
        existing.Fee = competencySchema.Fee;
        existing.Competencies = competencySchema.Competencies;
        existing.Image = competencySchema.Image;
        existing.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var competencySchema = await db.CompetencySchemas.FirstOrDefaultAsync(x => x.Id == id);
        if (competencySchema != null)
        {
            db.CompetencySchemas.Remove(competencySchema);
            await db.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<PaginatedResponse<CompetencySchema>> Find(FindRequest request)
    {
        var query = db.CompetencySchemas.AsQueryable();

        // Search filter
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => x.Name.Contains(request.Search) || x.Description.Contains(request.Search));
        }

        // Additional filters
        if (!string.IsNullOrEmpty(request.Filter))
        {
            var filters = request.Filter.Split(',');
            foreach (var filter in filters)
            {
                var parts = filter.Split(':');
                if (parts.Length == 2)
                {
                    var field = parts[0].Trim().ToLower();
                    var value = parts[1].Trim();

                    switch (field)
                    {
                        case "duration":
                            query = query.Where(x => x.Duration.Contains(value));
                            break;
                        case "fee":
                            if (decimal.TryParse(value, out decimal fee))
                                query = query.Where(x => x.Fee == fee);
                            break;
                    }
                }
            }
        }

        // Sorting
        if (!string.IsNullOrEmpty(request.Sort))
        {
            var isDescending = !string.IsNullOrEmpty(request.Order) && request.Order.ToLower() == "desc";

            query = request.Sort.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "fee" => isDescending ? query.OrderByDescending(x => x.Fee) : query.OrderBy(x => x.Fee),
                "duration" => isDescending ? query.OrderByDescending(x => x.Duration) : query.OrderBy(x => x.Duration),
                "createdat" => isDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                _ => isDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id)
            };
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PaginatedResponse<CompetencySchema>
        {
            Data = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}