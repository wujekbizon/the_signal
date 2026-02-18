namespace TheSignal.Models.Entities;

public class ProceduralRoom
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public Character? Character { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioDescription { get; set; } = string.Empty;
    public string EnvironmentalDescription { get; set; } = string.Empty;
    
    public int TwistLevel { get; set; }
    public bool IsAltered => TwistLevel > 1;
    
    public int GapSequenceNumber { get; set; }
    public string GapType { get; set; } = string.Empty;
    
    public int HealthChange { get; set; }
    public int SanityChange { get; set; }
    public int OxygenChange { get; set; }
    public string? FoundItemName { get; set; }
    public string? DiscoveryClue { get; set; }
    
    public int? SourceAnchorId { get; set; }
    public int? TargetAnchorId { get; set; }
    
    public bool IsBacktrack { get; set; }
    public int VisitCount { get; set; } = 1;
    
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastVisitedAt { get; set; }
    
    public string ContextHash { get; set; } = string.Empty;
}
