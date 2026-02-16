namespace TheSignal.Models.Entities;

public class GeneratedNarrative
{
    public int Id { get; set; }
    public string ContextHash { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public Location? Location { get; set; }
    
    public string GeneratedText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    public int AccessCount { get; set; } = 1;
    
    public string? ContextJson { get; set; }
}
