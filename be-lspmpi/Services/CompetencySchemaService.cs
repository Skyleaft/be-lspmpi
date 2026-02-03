using be_lspmpi.Models;
using be_lspmpi.Repositories;
using be_lspmpi.Dto;

namespace be_lspmpi.Services;

public class CompetencySchemaService : ICompetencySchemaService
{
    private readonly ICompetencySchemaRepository _repository;

    public CompetencySchemaService(ICompetencySchemaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CompetencySchema>> GetAllCompetencySchemasAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<CompetencySchema?> GetCompetencySchemaByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be greater than 0");

        return await _repository.GetByIdAsync(id);
    }

    public async Task<CompetencySchema> CreateCompetencySchemaAsync(CompetencySchema competencySchema)
    {
        if (string.IsNullOrWhiteSpace(competencySchema.Name))
            throw new ArgumentException("Name is required");

        if (competencySchema.Fee < 0)
            throw new ArgumentException("Fee cannot be negative");

        return await _repository.CreateAsync(competencySchema);
    }

    public async Task<CompetencySchema> UpdateCompetencySchemaAsync(CompetencySchema competencySchema)
    {
        if (competencySchema.Id <= 0)
            throw new ArgumentException("Id must be greater than 0");

        if (string.IsNullOrWhiteSpace(competencySchema.Name))
            throw new ArgumentException("Name is required");

        if (competencySchema.Fee < 0)
            throw new ArgumentException("Fee cannot be negative");

        return await _repository.UpdateAsync(competencySchema);
    }

    public async Task<bool> DeleteCompetencySchemaAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be greater than 0");

        return await _repository.DeleteAsync(id);
    }

    public async Task<PaginatedResponse<CompetencySchema>> FindCompetencySchemasAsync(FindRequest request)
    {
        if (request.Page <= 0)
            request.Page = 1;

        if (request.PageSize <= 0)
            request.PageSize = 10;

        return await _repository.Find(request);
    }
}