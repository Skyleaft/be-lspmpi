namespace be_lspmpi.Dto
{
    public class ArticleTagsDto
    {
        public int ArticleId { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }
}