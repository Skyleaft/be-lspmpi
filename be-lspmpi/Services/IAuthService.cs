using be_lspmpi.Dto;
using LoginRequest = be_lspmpi.Dto.LoginRequest;

namespace be_lspmpi.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<ServiceResponse> Register(RegisterRequest request);
        Task<ServiceResponse> Logout();
        Task<ServiceResponse> ChangePassword(ChangePasswordRequest request);
        Task<ServiceResponse> ForgotPassword(string email);
        Task<ServiceResponse> ChangeForgotPassword(ChangeForgotPasswordRequest request);
    }
}