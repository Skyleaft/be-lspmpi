using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Services;

namespace be_lspmpi.Endpoints
{
    public static class ArticleCategoryEndpoint
    {
        public static void MapArticleCategoryEndpoints(this WebApplication app)
        {
            var categories = app.MapGroup("/api/article-categories").WithTags("Article Categories");

            categories.MapPost("/find", async (FindRequest request, IArticleCategoryService categoryService) =>
                await categoryService.Find(request))
            .WithName("FindArticleCategories")
            .WithSummary("Find article categories")
            .WithDescription("Search article categories with criteria")
            .Produces<PaginatedResponse<ArticleCategory>>(200)
            .AllowAnonymous();

            categories.MapGet("/", async (IArticleCategoryService categoryService) =>
                await categoryService.GetAll())
            .WithName("GetAllArticleCategories")
            .WithSummary("Get all article categories")
            .WithDescription("Retrieve all article categories")
            .Produces<IEnumerable<ArticleCategory>>(200)
            .AllowAnonymous();

            categories.MapGet("/{id}", async (int id, IArticleCategoryService categoryService) =>
                {
                    var category = await categoryService.Get(id);
                    return category is not null ? Results.Ok(category) : Results.NotFound();
                })
                .WithName("GetArticleCategory")
                .WithSummary("Get article category by ID")
                .WithDescription("Retrieve article category by ID")
                .Produces<ArticleCategory>(200)
                .Produces(404)
                .AllowAnonymous();

            categories.MapPost("/", async (CreateArticleCategoryDto request, IArticleCategoryService categoryService) =>
            {
                var result = await categoryService.Create(request);
                return result.Success ? Results.Created() : Results.BadRequest(result.Message);
            })
            .WithName("CreateArticleCategory")
            .WithSummary("Create new article category")
            .WithDescription("Create a new article category")
            .Produces(201)
            .Produces(400)
            .RequireAuthorization();

            categories.MapPut("/{id}", async (int id, UpdateArticleCategoryDto categoryDto, IArticleCategoryService categoryService) =>
            {
                var result = await categoryService.Update(id, categoryDto);
                return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
            })
            .WithName("UpdateArticleCategory")
            .WithSummary("Update article category")
            .WithDescription("Update article category information")
            .Produces(204)
            .Produces(400)
            .RequireAuthorization();

            categories.MapDelete("/{id}", async (int id, IArticleCategoryService categoryService) =>
            {
                var result = await categoryService.Delete(id);
                return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
            })
            .WithName("DeleteArticleCategory")
            .WithSummary("Delete article category")
            .WithDescription("Delete article category")
            .Produces(204)
            .Produces(400)
            .RequireAuthorization();
        }
    }
}