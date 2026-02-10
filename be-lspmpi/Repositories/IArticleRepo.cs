using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Repositories
{
    public interface IArticleRepo
    {
        Task<Article> Get(int id);
        Task<Article> GetBySlug(string slug);
        Task<Article> Create(Article article);
        Task<Article> Update(Article article);
        Task Delete(int id);
        Task<PaginatedResponse<Article>> Find(FindRequest request);
        Task AddTags(int articleId, List<int> tagIds);
        Task RemoveTags(int articleId, List<int> tagIds);
        Task<List<ArticleTag>> GetArticleTags(int articleId);
        Task<List<Article>> GetLatest(int count);
    }
}