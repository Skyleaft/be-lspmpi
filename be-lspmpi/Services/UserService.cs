using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Repositories;
using Microsoft.Extensions.Logging;

namespace be_lspmpi.Services
{
    public class UserService(IUserRepo userRepo, IEncryptionService encryptionService, ILogger<UserService> logger) : IUserService
    {
        public async Task<User> Get(Guid id)
        {
            logger.LogInformation("Getting user with ID: {UserId}", id);
            return await userRepo.Get(id);
        }

        public async Task Create(CreateUserRequest request)
        {
            logger.LogInformation("Creating user with username: {Username}", request.Username);
            var salt = encryptionService.GenerateSalt();
            request.Password = encryptionService.HashPassword(request.Password, salt);
            var user = new User
            {
                Username = request.Username,
                Password = request.Password,
                PasswordSalt = salt,
                UserProfile = new()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address,
                    City = request.City
                },
                RoleId = request.RoleId
            };
            await userRepo.Create(user);
            logger.LogInformation("User created successfully with username: {Username}", request.Username);
        }

        public async Task Update(Guid id, UserProfile profile)
        {
            logger.LogInformation("Updating user profile for ID: {UserId}", id);
            var currentUser = await userRepo.Get(id);
            if (currentUser is null)
            {
                logger.LogWarning("User not found for update with ID: {UserId}", id);
                throw new Exception("User not found");
            }
            currentUser.UserProfile = profile;
            currentUser.UpdatedAt = DateTime.UtcNow;
            await userRepo.Update(currentUser);
            logger.LogInformation("User profile updated successfully for ID: {UserId}", id);
        }

        public async Task Delete(Guid id)
        {
            logger.LogInformation("Deleting user with ID: {UserId}", id);
            var currentUser = await userRepo.Get(id);
            if (currentUser is null)
            {
                logger.LogWarning("User not found for deletion with ID: {UserId}", id);
                throw new Exception("User not found");
            }
            await userRepo.Delete(id);
            logger.LogInformation("User deleted successfully with ID: {UserId}", id);
        }

        public async Task<User> ValidateUser(string username, string password)
        {
            logger.LogInformation("Validating user with username: {Username}", username);
            var user = await userRepo.Get(username);
            if (user is null)
            {
                logger.LogWarning("User validation failed - user not found: {Username}", username);
                throw new Exception("username or password is not valid");
            }
            if (user.Password != password)
            {
                logger.LogWarning("User validation failed - invalid password for username: {Username}", username);
                throw new Exception("username or password is not valid");
            }
            logger.LogInformation("User validated successfully: {Username}", username);
            return user;
        }

        public Task<User> GetUserByEmail(string email)
        {
            logger.LogWarning("GetUserByEmail method not implemented for email: {Email}", email);
            throw new NotImplementedException();
        }

        public async Task<PaginatedResponse<User>> Find(FindRequest findRequest)
        {
            logger.LogInformation("Finding users with search: {Search}, page: {Page}, pageSize: {PageSize}", 
                findRequest.Search, findRequest.Page, findRequest.PageSize);
            return await userRepo.Find(findRequest);
        }

        public async Task UpdateProfilePhoto(Guid userId, string fileName)
        {
            logger.LogInformation("Updating profile photo for user ID: {UserId}, fileName: {FileName}", userId, fileName);
            var user = await userRepo.Get(userId);
            if (user == null)
            {
                logger.LogWarning("User not found for profile photo update with ID: {UserId}", userId);
                throw new Exception("User not found");
            }

            if (user.UserProfile == null) user.UserProfile = new UserProfile();
            user.UserProfile.ProfilePicture = fileName;
            user.UpdatedAt = DateTime.UtcNow;

            await userRepo.Update(user);
            logger.LogInformation("Profile photo updated successfully for user ID: {UserId}", userId);
        }
    }
}