using System.ComponentModel.DataAnnotations;

namespace be_lspmpi.Models
{
    public class Article
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string Slug { get; set; }
        public string Content { get; set; }
        [MaxLength(100)]
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public int CategoryId { get; set; }
        [MaxLength(255)]
        public string Thumbnail { get; set; }
    }
}