using be_lspmpi.Data;
using be_lspmpi.Models;
using Microsoft.EntityFrameworkCore;

namespace be_lspmpi.Repositories
{
    public class WebSettingRepo(IDBContext db) : IWebSettingRepo
    {
        public async Task<WebSetting> Get()
        {
            return await db.WebSettings.FirstOrDefaultAsync() ?? new WebSetting();
        }

        public async Task<WebSetting> Update(WebSetting webSetting)
        {
            var existing = await db.WebSettings.FirstOrDefaultAsync();
            if (existing == null)
            {
                await db.WebSettings.AddAsync(webSetting);
            }
            else
            {
                existing.SiteName = webSetting.SiteName;
                existing.SiteDescription = webSetting.SiteDescription;
                existing.SiteUrl = webSetting.SiteUrl;
                existing.SiteKeywords = webSetting.SiteKeywords;
                existing.SiteAuthor = webSetting.SiteAuthor;
                existing.SiteVersion = webSetting.SiteVersion;
                existing.SiteCopyright = webSetting.SiteCopyright;
                existing.SiteEmail = webSetting.SiteEmail;
                existing.SitePhone = webSetting.SitePhone;
                existing.SiteAddress = webSetting.SiteAddress;
                existing.SiteLogo = webSetting.SiteLogo;
                existing.SiteFavicon = webSetting.SiteFavicon;
                existing.SiteTheme = webSetting.SiteTheme;
                existing.SiteLanguage = webSetting.SiteLanguage;
                existing.SiteTimezone = webSetting.SiteTimezone;
                existing.SiteStatus = webSetting.SiteStatus;
                existing.SitePerPage = webSetting.SitePerPage;
                existing.SiteMaintenance = webSetting.SiteMaintenance;
                existing.SiteMaintenanceMessage = webSetting.SiteMaintenanceMessage;
                existing.SiteSocialMedia = webSetting.SiteSocialMedia;
                existing.SiteAnalytics = webSetting.SiteAnalytics;
                existing.SiteSeo = webSetting.SiteSeo;
                existing.SiteMail = webSetting.SiteMail;
                existing.SiteUpload = webSetting.SiteUpload;
                existing.SitePayment = webSetting.SitePayment;
                existing.SiteMap = webSetting.SiteMap;
                existing.SiteCaptcha = webSetting.SiteCaptcha;
                existing.SiteChat = webSetting.SiteChat;
                existing.SiteBackup = webSetting.SiteBackup;
                existing.SiteOther = webSetting.SiteOther;
                existing.SiteNotification = webSetting.SiteNotification;
                existing.SiteSecurity = webSetting.SiteSecurity;
                existing.SiteCache = webSetting.SiteCache;
                existing.SiteSession = webSetting.SiteSession;
                existing.SiteCookie = webSetting.SiteCookie;
                existing.SiteDebug = webSetting.SiteDebug;
                existing.SiteLog = webSetting.SiteLog;
                existing.SiteApi = webSetting.SiteApi;
                existing.SiteThemeConfig = webSetting.SiteThemeConfig;
                existing.SiteFooter = webSetting.SiteFooter;
                existing.SiteHeader = webSetting.SiteHeader;
                existing.SiteMeta = webSetting.SiteMeta;
            }
            await db.SaveChangesAsync();
            return webSetting;
        }
    }
}
