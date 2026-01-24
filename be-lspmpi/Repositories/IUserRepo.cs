using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Repositories
{
    public interface IUserRepo
    {
        Task<User> Get(Guid id);
        Task<User> Get(string username);
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task Delete(Guid id);
        Task<PaginatedResponse<User>> Find(FindRequest request);
    }
}