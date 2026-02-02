namespace be_lspmpi.Models
{
    public class ArticleTagMapping
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int ArticleTagId { get; set; }
        public ArticleTag ArticleTag { get; set; }
    }
}