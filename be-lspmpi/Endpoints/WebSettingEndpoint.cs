using be_lspmpi.Models;
using be_lspmpi.Services;

namespace be_lspmpi.Endpoints
{
    public static class WebSettingEndpoint
    {
        public static void MapWebSettingEndpoints(this WebApplication app)
        {
            var settings = app.MapGroup("/api/settings").WithTags("Settings");

            settings.MapGet("/", async (IWebSettingService webSettingService) =>
            {
                var result = await webSettingService.GetPublic();
                return Results.Ok(result);
            })
            .WithName("GetWebSettings")
            .WithSummary("Get public web settings")
            .WithDescription("Retrieve public web settings (excludes sensitive data)")
            .Produces<WebSettingPublic>(200)
            .AllowAnonymous();

            settings.MapGet("/admin", async (IWebSettingService webSettingService, IClaimService claimService) =>
            {
                if (!claimService.IsInRole([1]))
                {
                    return Results.Forbid();
                }

                var result = await webSettingService.Get();
                return Results.Ok(result);
            })
            .WithName("GetWebSettingsAdmin")
            .WithSummary("Get all web settings (admin only)")
            .WithDescription("Retrieve all web settings including sensitive data (Role level 1 required)")
            .Produces<WebSetting>(200)
            .Produces(403)
            .RequireAuthorization();

            settings.MapPut("/", async (WebSetting webSetting, IWebSettingService webSettingService) =>
            {
                var result = await webSettingService.Update(webSetting);
                return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
            })
            .WithName("UpdateWebSettings")
            .WithSummary("Update web settings")
            .WithDescription("Update web settings")
            .Produces(204)
            .Produces(400)
            .RequireAuthorization();
        }
    }
}
