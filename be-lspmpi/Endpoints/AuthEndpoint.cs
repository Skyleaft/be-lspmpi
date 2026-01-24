using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Services;

namespace be_lspmpi.Endpoints
{
    public static class AuthEndpoint
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var auth = app.MapGroup("/api/auth").WithTags("Authentication");

            auth.MapPost("/login", async (LoginRequest request, IAuthService authService, IClaimService claimService) =>
            {
                var userId = claimService.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    return Results.BadRequest(new LoginResponse { IsAuth = false, Message = "User already logged in" });
                }

                var result = await authService.Login(request);
                return result.IsAuth ? Results.Ok(result) : Results.Unauthorized();
            })
            .WithName("Login")
            .WithSummary("User login")
            .WithDescription("Authenticate user and create session cookie")
            .Produces<LoginResponse>(200)
            .Produces<LoginResponse>(400)
            .Produces(401);

            auth.MapPost("/register", async (RegisterRequest request, IAuthService authService) =>
            {
                var result = await authService.Register(request);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            })
            .WithName("Register")
            .WithSummary("User registration")
            .WithDescription("Create new user account")
            .Produces<ServiceResponse>(200)
            .Produces<ServiceResponse>(400);

            auth.MapPost("/logout", async (IAuthService authService) =>
            {
                var result = await authService.Logout();
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithName("Logout")
            .WithSummary("User logout")
            .WithDescription("Clear user session cookie")
            .Produces<ServiceResponse>(200);

            auth.MapPost("/change-password", async (ChangePasswordRequest request, IAuthService authService) =>
            {
                var result = await authService.ChangePassword(request);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            })
            .RequireAuthorization()
            .WithName("ChangePassword")
            .WithSummary("Change user password")
            .WithDescription("Update current user password")
            .Produces<ServiceResponse>(200)
            .Produces<ServiceResponse>(400);

            auth.MapPost("/forgot-password", async (string email, IAuthService authService) =>
            {
                var result = await authService.ForgotPassword(email);
                return Results.Ok(result);
            })
            .WithName("ForgotPassword")
            .WithSummary("Forgot password")
            .WithDescription("Send password reset instructions to email")
            .Produces<ServiceResponse>(200);

            auth.MapGet("/me", (IClaimService claimService) =>
            {
                var userId = claimService.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var user = claimService.GetClaimUser();

                return Results.Ok(user);
            })
            .RequireAuthorization()
            .WithName("GetCurrentUser")
            .WithSummary("Get current user")
            .WithDescription("Get authenticated user information from cookie")
            .Produces<ClaimUser>(200)
            .Produces(401);
        }
    }
}