using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Services
{
    public interface IArticleCategoryService
    {
        Task<ArticleCategory> Get(int id);
        Task<ServiceResponse> Create(CreateArticleCategoryDto categoryDto);
        Task<ServiceResponse> Update(int id, UpdateArticleCategoryDto categoryDto);
        Task<ServiceResponse> Delete(int id);
        Task<PaginatedResponse<ArticleCategory>> Find(FindRequest request);
        Task<IEnumerable<ArticleCategory>> GetAll();
    }
}