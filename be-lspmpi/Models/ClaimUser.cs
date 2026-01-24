namespace be_lspmpi.Models
{
    public class ClaimUser
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Username { get; set; }
        public string? ProfileName { get; set; }
    }
}