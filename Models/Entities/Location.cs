using ChroniclesOfTheVoid.Models.Enums;

namespace ChroniclesOfTheVoid.Models.Entities;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BaseDescription { get; set; } = string.Empty;
    
    public BiomeType Biome { get; set; }
    public TimeOfDay DefaultTime { get; set; }
    public AtmosphereType DefaultAtmosphere { get; set; }
    public MoodType DefaultMood { get; set; }
    public ThreatLevel ThreatLevel { get; set; }
    
    public List<string> KeyElements { get; set; } = new();
    public List<LocationExit> Exits { get; set; } = new();
    
    public string? GeneratedDescription { get; set; }
    public string? ContextHash { get; set; }
    public DateTime? LastGenerated { get; set; }
    
    public bool IsActive { get; set; } = true;
}
