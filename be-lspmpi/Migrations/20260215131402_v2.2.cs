using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace be_lspmpi.Migrations
{
    /// <inheritdoc />
    public partial class v22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "WebSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteName = table.Column<string>(type: "text", nullable: true),
                    SiteDescription = table.Column<string>(type: "text", nullable: true),
                    SiteUrl = table.Column<string>(type: "text", nullable: true),
                    SiteKeywords = table.Column<string>(type: "text", nullable: true),
                    SiteAuthor = table.Column<string>(type: "text", nullable: true),
                    SiteVersion = table.Column<string>(type: "text", nullable: true),
                    SiteCopyright = table.Column<string>(type: "text", nullable: true),
                    SiteEmail = table.Column<string>(type: "text", nullable: true),
                    SitePhone = table.Column<string>(type: "text", nullable: true),
                    SiteAddress = table.Column<string>(type: "text", nullable: true),
                    SiteLogo = table.Column<string>(type: "text", nullable: true),
                    SiteFavicon = table.Column<string>(type: "text", nullable: true),
                    SiteTheme = table.Column<string>(type: "text", nullable: true),
                    SiteLanguage = table.Column<string>(type: "text", nullable: true),
                    SiteTimezone = table.Column<string>(type: "text", nullable: true),
                    SiteStatus = table.Column<bool>(type: "boolean", nullable: false),
                    SitePerPage = table.Column<int>(type: "integer", nullable: false),
                    SiteMaintenance = table.Column<bool>(type: "boolean", nullable: false),
                    SiteMaintenanceMessage = table.Column<string>(type: "text", nullable: true),
                    SiteSocialMedia = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteAnalytics = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteSeo = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteMail = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteUpload = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SitePayment = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteMap = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteCaptcha = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteChat = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteBackup = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteOther = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteNotification = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteSecurity = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteCache = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteSession = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteCookie = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteDebug = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteLog = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteApi = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteThemeConfig = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteFooter = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteHeader = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    SiteMeta = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebSettings");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:hstore", ",,");
        }
    }
}
