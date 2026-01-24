namespace be_lspmpi.Dto
{
    public class CreateArticleDto
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public int CategoryId { get; set; }
        public string? Thumbnail { get; set; }
    }
}