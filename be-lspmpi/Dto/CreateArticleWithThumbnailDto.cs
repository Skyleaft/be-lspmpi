namespace be_lspmpi.Dto
{
    public class CreateArticleWithThumbnailDto : CreateArticleDto
    {
        public IFormFile? ThumbnailFile { get; set; }
    }
}