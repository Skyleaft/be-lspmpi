using be_lspmpi.Dto;
using be_lspmpi.Models;
using be_lspmpi.Services;

namespace be_lspmpi.Endpoints
{
    public static class CompetencySchemaEndpoint
    {
        public static void MapCompetencySchemaEndpoints(this WebApplication app)
        {
            var competencySchemas = app.MapGroup("/api/competency-schemas").WithTags("CompetencySchemas");

            competencySchemas.MapPost("/find", async (FindRequest request, ICompetencySchemaService competencySchemaService) =>
                await competencySchemaService.FindCompetencySchemasAsync(request))
            .WithName("FindCompetencySchemas")
            .WithSummary("Find competency schemas")
            .WithDescription("Search competency schemas with criteria")
            .Produces<PaginatedResponse<CompetencySchema>>(200)
            .AllowAnonymous();

            competencySchemas.MapGet("/", async (ICompetencySchemaService competencySchemaService) =>
                await competencySchemaService.GetAllCompetencySchemasAsync())
            .WithName("GetAllCompetencySchemas")
            .WithSummary("Get all competency schemas")
            .WithDescription("Retrieve all competency schemas")
            .Produces<IEnumerable<CompetencySchema>>(200)
            .AllowAnonymous();

            competencySchemas.MapGet("/{id}", async (int id, ICompetencySchemaService competencySchemaService) =>
            {
                var competencySchema = await competencySchemaService.GetCompetencySchemaByIdAsync(id);
                return competencySchema is not null ? Results.Ok(competencySchema) : Results.NotFound();
            })
            .WithName("GetCompetencySchema")
            .WithSummary("Get competency schema by ID")
            .WithDescription("Retrieve competency schema by ID")
            .Produces<CompetencySchema>(200)
            .Produces(404)
            .AllowAnonymous();

            competencySchemas.MapPost("/", async (CompetencySchema competencySchema, ICompetencySchemaService competencySchemaService) =>
            {
                try
                {
                    var result = await competencySchemaService.CreateCompetencySchemaAsync(competencySchema);
                    return Results.Created($"/api/competency-schemas/{result.Id}", result);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CreateCompetencySchema")
            .WithSummary("Create new competency schema")
            .WithDescription("Create a new competency schema")
            .Produces<CompetencySchema>(201)
            .Produces(400)
            .RequireAuthorization();

            competencySchemas.MapPut("/{id}", async (int id, CompetencySchema competencySchema, ICompetencySchemaService competencySchemaService) =>
            {
                try
                {
                    competencySchema.Id = id;
                    var result = await competencySchemaService.UpdateCompetencySchemaAsync(competencySchema);
                    return Results.Ok(result);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("UpdateCompetencySchema")
            .WithSummary("Update competency schema")
            .WithDescription("Update competency schema information")
            .Produces<CompetencySchema>(200)
            .Produces(400)
            .RequireAuthorization();

            competencySchemas.MapDelete("/{id}", async (int id, ICompetencySchemaService competencySchemaService) =>
            {
                try
                {
                    var result = await competencySchemaService.DeleteCompetencySchemaAsync(id);
                    return result ? Results.NoContent() : Results.NotFound();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("DeleteCompetencySchema")
            .WithSummary("Delete competency schema")
            .WithDescription("Delete competency schema")
            .Produces(204)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();
        }
    }
}