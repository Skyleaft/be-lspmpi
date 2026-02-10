using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Services;

namespace be_lspmpi.Endpoints
{
    public static class ArticleEndpoint
    {
        public static void MapArticleEndpoints(this WebApplication app)
        {
            var articles = app.MapGroup("/api/articles").WithTags("Articles");

            articles.MapPost("/find", async (FindRequest request, IArticleService articleService) =>
                await articleService.Find(request))
            .WithName("FindArticles")
            .WithSummary("Find articles")
            .WithDescription("Search articles with criteria")
            .Produces<PaginatedResponse<Article>>(200)
            .AllowAnonymous();
            
            articles.MapGet("/latest", async (IArticleService articleService) =>
                {
                    var articles = await articleService.GetLatest();
                    return Results.Ok(articles.Select(a => new
                    {
                        a.Id,
                        a.Title,
                        a.Content,
                        a.Author,
                        a.Slug,
                        a.Thumbnail,
                        a.IsPublished,
                        a.CreatedAt,
                        a.UpdatedAt,
                        Category = a.Category != null ? new { a.Category.Id, a.Category.Name } : null,
                        Tags = a.ArticleTagMappings?.Select(m => new { m.ArticleTag.Id, m.ArticleTag.Name }).ToList()
                    }));
                })
                .WithName("GetLatest Article")
                .WithSummary("GetLatest Article")
                .WithDescription("Retrieve Latest Article")
                .Produces<List<Article>>(200)
                .AllowAnonymous();

            articles.MapGet("/{id}", async (int id, IArticleService articleService) =>
                {
                    var article = await articleService.Get(id);
                    return article is not null ? Results.Ok(article) : Results.NotFound();
                })
                .WithName("GetArticle")
                .WithSummary("Get article by ID")
                .WithDescription("Retrieve article by ID")
                .Produces<Article>(200)
                .Produces(404)
                .AllowAnonymous();

            articles.MapGet("/slug/{slug}", async (string slug, IArticleService articleService) =>
                {
                    var article = await articleService.GetBySlug(slug);
                    return article is not null ? Results.Ok(article) : Results.NotFound();
                })
                .WithName("GetArticleBySlug")
                .WithSummary("Get article by slug")
                .WithDescription("Retrieve article by slug")
                .Produces<Article>(200)
                .Produces(404)
                .AllowAnonymous();

            articles.MapPost("/", async (CreateArticleDto request, IArticleService articleService) =>
            {
                var result = await articleService.Create(request);
                return result.Success ? Results.Created() : Results.BadRequest(result.Message);
            })
            .WithName("CreateArticle")
            .WithSummary("Create new article")
            .WithDescription("Create a new article with thumbnail filename")
            .Produces(201)
            .Produces(400)
            .RequireAuthorization();

            articles.MapPost("/with-thumbnail", async (HttpContext context, IArticleService articleService, IThumbnailService thumbnailService) =>
            {
                var form = await context.Request.ReadFormAsync();

                var dto = new CreateArticleWithThumbnailDto
                {
                    Title = form["title"],
                    Content = form["content"],
                    Author = form["author"],
                    CategoryId = int.TryParse(form["categoryId"], out var catId) ? catId : 0,
                    ThumbnailFile = form.Files["thumbnailFile"]
                };

                if (dto.ThumbnailFile != null && dto.ThumbnailFile.Length > 0)
                {
                    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp", "image/svg" };
                    if (!allowedTypes.Contains(dto.ThumbnailFile.ContentType))
                        return Results.BadRequest("Invalid file type");

                    dto.Thumbnail = await thumbnailService.CompressToWebPAsync(dto.ThumbnailFile);
                }

                var result = await articleService.Create(dto);
                return result.Success ? Results.Created() : Results.BadRequest(result.Message);
            })
            .WithName("CreateArticleWithThumbnail")
            .WithSummary("Create article with thumbnail")
            .WithDescription("Create a new article and upload thumbnail in one request")
            .DisableAntiforgery()
            .Accepts<CreateArticleWithThumbnailDto>("multipart/form-data")
            .Produces(201)
            .Produces(400)
            .RequireAuthorization();

            articles.MapPut("/{id}", async (int id, UpdateArticleDto articleDto, IArticleService articleService) =>
            {
                var result = await articleService.Update(id, articleDto);
                return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
            })
            .WithName("UpdateArticle")
            .WithSummary("Update article")
            .WithDescription("Update article information")
            .Produces(204)
            .Produces(400)
            .RequireAuthorization();

            articles.MapDelete("/{id}", async (int id, IArticleService articleService, IThumbnailService thumbnailService) =>
            {
                var result = await articleService.Delete(id);
                return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
            })
            .WithName("DeleteArticle")
            .WithSummary("Delete article")
            .WithDescription("Delete article and its thumbnail")
            .Produces(204)
            .Produces(400)
            .RequireAuthorization();

            articles.MapPost("/thumbnail", async (HttpContext context, IThumbnailService thumbnailService) =>
            {
                var form = await context.Request.ReadFormAsync();
                var file = form.Files["thumbnail"];
                if (file == null || file.Length == 0) return Results.BadRequest("No file uploaded");

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp", "image/svg" };
                if (!allowedTypes.Contains(file.ContentType)) return Results.BadRequest("Invalid file type");

                var fileName = await thumbnailService.CompressToWebPAsync(file);
                return Results.Ok(new PhotoUploadResponse { FileName = fileName, Message = "Thumbnail uploaded successfully" });
            })
            .WithName("UploadThumbnail")
            .WithSummary("Upload thumbnail")
            .WithDescription("Upload article thumbnail")
            .Accepts<PhotoUploadRequest>("multipart/form-data")
            .Produces<PhotoUploadResponse>(200)
            .Produces(400)
            .RequireAuthorization();

            articles.MapGet("/thumbnail/{fileName}", async (string fileName) =>
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "thumbnails", fileName);
                if (!File.Exists(filePath)) return Results.NotFound();

                var contentType = Path.GetExtension(filePath).ToLower() switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".webp" => "image/webp",
                    _ => "application/octet-stream"
                };

