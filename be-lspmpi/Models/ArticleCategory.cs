using System.ComponentModel.DataAnnotations;

namespace be_lspmpi.Models
{
    public class ArticleCategory
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}