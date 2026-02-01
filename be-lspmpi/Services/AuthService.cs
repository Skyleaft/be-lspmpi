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
        IAvatarService avatarService,
        ILogger<AuthService> logger) : IAuthService
    {
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            logger.LogDebug("Login attempt for username: {Username}", request.Username);
            try
            {
                var user = await userRepo.Get(request.Username);
                if (user == null)
                {
                    logger.LogDebug("Login failed: User not found for username: {Username}", request.Username);
                    return new LoginResponse { IsAuth = false, Message = "Invalid username or password" };
                }

                var hashedPassword = encryptionService.HashPassword(request.Password, user.PasswordSalt);
                if (user.Password != hashedPassword)
                {
                    logger.LogDebug("Login failed: Invalid password for username: {Username}", request.Username);
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

                logger.LogDebug("Login successful for username: {Username}, UserId: {UserId}", request.Username, user.Id);
                return new LoginResponse { IsAuth = true, Message = "Login successful" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Login error for username: {Username}", request.Username);
                return new LoginResponse { IsAuth = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Register(RegisterRequest request)
        {
            logger.LogDebug("Registration attempt for username: {Username}, email: {Email}", request.Username, request.Email);
            try
            {
                var existingUser = await userRepo.Get(request.Username);
                if (existingUser != null)
                {
                    logger.LogDebug("Registration failed: Username already exists: {Username}", request.Username);
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
                
                logger.LogDebug("Registration successful for username: {Username}, UserId: {UserId}", request.Username, user.Id);
                return new ServiceResponse { Success = true, Message = "Registration successful" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Registration error for username: {Username}", request.Username);
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Logout()
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            logger.LogDebug("Logout attempt for UserId: {UserId}", userId ?? "Unknown");
            try
            {
                await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                logger.LogDebug("Logout successful for UserId: {UserId}", userId ?? "Unknown");
                return new ServiceResponse { Success = true, Message = "Logout successful" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Logout error for UserId: {UserId}", userId ?? "Unknown");
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> ChangePassword(ChangePasswordRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            logger.LogDebug("Change password attempt for UserId: {UserId}", userId ?? "Unknown");
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    logger.LogDebug("Change password failed: User not authenticated");
                    return new ServiceResponse { Success = false, Message = "User not authenticated" };
                }

                var user = await userRepo.Get(Guid.Parse(userId));
                if (user == null)
                {
                    logger.LogDebug("Change password failed: User not found for UserId: {UserId}", userId);
                    return new ServiceResponse { Success = false, Message = "User not found" };
                }

                var currentHashedPassword = encryptionService.HashPassword(request.OldPassword, user.PasswordSalt);
                if (user.Password != currentHashedPassword)
                {
                    logger.LogDebug("Change password failed: Incorrect current password for UserId: {UserId}", userId);
                    return new ServiceResponse { Success = false, Message = "Current password is incorrect" };
                }

                var newSalt = encryptionService.GenerateSalt();
                user.Password = encryptionService.HashPassword(request.NewPassword, newSalt);
                user.PasswordSalt = newSalt;

                await userRepo.Update(user);
                logger.LogDebug("Password changed successfully for UserId: {UserId}", userId);
                return new ServiceResponse { Success = true, Message = "Password changed successfully" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Change password error for UserId: {UserId}", userId ?? "Unknown");
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse> ForgotPassword(string email)
        {
            logger.LogDebug("Forgot password request for email: {Email}", email);
            try
            {
                // Implementation would typically involve:
                // 1. Find user by email
                // 2. Generate reset token
                // 3. Send email with reset link
                // For now, returning a placeholder response
                logger.LogDebug("Forgot password placeholder response sent for email: {Email}", email);
                return new ServiceResponse { Success = true, Message = "Password reset instructions sent to email" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Forgot password error for email: {Email}", email);
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public Task<ServiceResponse> ChangeForgotPassword(ChangeForgotPasswordRequest request)
        {
            throw new NotImplementedException();
        }
    }
}