namespace be_lspmpi.Models;

public class CompetencySchema
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public List<string> Competencies { get; set; } = new List<string>();
    public string Image { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}