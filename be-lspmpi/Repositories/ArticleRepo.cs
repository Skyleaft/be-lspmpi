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
            var query = db.Articles
                .Include(a => a.Category)
                .Include(a => a.ArticleTagMappings)
                    .ThenInclude(m => m.ArticleTag)
                .AsQueryable();

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
                            case "category":
                                query = query.Where(x => x.Category.Name.Contains(value));
                                break;
                            case "tagid":
                                if (int.TryParse(value, out int tagId))
                                    query = query.Where(x => x.ArticleTagMappings.Any(m => m.ArticleTagId == tagId));
                                break;
                            case "tag":
                                query = query.Where(x => x.ArticleTagMappings.Any(m => m.ArticleTag.Name.Contains(value)));
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

        public async Task AddTags(int articleId, List<int> tagIds)
        {
            var mappings = tagIds.Select(tagId => new ArticleTagMapping
            {
                ArticleId = articleId,
                ArticleTagId = tagId
            }).ToList();

            await db.ArticleTagMappings.AddRangeAsync(mappings);
            await db.SaveChangesAsync();
        }

        public async Task RemoveTags(int articleId, List<int> tagIds)
        {
            var mappings = await db.ArticleTagMappings
                .Where(m => m.ArticleId == articleId && tagIds.Contains(m.ArticleTagId))
                .ToListAsync();

            db.ArticleTagMappings.RemoveRange(mappings);
            await db.SaveChangesAsync();
        }

        public async Task<List<ArticleTag>> GetArticleTags(int articleId)
        {
            return await db.ArticleTagMappings
                .Where(m => m.ArticleId == articleId)
                .Select(m => m.ArticleTag)
                .ToListAsync();
        }

        public async Task<List<Article>> GetLatest(int count)
        {
            return await db.Articles
                .Include(a => a.Category)
                .Include(a => a.ArticleTagMappings)
                    .ThenInclude(m => m.ArticleTag)
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .Select(a => new Article
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content.Length > 200 ? a.Content.Substring(0, 200) : a.Content,
                    Author = a.Author,
                    Slug = a.Slug,
                    Thumbnail = a.Thumbnail,
                    IsPublished = a.IsPublished,
                    CategoryId = a.CategoryId,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    Category = a.Category,
                    ArticleTagMappings = a.ArticleTagMappings
                })
                .ToListAsync();
        }
    }
}