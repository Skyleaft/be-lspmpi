using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Services;

namespace be_lspmpi.Endpoints
{
    public static class ArticleTagEndpoint
    {
        public static void MapArticleTagEndpoints(this WebApplication app)
        {
            var tags = app.MapGroup("/api/article-tags").WithTags("Article Tags");

            tags.MapPost("/find", async (FindRequest request, IArticleTagService tagService) =>
                await tagService.Find(request))
            .WithName("FindArticleTags")
            .WithSummary("Find article tags")
            .WithDescription("Search article tags with criteria")
            .Produces<PaginatedResponse<ArticleTag>>(200)
            .AllowAnonymous();

            tags.MapGet("/{id}", async (int id, IArticleTagService tagService) =>
                {
                    var tag = await tagService.Get(id);
                    return tag is not null ? Results.Ok(tag) : Results.NotFound();
                })
                .WithName("GetArticleTag")
                .WithSummary("Get article tag by ID")
                .WithDescription("Retrieve article tag by ID")
                .Produces<ArticleTag>(200)
                .Produces(404)
                .AllowAnonymous();

            tags.MapPost("/", async (CreateArticleTagDto request, IArticleTagService tagService) =>
            {
                var result = await tagService.Create(request);
                return result.Success ? Results.Created() : Results.BadRequest(result.Message);
            })
            .WithName("CreateArticleTag")
            .WithSummary("Create new article tag")
            .WithDescription("Create a new article tag")
            .Produces(201)
            .Produces(400)
            .RequireAuthorization();

            tags.MapPut("/{id}", async (int id, UpdateArticleTagDto tagDto, IArticleTagService tagService) =>
            {
                var result = await tagService.Update(id, tagDto);
                return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
            })
            .WithName("UpdateArticleTag")
            .WithSummary("Update article tag")
            .WithDescription("Update article tag information")
            .Produces(204)
            .Produces(400)
            .RequireAuthorization();

            tags.MapDelete("/{id}", async (int id, IArticleTagService tagService) =>
            {
                var result = await tagService.Delete(id);
                return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
            })
            .WithName("DeleteArticleTag")
            .WithSummary("Delete article tag")
            .WithDescription("Delete article tag")
            .Produces(204)
            .Produces(400)
            .RequireAuthorization();
        }
    }
}