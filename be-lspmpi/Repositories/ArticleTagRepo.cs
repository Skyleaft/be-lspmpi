using be_lspmpi.Data;
using be_lspmpi.Dto;
using be_lspmpi.Models;
using Microsoft.EntityFrameworkCore;

namespace be_lspmpi.Repositories
{
    public class ArticleTagRepo(IDBContext db) : IArticleTagRepo
    {
        public async Task<ArticleTag> Get(int id)
        {
            return await db.ArticleTags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ArticleTag> Create(ArticleTag articleTag)
        {
            await db.ArticleTags.AddAsync(articleTag);
            await db.SaveChangesAsync();
            return articleTag;
        }

        public async Task<ArticleTag> Update(ArticleTag articleTag)
        {
            var existingTag = await db.ArticleTags.FirstOrDefaultAsync(x => x.Id == articleTag.Id);
            if (existingTag == null)
            {
                return null;
            }

            existingTag.Name = articleTag.Name;
            existingTag.Description = articleTag.Description;
            existingTag.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return existingTag;
        }

        public async Task Delete(int id)
        {
            var articleTag = await db.ArticleTags.FirstOrDefaultAsync(x => x.Id == id);
            if (articleTag != null)
            {
                db.ArticleTags.Remove(articleTag);
                await db.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResponse<ArticleTag>> Find(FindRequest request)
        {
            var query = db.ArticleTags.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.Contains(request.Search) || x.Description.Contains(request.Search));
            }

            if (!string.IsNullOrEmpty(request.Sort))
            {
                var isDescending = !string.IsNullOrEmpty(request.Order) && request.Order.ToLower() == "desc";

                query = request.Sort.ToLower() switch
                {
                    "name" => isDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                    "createdat" => isDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                    "updatedat" => isDescending ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                    _ => isDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id)
                };
            }
            else
            {
                query = query.OrderBy(x => x.Name);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PaginatedResponse<ArticleTag>
            {
                Data = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}