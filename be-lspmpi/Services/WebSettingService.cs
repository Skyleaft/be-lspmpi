using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Repositories;

namespace be_lspmpi.Services
{
    public class WebSettingService(IWebSettingRepo webSettingRepo) : IWebSettingService
    {
        public async Task<WebSetting> Get()
        {
            return await webSettingRepo.Get();
        }

        public async Task<WebSettingPublic> GetPublic()
        {
            var settings = await webSettingRepo.Get();
            return new WebSettingPublic
            {
                SiteName = settings.SiteName,
                SiteDescription = settings.SiteDescription,
                SiteUrl = settings.SiteUrl,
                SiteKeywords = settings.SiteKeywords,
                SiteAuthor = settings.SiteAuthor,
                SiteVersion = settings.SiteVersion,
                SiteCopyright = settings.SiteCopyright,
                SiteEmail = settings.SiteEmail,
                SitePhone = settings.SitePhone,
                SiteAddress = settings.SiteAddress,
                SiteLogo = settings.SiteLogo,
                SiteFavicon = settings.SiteFavicon,
                SiteTheme = settings.SiteTheme,
                SiteLanguage = settings.SiteLanguage,
                SiteTimezone = settings.SiteTimezone,
                SiteStatus = settings.SiteStatus,
                SiteMaintenance = settings.SiteMaintenance,
                SiteMaintenanceMessage = settings.SiteMaintenanceMessage,
                SiteSocialMedia = settings.SiteSocialMedia,
                SiteSeo = settings.SiteSeo,
                SiteThemeConfig = settings.SiteThemeConfig,
                SiteFooter = settings.SiteFooter,
                SiteHeader = settings.SiteHeader,
                SiteMeta = settings.SiteMeta
            };
        }

        public async Task<ServiceResponse> Update(WebSetting webSetting)
        {
            try
            {
                await webSettingRepo.Update(webSetting);
                return new ServiceResponse { Success = true, Message = "Web settings updated successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { Success = false, Message = ex.Message };
            }
        }
    }
}
