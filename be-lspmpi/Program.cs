using be_lspmpi.Data;
using be_lspmpi.Endpoints;
using be_lspmpi.Json;
using be_lspmpi.Repositories;
using be_lspmpi.Services;
using Core.Systems;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDBContext, AppDbContext>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IArticleRepo, ArticleRepo>();
builder.Services.AddScoped<IArticleCategoryRepo, ArticleCategoryRepo>();

builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IArticleService, ArticleService>();
builder.Services.AddTransient<IArticleCategoryService, ArticleCategoryService>();
builder.Services.AddTransient<IAvatarService, AvatarService>();
builder.Services.AddTransient<IThumbnailService, ThumbnailService>();
builder.Services.AddTransient<IClaimService, ClaimService>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        var cookieConfig = builder.Configuration.GetSection("Cookie");
        options.Cookie.Name = cookieConfig.GetValue<string>("Name") ?? "auth";
        options.ExpireTimeSpan = TimeSpan.FromDays(cookieConfig.GetValue<int>("ExpireDays", 7));
        options.SlidingExpiration = cookieConfig.GetValue<bool>("SlidingExpiration", true);
        options.Cookie.HttpOnly = cookieConfig.GetValue<bool>("HttpOnly", true);
        var isSecure = cookieConfig.GetValue<bool>("Secure", false);
        options.Cookie.SecurePolicy = isSecure ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = Enum.Parse<SameSiteMode>(cookieConfig.GetValue<string>("SameSite") ?? "Lax");

        // Return 401 instead of redirecting for API endpoints
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    var corsConfig = builder.Configuration.GetSection("Cors");
    options.AddDefaultPolicy(policy =>
    {
        var origins = corsConfig.GetSection("AllowedOrigins").Get<string[]>() ?? ["*"];
        var methods = corsConfig.GetSection("AllowedMethods").Get<string[]>() ?? ["*"];
        var headers = corsConfig.GetSection("AllowedHeaders").Get<string[]>() ?? ["*"];
        var allowCredentials = corsConfig.GetValue<bool>("AllowCredentials");

        if (origins.Contains("*"))
        {
            policy.AllowAnyOrigin();
            // Cannot use AllowCredentials with AllowAnyOrigin
        }
        else
        {
            policy.WithOrigins(origins);
            if (allowCredentials)
                policy.AllowCredentials();
        }

        if (methods.Contains("*"))
            policy.AllowAnyMethod();
        else
            policy.WithMethods(methods);

        if (headers.Contains("*"))
            policy.AllowAnyHeader();
        else
            policy.WithHeaders(headers);
    });
});

#if !AOT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "LSP MPI API", Version = "v1" });
    c.AddSecurityDefinition("Cookie", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Name = "Cookie",
        Description = "Cookie authentication"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Cookie", document)] = []
    });

});
#endif


#if AOT
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
#endif

var app = builder.Build();

#if !AOT
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LSP MPI API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "LSP MPI API Documentation";
    });
}
#endif

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();



app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapArticleEndpoints();
app.MapArticleCategoryEndpoints();
app.MapGet("/ping", () => $"Phoonk!! - {DateTime.Now}");

app.MapGet("/openapi.json", () => Results.File("openapi.json", "application/json"));

app.Run();

