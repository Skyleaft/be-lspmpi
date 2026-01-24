namespace be_lspmpi.Services
{
    public interface IThumbnailService
    {
        Task<string> CompressToWebPAsync(IFormFile file);
        void DeleteOldThumbnail(string? fileName);
    }
}