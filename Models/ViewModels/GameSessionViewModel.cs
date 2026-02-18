using TheSignal.Models.Entities;
using TheSignal.Models.Enums;

namespace TheSignal.Models.ViewModels;

public class GameSessionViewModel
{
    public int CharacterId { get; set; }
    public string CharacterName { get; set; } = string.Empty;
    public CharacterClass CharacterClass { get; set; }
    public string CharacterClassName => CharacterClass.ToString();
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Oxygen { get; set; }
    public int MaxOxygen { get; set; }
    public int Sanity { get; set; }
    public int MaxSanity { get; set; }
    
    public int Technical { get; set; }
    public int Combat { get; set; }
    public int Medical { get; set; }
    public int Science { get; set; }
    
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationDescription { get; set; } = string.Empty;
    public string LocationVisual { get; set; } = string.Empty;
    public string LocationAudio { get; set; } = string.Empty;
    public string LocationAtmosphere { get; set; } = string.Empty;
    public string LocationPsychological { get; set; } = string.Empty;
    public BiomeType Biome { get; set; }
    public AtmosphereType Atmosphere { get; set; }
    public ThreatLevel ThreatLevel { get; set; }
    public StoryBeat CurrentStoryBeat { get; set; }
    public string? OpeningNarrative { get; set; }
    
    public List<ExitViewModel> AvailableExits { get; set; } = new();
    public List<InteractableViewModel> Interactables { get; set; } = new();
    public List<InteractableViewModel> DiscoveredInteractables { get; set; } = new();
    public List<InventoryItemViewModel> Inventory { get; set; } = new();
    public List<StatusEffectViewModel> ActiveEffects { get; set; } = new();
    public List<string> RecentActions { get; set; } = new();
    public string NarrativeText { get; set; } = string.Empty;
    public string ParsedNarrativeHtml { get; set; } = string.Empty;
    
    // Procedural Exploration
    public bool IsInProceduralGap { get; set; }
    public ProceduralRoom? ProceduralRoom { get; set; }
    public int RemainingGapSteps { get; set; }
    public int TwistLevel { get; set; }
}

public class ExitViewModel
{
    public int TargetLocationId { get; set; }
    public string Direction { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string? BlockedReason { get; set; }
}

public class InventoryItemViewModel
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public int Quantity { get; set; }
    public int? UsesRemaining { get; set; }
}

public class StatusEffectViewModel
{
    public string Name { get; set; } = string.Empty;
    public EffectType Type { get; set; }
    public int? DurationMinutes { get; set; }
    public DateTime AppliedAt { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class InteractableViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public bool IsKeyItem { get; set; }
}
