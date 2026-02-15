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
                var result = await webSettingService.Get();
                return Results.Ok(result);
            })
            .WithName("GetWebSettings")
            .WithSummary("Get web settings")
            .WithDescription("Retrieve web settings")
            .Produces<WebSetting>(200)
            .AllowAnonymous();

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
