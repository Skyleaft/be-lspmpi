using be_lspmpi.Data;
using be_lspmpi.Dto;
using be_lspmpi.Models;
using Microsoft.EntityFrameworkCore;

namespace be_lspmpi.Repositories
{
    public class ArticleCategoryRepo(IDBContext db) : IArticleCategoryRepo
    {
        public async Task<ArticleCategory> Get(int id)
        {
            return await db.ArticleCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ArticleCategory> Create(ArticleCategory category)
        {
            await db.ArticleCategories.AddAsync(category);
            await db.SaveChangesAsync();
            return category;
        }

        public async Task<ArticleCategory> Update(ArticleCategory category)
        {
            var existing = await db.ArticleCategories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existing == null) return null;

            existing.Name = category.Name;
            await db.SaveChangesAsync();
            return existing;
        }

        public async Task Delete(int id)
        {
            var category = await db.ArticleCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                db.ArticleCategories.Remove(category);
                await db.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResponse<ArticleCategory>> Find(FindRequest request)
        {
            var query = db.ArticleCategories.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.Contains(request.Search));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PaginatedResponse<ArticleCategory>
            {
                Data = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<IEnumerable<ArticleCategory>> GetAll()
        {
            return await db.ArticleCategories.ToListAsync();
        }
    }
}