using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Services;

namespace be_lspmpi.Endpoints
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var users = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization();

            users.MapPost("/find", async (FindRequest request, IUserService userService) =>
                await userService.Find(request))
            .WithName("FindUsers")
            .WithSummary("Find users")
            .WithDescription("Search users with criteria")
            .Produces<IEnumerable<Models.User>>(200);

            users.MapGet("/{id}", async (string id, IUserService userService) =>
                {
                    if (!Guid.TryParse(id, out var guid)) return Results.BadRequest();
                    var user = await userService.Get(guid);
                    return user is not null ? Results.Ok(user) : Results.NotFound();
                })
                .WithName("GetUser")
                .WithSummary("Get user by ID")
                .WithDescription("Retrieve user information by user ID")
                .Produces<User>(200)
                .Produces(400)
                .Produces(404)
                .AllowAnonymous();

            users.MapPost("/", async (CreateUserRequest request, IUserService userService, IClaimService claimService) =>
            {
                if (!claimService.IsInRole([1, 2]))
                {
                    return Results.Forbid();
                }

                await userService.Create(request);
                return Results.Created();
            })
            .WithName("CreateUser")
            .WithSummary("Create new user")
            .WithDescription("Create a new user account (Admin and Manager only)")
            .Produces(201)
            .Produces(403);

            users.MapPut("/{id}", async (string id, UserProfile profile, IUserService userService) =>
            {
                if (!Guid.TryParse(id, out var guid)) return Results.BadRequest();
                await userService.Update(guid, profile);
                return Results.NoContent();
            })
            .WithName("UpdateUser")
            .WithSummary("Update user profile")
            .WithDescription("Update user profile information")
            .Produces(204)
            .Produces(400);

            users.MapDelete("/{id}", async (string id, IUserService userService) =>
            {
                if (!Guid.TryParse(id, out var guid)) return Results.BadRequest();
                await userService.Delete(guid);
                return Results.NoContent();
            })
            .WithName("DeleteUser")
            .WithSummary("Delete user")
            .WithDescription("Delete user account")
            .Produces(204)
            .Produces(400);

            users.MapPost("/profile-photo", async (HttpContext context, IUserService userService, IAvatarService avatarService, IClaimService claimService) =>
            {
                var userId = claimService.GetUserId();
                if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

                var user = await userService.Get(Guid.Parse(userId));
                if (user == null) return Results.NotFound("User not found");

                var form = await context.Request.ReadFormAsync();
                var file = form.Files["photo"];
                if (file == null || file.Length == 0) return Results.BadRequest("No file uploaded");

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp", "image/svg" };
                if (!allowedTypes.Contains(file.ContentType)) return Results.BadRequest("Invalid file type");

                // Delete old profile photo
                avatarService.DeleteOldProfilePhoto(user.UserProfile?.ProfilePicture);

                // Compress to WebP
                var fileName = await avatarService.CompressToWebPAsync(file, Guid.Parse(userId));

                await userService.UpdateProfilePhoto(Guid.Parse(userId), fileName);
                return Results.Ok(new PhotoUploadResponse { FileName = fileName, Message = "Profile photo uploaded successfully" });
            })
            .WithName("UploadProfilePhoto")
            .WithSummary("Upload profile photo")
            .WithDescription("Upload user profile photo")
            .Accepts<PhotoUploadRequest>("multipart/form-data")
            .Produces<PhotoUploadResponse>(200)
            .Produces(400)
            .Produces(401);

            users.MapGet("/profile-photo/{userId}", async (string userId, IUserService userService) =>
            {
                if (!Guid.TryParse(userId, out var guid)) return Results.BadRequest();

                var user = await userService.Get(guid);
                if (user?.UserProfile?.ProfilePicture == null) return Results.NotFound();

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "profile-photos", user.UserProfile.ProfilePicture);
                if (!File.Exists(filePath)) return Results.NotFound();

                var contentType = Path.GetExtension(filePath).ToLower() switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".svg" => "image/svg+xml",
                    ".webp" => "image/webp",
                    _ => "application/octet-stream"
                };

                return Results.File(filePath, contentType);
            })
            .WithName("GetProfilePhoto")
            .WithSummary("Get profile photo")
            .WithDescription("Retrieve user profile photo")
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .AllowAnonymous();
        }
    }
}
