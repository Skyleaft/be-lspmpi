using be_lspmpi.Dto;
using be_lspmpi.Models;

namespace be_lspmpi.Services
{
    public interface IUserService
    {
        Task<User> Get(Guid id);
        Task Create(CreateUserRequest request);
        Task Update(Guid id, UserProfile profile);
        Task Delete(Guid id);
        Task<User> ValidateUser(string username, string password);
        Task<User> GetUserByEmail(string email);
        Task<PaginatedResponse<User>> Find(FindRequest findRequest);
        Task UpdateProfilePhoto(Guid userId, string fileName);
    }
}