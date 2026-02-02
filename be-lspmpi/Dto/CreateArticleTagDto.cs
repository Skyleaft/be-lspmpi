using System.ComponentModel.DataAnnotations;

namespace be_lspmpi.Dto
{
    public class CreateArticleTagDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
    }
}