                return Results.File(filePath, contentType);
            })
            .WithName("GetThumbnail")
            .WithSummary("Get thumbnail")
            .WithDescription("Retrieve article thumbnail")
            .Produces(200)
            .Produces(404)
            .AllowAnonymous();

            articles.MapGet("/{id}/tags", async (int id, IArticleService articleService) =>
            {
                var tags = await articleService.GetArticleTags(id);
                return Results.Ok(tags);
            })
            .WithName("GetArticleTags")
            .WithSummary("Get article tags")
            .WithDescription("Get all tags for an article")
            .Produces<List<ArticleTag>>(200)
            .AllowAnonymous();
            
            articles.MapPost("/{id}/tags", async (int id, List<int> tagIds, IArticleService articleService) =>
            {
                var dto = new ArticleTagsDto { ArticleId = id, TagIds = tagIds };
                var result = await articleService.AddTags(dto);
                return result.Success ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
            })
            .WithName("AddArticleTags")
            .WithSummary("Add tags to article")
            .WithDescription("Add multiple tags to an article")
            .Produces(200)
            .Produces(400)
            .RequireAuthorization();
            
            articles.MapDelete("/{id}/tags", async (int id, HttpContext context, IArticleService articleService) =>
            {
                var tagIdsParam = context.Request.Query["tagIds"].ToString();
                if (string.IsNullOrEmpty(tagIdsParam))
                    return Results.BadRequest("tagIds parameter is required");
                
                var tagIds = tagIdsParam.Split(',').Select(int.Parse).ToList();
                var dto = new ArticleTagsDto { ArticleId = id, TagIds = tagIds };
                var result = await articleService.RemoveTags(dto);
                return result.Success ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
            })
            .WithName("RemoveArticleTags")
            .WithSummary("Remove tags from article")
            .WithDescription("Remove multiple tags from an article. Use ?tagIds=1,2,3 format")
            .Produces(200)
            .Produces(400)
            .RequireAuthorization();
        }
    }
}