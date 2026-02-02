using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Services
{
    public interface IArticleTagService
    {
        Task<ArticleTag> Get(int id);
        Task<ServiceResponse> Create(CreateArticleTagDto tagDto);
        Task<ServiceResponse> Update(int id, UpdateArticleTagDto tagDto);
        Task<ServiceResponse> Delete(int id);
        Task<PaginatedResponse<ArticleTag>> Find(FindRequest request);
    }
}