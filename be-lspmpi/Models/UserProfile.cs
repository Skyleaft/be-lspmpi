using System.ComponentModel.DataAnnotations;

namespace be_lspmpi.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(255)]
        public string? ProfilePicture { get; set; }
    }
}