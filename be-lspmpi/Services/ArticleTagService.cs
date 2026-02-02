using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Repositories;

namespace be_lspmpi.Services
{
    public class ArticleTagService(IArticleTagRepo articleTagRepo) : IArticleTagService
    {
        public async Task<ArticleTag> Get(int id)
        {
            return await articleTagRepo.Get(id);
        }

        public async Task<ServiceResponse> Create(CreateArticleTagDto tagDto)
        {
            try
            {
                var articleTag = new ArticleTag
                {
                    Name = tagDto.Name,
                    Description = tagDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await articleTagRepo.Create(articleTag);
                return new ServiceResponse { Success = true, Message = "Article tag created successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Update(int id, UpdateArticleTagDto tagDto)
        {
            try
            {
                var existingTag = await articleTagRepo.Get(id);
                if (existingTag == null)
                {
                    return new ServiceResponse { Success = false, Message = "Article tag not found" };
                }

                existingTag.Name = tagDto.Name;
                existingTag.Description = tagDto.Description;

                await articleTagRepo.Update(existingTag);
                return new ServiceResponse { Success = true, Message = "Article tag updated successfully" };
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
                var existingTag = await articleTagRepo.Get(id);
                if (existingTag == null)
                {
                    return new ServiceResponse { Success = false, Message = "Article tag not found" };
                }

                await articleTagRepo.Delete(id);
                return new ServiceResponse { Success = true, Message = "Article tag deleted successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginatedResponse<ArticleTag>> Find(FindRequest request)
        {
            return await articleTagRepo.Find(request);
        }
    }
}