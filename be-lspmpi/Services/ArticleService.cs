using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Repositories;

namespace be_lspmpi.Services
{
    public class ArticleService(IArticleRepo articleRepo, IThumbnailService thumbnailService, IClaimService claimService) : IArticleService
    {
        public async Task<Article> Get(int id)
        {
            return await articleRepo.Get(id);
        }

        public async Task<Article> GetBySlug(string slug)
        {
            return await articleRepo.GetBySlug(slug);
        }

        public async Task<ServiceResponse> Create(CreateArticleDto articleDto)
        {
            try
            {
                var article = new Article
                {
                    Title = articleDto.Title,
                    Slug = GenerateSlug(articleDto.Title),
                    Content = articleDto.Content,
                    Author = claimService.GetProfileName(),
                    CategoryId = articleDto.CategoryId,
                    Thumbnail = articleDto.Thumbnail,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsPublished = false
                };

                await articleRepo.Create(article);
                return new ServiceResponse { Success = true, Message = "Article created successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Update(int id, UpdateArticleDto articleDto)
        {
            try
            {
                var existingArticle = await articleRepo.Get(id);
                if (existingArticle == null)
                {
                    return new ServiceResponse { Success = false, Message = "Article not found" };
                }

                // Delete old thumbnail if new one is provided
                if (!string.IsNullOrEmpty(articleDto.Thumbnail) && articleDto.Thumbnail != existingArticle.Thumbnail)
                {
                    thumbnailService.DeleteOldThumbnail(existingArticle.Thumbnail);
                }

                existingArticle.Title = articleDto.Title;
                existingArticle.Slug = GenerateSlug(articleDto.Title);
                existingArticle.Content = articleDto.Content;
                existingArticle.CategoryId = articleDto.CategoryId;
                existingArticle.Thumbnail = articleDto.Thumbnail;
                existingArticle.IsPublished = articleDto.IsPublished;

                await articleRepo.Update(existingArticle);
                return new ServiceResponse { Success = true, Message = "Article updated successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Delete(int id)
        {
            try
            {
                var existingArticle = await articleRepo.Get(id);
                if (existingArticle == null)
                {
                    return new ServiceResponse { Success = false, Message = "Article not found" };
                }

                // Delete thumbnail file
                thumbnailService.DeleteOldThumbnail(existingArticle.Thumbnail);

                await articleRepo.Delete(id);
                return new ServiceResponse { Success = true, Message = "Article deleted successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginatedResponse<Article>> Find(FindRequest request)
        {
            return await articleRepo.Find(request);
        }

        public async Task<ServiceResponse> AddTags(ArticleTagsDto dto)
        {
            try
            {
                var article = await articleRepo.Get(dto.ArticleId);
                if (article == null)
                {
                    return new ServiceResponse { Success = false, Message = "Article not found" };
                }

                await articleRepo.AddTags(dto.ArticleId, dto.TagIds);
                return new ServiceResponse { Success = true, Message = "Tags added successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> RemoveTags(ArticleTagsDto dto)
        {
            try
            {
                await articleRepo.RemoveTags(dto.ArticleId, dto.TagIds);
                return new ServiceResponse { Success = true, Message = "Tags removed successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<List<ArticleTag>> GetArticleTags(int articleId)
        {
            return await articleRepo.GetArticleTags(articleId);
        }

        private static string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title)) return string.Empty;

            return title.ToLower()
                .Replace(" ", "-")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("!", "")
                .Replace("?", "");
        }
    }
}