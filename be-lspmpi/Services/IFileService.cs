using Microsoft.AspNetCore.Http;

namespace be_lspmpi.Services;

public interface IFileService
{
    Task<string> SaveImageAsync(IFormFile file, string subdir);
    void DeleteFile(string relativePath);
}
