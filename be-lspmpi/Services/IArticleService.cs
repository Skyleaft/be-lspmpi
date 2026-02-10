using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Services
{
    public interface IArticleService
    {
        Task<Article> Get(int id);
        Task<Article> GetBySlug(string slug);
        Task<ServiceResponse> Create(CreateArticleDto articleDto);
        Task<ServiceResponse> Update(int id, UpdateArticleDto articleDto);
        Task<ServiceResponse> Delete(int id);
        Task<PaginatedResponse<Article>> Find(FindRequest request);
        Task<ServiceResponse> AddTags(ArticleTagsDto dto);
        Task<ServiceResponse> RemoveTags(ArticleTagsDto dto);
        Task<List<ArticleTag>> GetArticleTags(int articleId);
        Task<List<Article>> GetLatest();
    }
}
