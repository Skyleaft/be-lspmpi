using be_lspmpi.Models;
using be_lspmpi.Dto;

namespace be_lspmpi.Services;

public interface ICompetencySchemaService
{
    Task<IEnumerable<CompetencySchema>> GetAllCompetencySchemasAsync();
    Task<CompetencySchema?> GetCompetencySchemaByIdAsync(int id);
    Task<CompetencySchema> CreateCompetencySchemaAsync(CompetencySchema competencySchema);
    Task<CompetencySchema> UpdateCompetencySchemaAsync(CompetencySchema competencySchema);
    Task<bool> DeleteCompetencySchemaAsync(int id);
    Task<PaginatedResponse<CompetencySchema>> FindCompetencySchemasAsync(FindRequest request);
}