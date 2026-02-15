namespace be_lspmpi.Models;

public class WebSetting
{
    public int Id { get; set; }
    public string SiteName { get; set; } = "LSPMPI";
    public string SiteDescription { get; set; } = "LSPMPI - LSPMPI";
    public string SiteUrl { get; set; } = "https://localhost:5001";
    public string SiteKeywords { get; set; } = "LSPMPI";
    public string SiteAuthor { get; set; } = "LSPMPI";
    public string SiteVersion { get; set; } = "1.0.0";
    public string SiteCopyright { get; set; } = "Copyright © 2023 LSPMPI. All rights reserved.";
    public string SiteEmail { get; set; } = "admin@lspmpi.com";
    public string SitePhone { get; set; } = "+62 812 3456 7890";
    public string SiteAddress { get; set; } = "Indonesia";
    public string SiteLogo { get; set; } = "/images/logo.png";
    public string SiteFavicon { get; set; } = "/images/favicon.ico";
    public string SiteTheme { get; set; } = "light";
    public string SiteLanguage { get; set; } = "en";
    public string SiteTimezone { get; set; } = "Asia/Jakarta";
    public bool SiteStatus { get; set; } = true;
    public int SitePerPage { get; set; } = 10;
    public bool SiteMaintenance { get; set; } = false;
    public string SiteMaintenanceMessage { get; set; } = "We are currently under maintenance. Please check back later.";
    public Dictionary<string, string> SiteSocialMedia { get; set; } = new Dictionary<string, string>
    {
        { "facebook", "#" },
        { "twitter", "#" },
        { "instagram", "#" },
        { "linkedin", "#" },
        { "youtube", "#" }
    };
    public Dictionary<string, string> SiteAnalytics { get; set; } = new Dictionary<string, string>
    {
        { "google", "#" },
        { "facebook", "#" }
    };
    public Dictionary<string, string> SiteSeo { get; set; } = new Dictionary<string, string>
    {
        { "title", "LSPMPI" },
        { "description", "LSPMPI" },
        { "keywords", "LSPMPI" }
    };
    public Dictionary<string, string> SiteMail { get; set; } = new Dictionary<string, string>
    {
        { "host", "smtp.gmail.com" },
        { "port", "587" },
        { "username", "admin@lspmpi.com" },
        { "password", "password" },
        { "from", "admin@lspmpi.com" },
        { "to", "admin@lspmpi.com" }
    };
    public Dictionary<string, string> SiteUpload { get; set; } = new Dictionary<string, string>
    {
        { "path", "/uploads/" },
        { "size", "10" },
        { "type", "jpg,png,gif,pdf,doc,docx,xls,xlsx,ppt,pptx" }
    };
    public Dictionary<string, string> SitePayment { get; set; } = new Dictionary<string, string>
    {
        { "midtrans", "#" },
        { "va", "#" },
        { "paypal", "#" }
    };
    public Dictionary<string, string> SiteMap { get; set; } = new Dictionary<string, string>
    {
        { "google", "#" }
    };
    public Dictionary<string, string> SiteCaptcha { get; set; } = new Dictionary<string, string>
    {
        { "sitekey", "#" },
        { "secretkey", "#" }
    };
    public Dictionary<string, string> SiteChat { get; set; } = new Dictionary<string, string>
    {
        { "tawk", "#" }
    };
    public Dictionary<string, string> SiteBackup { get; set; } = new Dictionary<string, string>
    {
        { "database", "#" },
        { "file", "#" }
    };
    public Dictionary<string, string> SiteOther { get; set; } = new Dictionary<string, string>
    {
        { "api", "#" },
        { "token", "#" }
    };
    public Dictionary<string, string> SiteNotification { get; set; } = new Dictionary<string, string>
    {
        { "push", "#" },
        { "email", "#" },
        { "sms", "#" }
    };
    public Dictionary<string, string> SiteSecurity { get; set; } = new Dictionary<string, string>
    {
        { "csrf", "#" },
        { "xss", "#" },
        { "sql", "#" }
    };
    public Dictionary<string, string> SiteCache { get; set; } = new Dictionary<string, string>
    {
        { "redis", "#" },
        { "memcached", "#" }
    };
    public Dictionary<string, string> SiteSession { get; set; } = new Dictionary<string, string>
    {
        { "driver", "memory" },
        { "lifetime", "120" }
    };
    public Dictionary<string, string> SiteCookie { get; set; } = new Dictionary<string, string>
    {
        { "domain", "" },
        { "path", "/" },
        { "secure", "false" },
        { "httponly", "true" }
    };
    public Dictionary<string, string> SiteDebug { get; set; } = new Dictionary<string, string>
    {
        { "error", "true" },
        { "log", "true" }
    };
    public Dictionary<string, string> SiteLog { get; set; } = new Dictionary<string, string>
    {
        { "path", "/logs/" },
        { "level", "debug" }
    };
    public Dictionary<string, string> SiteApi { get; set; } = new Dictionary<string, string>
    {
        { "version", "v1" },
        { "prefix", "/api" }
    };
    public Dictionary<string, string> SiteThemeConfig { get; set; } = new Dictionary<string, string>
    {
        { "primary", "#007bff" },
        { "secondary", "#6c757d" },
        { "success", "#28a745" },
        { "danger", "#dc3545" },
        { "warning", "#ffc107" },
        { "info", "#17a2b8" },
        { "light", "#f8f9fa" },
        { "dark", "#343a40" }
    };
    public Dictionary<string, string> SiteFooter { get; set; } = new Dictionary<string, string>
    {
        { "copyright", "Copyright © 2023 LSPMPI. All rights reserved." },
        { "address", "Indonesia" },
        { "phone", "+62 812 3456 7890" },
        { "email", "admin@lspmpi.com" }
    };
    public Dictionary<string, string> SiteHeader { get; set; } = new Dictionary<string, string>
    {
        { "logo", "/images/logo.png" },
        { "title", "LSPMPI" }
    };
    public Dictionary<string, string> SiteMeta { get; set; } = new Dictionary<string, string>
    {
        { "viewport", "width=device-width, initial-scale=1.0" },
        { "charset", "UTF-8" },
        { "robots", "index, follow" }
    };
}