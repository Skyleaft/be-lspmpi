using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Repositories
{
    public interface IArticleTagRepo
    {
        Task<ArticleTag> Get(int id);
        Task<ArticleTag> Create(ArticleTag articleTag);
        Task<ArticleTag> Update(ArticleTag articleTag);
        Task Delete(int id);
        Task<PaginatedResponse<ArticleTag>> Find(FindRequest request);
    }
}