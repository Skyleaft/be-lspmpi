using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace be_lspmpi.Services
{
    public class AvatarService : IAvatarService
    {
        public async Task<string> GenerateAvatarAsync(string name, Guid userId)
        {
            var initials = GetInitials(name);
            var fileName = $"avatar_{userId}.svg";
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "profile-photos");
            Directory.CreateDirectory(uploadsDir);
            var filePath = Path.Combine(uploadsDir, fileName);

            await GenerateAvatarSvg(initials, filePath);
            return fileName;
        }

        private static string GetInitials(string name)
        {
            var words = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return "U";
            if (words.Length == 1) return words[0][0].ToString().ToUpper();
            return $"{words[0][0]}{words[^1][0]}".ToUpper();
        }

        public void DeleteOldProfilePhoto(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "profile-photos");
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

        public async Task<string> CompressToWebPAsync(IFormFile file, Guid userId)
        {
            var fileName = $"{userId}_{Guid.NewGuid()}.webp";
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "profile-photos");
            Directory.CreateDirectory(uploadsDir);
            var outputPath = Path.Combine(uploadsDir, fileName);

            using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);

            await image.SaveAsWebpAsync(outputPath, new WebpEncoder
            {
                Quality = 90
            });

            return fileName;
        }

        private static async Task GenerateAvatarSvg(string initials, string filePath)
        {
            var svg = $@"<svg width=""200"" height=""200"" xmlns=""http://www.w3.org/2000/svg"">
  <rect width=""200"" height=""200"" fill=""#3498db""/>
  <text x=""100"" y=""120"" font-family=""Arial, sans-serif"" font-size=""60"" font-weight=""bold"" fill=""white"" text-anchor=""middle"">{initials}</text>
</svg>";

            await File.WriteAllTextAsync(filePath, svg);
        }
    }
}