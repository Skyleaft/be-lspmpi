using System.Security.Claims;
using be_lspmpi.Dto;
using be_lspmpi.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace be_lspmpi.Services
{
    public class AuthService(IUserRepo userRepo,
        IEncryptionService encryptionService,
        IHttpContextAccessor httpContextAccessor,
        IAvatarService avatarService) : IAuthService
    {
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var user = await userRepo.Get(request.Username);
                if (user == null)
                {
                    return new LoginResponse { IsAuth = false, Message = "Invalid username or password" };
                }

                var hashedPassword = encryptionService.HashPassword(request.Password, user.PasswordSalt);
                if (user.Password != hashedPassword)
                {
                    return new LoginResponse { IsAuth = false, Message = "Invalid username or password" };
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Email, user.UserProfile?.Email ?? ""),
                    new(ClaimTypes.Role, user.RoleId.ToString()),
                    new(ClaimTypes.GivenName, user.UserProfile?.Name ?? "")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return new LoginResponse { IsAuth = true, Message = "Login successful" };
            }
            catch (Exception ex)
            {
                return new LoginResponse { IsAuth = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Register(RegisterRequest request)
        {
            try
            {
                var existingUser = await userRepo.Get(request.Username);
                if (existingUser != null)
                {
                    return new ServiceResponse { Success = false, Message = "Username already exists" };
                }

                var salt = encryptionService.GenerateSalt();
                var hashedPassword = encryptionService.HashPassword(request.Password, salt);

                var user = new Models.User
                {
                    Username = request.Username,
                    Password = hashedPassword,
                    PasswordSalt = salt,
                    UserProfile = new()
                    {
                        Name = request.Name,
                        Email = request.Email
                    },
                    RoleId = 3
                };

                await userRepo.Create(user);

                var avatarFileName = await avatarService.GenerateAvatarAsync(request.Name, user.Id);
                user.UserProfile.ProfilePicture = avatarFileName;
                await userRepo.Update(user);
                return new ServiceResponse { Success = true, Message = "Registration successful" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Logout()
        {
            try
            {
                await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return new ServiceResponse { Success = true, Message = "Logout successful" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ServiceResponse { Success = false, Message = "User not authenticated" };
                }

                var user = await userRepo.Get(Guid.Parse(userId));
                if (user == null)
                {
                    return new ServiceResponse { Success = false, Message = "User not found" };
                }

                var currentHashedPassword = encryptionService.HashPassword(request.OldPassword, user.PasswordSalt);
                if (user.Password != currentHashedPassword)
                {
                    return new ServiceResponse { Success = false, Message = "Current password is incorrect" };
                }

                var newSalt = encryptionService.GenerateSalt();
                user.Password = encryptionService.HashPassword(request.NewPassword, newSalt);
                user.PasswordSalt = newSalt;

                await userRepo.Update(user);
                return new ServiceResponse { Success = true, Message = "Password changed successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> ForgotPassword(string email)
        {
            try
            {
                // Implementation would typically involve:
                // 1. Find user by email
                // 2. Generate reset token
                // 3. Send email with reset link
                // For now, returning a placeholder response
                return new ServiceResponse { Success = true, Message = "Password reset instructions sent to email" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public Task<ServiceResponse> ChangeForgotPassword(ChangeForgotPasswordRequest request)
        {
            throw new NotImplementedException();
        }
    }
}