using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using Microsoft.AspNetCore.Http;

namespace be_lspmpi.Services;

public class FileService : IFileService
{
    public async Task<string> SaveImageAsync(IFormFile file, string subdir)
    {
        var fileName = $"{Guid.NewGuid()}.webp";
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", subdir);
        Directory.CreateDirectory(uploadsDir);
        var outputPath = Path.Combine(uploadsDir, fileName);

        using var inputStream = file.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);

        await image.SaveAsWebpAsync(outputPath, new WebpEncoder
        {
            Quality = 90
        });

        return $"{subdir}/{fileName}";
    }

    public void DeleteFile(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath)) return;

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", relativePath);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                // Ignore deletion errors or log them
            }
        }
    }
}
