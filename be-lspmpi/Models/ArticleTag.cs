using System.ComponentModel.DataAnnotations;

namespace be_lspmpi.Models
{
    public class ArticleTag
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ArticleTagMapping> ArticleTagMappings { get; set; } = new List<ArticleTagMapping>();
    }
}