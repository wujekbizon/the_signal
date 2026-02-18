using TheSignal.Models.Enums;

namespace TheSignal.Models.Entities;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BaseDescription { get; set; } = string.Empty;
    public string ContentKey { get; set; } = string.Empty;
    
    public BiomeType Biome { get; set; }
    public TimeOfDay DefaultTime { get; set; }
    public AtmosphereType DefaultAtmosphere { get; set; }
    public MoodType DefaultMood { get; set; }
    public ThreatLevel ThreatLevel { get; set; }
    
    public bool IsClassExclusive { get; set; }
    public CharacterClass? RequiredClass { get; set; }
    
    public List<string> KeyElements { get; set; } = new();
    
    [System.Text.Json.Serialization.JsonIgnore]
    public List<LocationExit> Exits { get; set; } = new();
    
    public string? GeneratedDescription { get; set; }
    public string? ContextHash { get; set; }
    public DateTime? LastGenerated { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int Deck { get; set; } = 1;
    public string Sector { get; set; } = string.Empty;
    public bool IsStatic { get; set; } = true;
    public bool IsStartingLocation { get; set; } = false;
}
