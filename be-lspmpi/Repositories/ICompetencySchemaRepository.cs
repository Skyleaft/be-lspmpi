using be_lspmpi.Models;
using be_lspmpi.Dto;

namespace be_lspmpi.Repositories;

public interface ICompetencySchemaRepository
{
    Task<IEnumerable<CompetencySchema>> GetAllAsync();
    Task<CompetencySchema?> GetByIdAsync(int id);
    Task<CompetencySchema> CreateAsync(CompetencySchema competencySchema);
    Task<CompetencySchema> UpdateAsync(CompetencySchema competencySchema);
    Task<bool> DeleteAsync(int id);
    Task<PaginatedResponse<CompetencySchema>> Find(FindRequest request);
}