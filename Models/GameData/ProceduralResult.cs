namespace TheSignal.Models.GameData;

public class ProceduralResult
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioDescription { get; set; } = string.Empty;
    public string EnvironmentalDescription { get; set; } = string.Empty;
    
    public int HealthChange { get; set; }
    public int SanityChange { get; set; }
    public int OxygenChange { get; set; }
    public string? FoundItemName { get; set; }
    public string? DiscoveryClue { get; set; }
    public string EventType { get; set; } = "Hazard";
}
