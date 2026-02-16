namespace be_lspmpi.Models;

public class WebSettingPublic
{
    public string SiteName { get; set; } = string.Empty;
    public string SiteDescription { get; set; } = string.Empty;
    public string SiteUrl { get; set; } = string.Empty;
    public string SiteKeywords { get; set; } = string.Empty;
    public string SiteAuthor { get; set; } = string.Empty;
    public string SiteVersion { get; set; } = string.Empty;
    public string SiteCopyright { get; set; } = string.Empty;
    public string SiteEmail { get; set; } = string.Empty;
    public string SitePhone { get; set; } = string.Empty;
    public string SiteAddress { get; set; } = string.Empty;
    public string SiteLogo { get; set; } = string.Empty;
    public string SiteFavicon { get; set; } = string.Empty;
    public string SiteTheme { get; set; } = string.Empty;
    public string SiteLanguage { get; set; } = string.Empty;
    public string SiteTimezone { get; set; } = string.Empty;
    public bool SiteStatus { get; set; }
    public bool SiteMaintenance { get; set; }
    public string SiteMaintenanceMessage { get; set; } = string.Empty;
    public Dictionary<string, string> SiteSocialMedia { get; set; } = new();
    public Dictionary<string, string> SiteSeo { get; set; } = new();
    public Dictionary<string, string> SiteThemeConfig { get; set; } = new();
    public Dictionary<string, string> SiteFooter { get; set; } = new();
    public Dictionary<string, string> SiteHeader { get; set; } = new();
    public Dictionary<string, string> SiteMeta { get; set; } = new();
}
