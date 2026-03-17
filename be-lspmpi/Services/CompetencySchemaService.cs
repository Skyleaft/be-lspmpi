using be_lspmpi.Models;
using be_lspmpi.Repositories;
using be_lspmpi.Dto;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace be_lspmpi.Services;

public class CompetencySchemaService : ICompetencySchemaService
{
    private readonly ICompetencySchemaRepository _repository;
    private readonly IFileService _fileService;

    public CompetencySchemaService(ICompetencySchemaRepository repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
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

        var existing = await _repository.GetByIdAsync(id);
        if (existing != null)
        {
            _fileService.DeleteFile(existing.ImageUrl);
            return await _repository.DeleteAsync(id);
        }

        return false;
    }

    public async Task<PaginatedResponse<CompetencySchema>> FindCompetencySchemasAsync(FindRequest request)
    {
        if (request.Page <= 0)
            request.Page = 1;

        if (request.PageSize <= 0)
            request.PageSize = 10;

        return await _repository.Find(request);
    }

    public async Task<string> UploadImageUrlAsync(int id, IFormFile file)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("Competency schema not found");

        // Delete old image if it exists
        _fileService.DeleteFile(existing.ImageUrl);

        // Save new image
        var relativePath = await _fileService.SaveImageAsync(file, "competency-schemas");

        existing.ImageUrl = relativePath;
        await _repository.UpdateAsync(existing);

        return relativePath;
    }
}