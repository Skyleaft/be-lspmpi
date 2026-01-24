using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace be_lspmpi.Services
{
    public class ThumbnailService : IThumbnailService
    {
        public async Task<string> CompressToWebPAsync(IFormFile file)
        {
            var fileName = $"thumbnail_{Guid.NewGuid()}.webp";
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "thumbnails");
            Directory.CreateDirectory(uploadsDir);
            var outputPath = Path.Combine(uploadsDir, fileName);

            using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(800, 600),
                Mode = ResizeMode.Max
            }));

            await image.SaveAsWebpAsync(outputPath, new WebpEncoder
            {
                Quality = 85
            });

            return fileName;
        }

        public void DeleteOldThumbnail(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "thumbnails");
            var filePath = Path.Combine(uploadsDir, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    // Ignore deletion errors
                }
            }
        }
    }
}