using be_lspmpi.Data;
using be_lspmpi.Dto;
using be_lspmpi.Models;
using Microsoft.EntityFrameworkCore;

namespace be_lspmpi.Repositories
{
    public class ArticleRepo(IDBContext db) : IArticleRepo
    {
        public async Task<Article> Get(int id)
        {
            return await db.Articles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Article> GetBySlug(string slug)
        {
            return await db.Articles.FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<Article> Create(Article article)
        {
            await db.Articles.AddAsync(article);
            await db.SaveChangesAsync();
            return article;
        }

        public async Task<Article> Update(Article article)
        {
            var existingArticle = await db.Articles.FirstOrDefaultAsync(x => x.Id == article.Id);
            if (existingArticle == null)
            {
                return null;
            }

            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            existingArticle.CategoryId = article.CategoryId;
            existingArticle.Thumbnail = article.Thumbnail;
            existingArticle.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return existingArticle;
        }

        public async Task Delete(int id)
        {
            var article = await db.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (article != null)
            {
                db.Articles.Remove(article);
                await db.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResponse<Article>> Find(FindRequest request)
        {
            var query = db.Articles.AsQueryable();

            // Search filter
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Title.Contains(request.Search) || x.Content.Contains(request.Search) || x.Author.Contains(request.Search));
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
                            case "categoryid":
                                if (int.TryParse(value, out int categoryId))
                                    query = query.Where(x => x.CategoryId == categoryId);
                                break;
                            case "ispublished":
                                if (bool.TryParse(value, out bool isPublished))
                                    query = query.Where(x => x.IsPublished == isPublished);
                                break;
                            case "author":
                                query = query.Where(x => x.Author.Contains(value));
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
                    "title" => isDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                    "author" => isDescending ? query.OrderByDescending(x => x.Author) : query.OrderBy(x => x.Author),
                    "createdat" => isDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                    "updatedat" => isDescending ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                    "categoryid" => isDescending ? query.OrderByDescending(x => x.CategoryId) : query.OrderBy(x => x.CategoryId),
                    _ => isDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id)
                };
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt); // Default sort
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PaginatedResponse<Article>
            {
                Data = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}