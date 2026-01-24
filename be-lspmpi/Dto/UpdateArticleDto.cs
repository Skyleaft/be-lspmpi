namespace be_lspmpi.Dto
{
    public class UpdateArticleDto
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public int CategoryId { get; set; }
        public string? Thumbnail { get; set; }
        public bool IsPublished { get; set; }
    }
}