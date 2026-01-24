using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Repositories
{
    public interface IArticleCategoryRepo
    {
        Task<ArticleCategory> Get(int id);
        Task<ArticleCategory> Create(ArticleCategory category);
        Task<ArticleCategory> Update(ArticleCategory category);
        Task Delete(int id);
        Task<PaginatedResponse<ArticleCategory>> Find(FindRequest request);
        Task<IEnumerable<ArticleCategory>> GetAll();
    }
}