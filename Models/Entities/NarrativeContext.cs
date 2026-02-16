using TheSignal.Models.Enums;

namespace TheSignal.Models.Entities;

public class NarrativeContext
{
    public string ContextHash => GenerateContextHash();
    
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationBaseDescription { get; set; } = string.Empty;
    public BiomeType Biome { get; set; }
    public List<string> LocationKeyElements { get; set; } = new();
    
    public TimeOfDay CurrentTime { get; set; }
    public AtmosphereType CurrentAtmosphere { get; set; }
    public ThreatLevel CurrentThreat { get; set; }
    public List<string> WorldEvents { get; set; } = new();
    public HorrorEventType LastEvent { get; set; }
    
    public int CharacterId { get; set; }
    public string CharacterName { get; set; } = string.Empty;
    public int HealthPercent => MaxHealth > 0 ? (Health * 100) / MaxHealth : 100;
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Sanity { get; set; }
    public int MaxSanity { get; set; }
    public List<string> ActiveStatusEffects { get; set; } = new();
    public List<string> KeyInventoryItems { get; set; } = new();
    public List<string> ActiveQuestTitles { get; set; } = new();
    public List<string> RecentActions { get; set; } = new();
    public List<string> DiscoveredClues { get; set; } = new();
    
    public MoodType CurrentMood { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    private string GenerateContextHash()
    {
        var hashData = $"{LocationId}_{CurrentTime}_{CurrentAtmosphere}_{CurrentThreat}_{HealthPercent}_{Sanity}_{string.Join(",", ActiveStatusEffects)}_{string.Join(",", WorldEvents)}_{LastEvent}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(hashData));
    }
    
    public static NarrativeContext Create(Location location, Character character, 
        TimeOfDay time, AtmosphereType atmosphere, ThreatLevel threat, List<string>? worldEvents = null)
    {
        return new NarrativeContext
        {
            LocationId = location.Id,
            LocationName = location.Name,
            LocationBaseDescription = location.BaseDescription,
            Biome = location.Biome,
            LocationKeyElements = location.KeyElements,
            CurrentTime = time,
            CurrentAtmosphere = atmosphere,
            CurrentThreat = threat,
            WorldEvents = worldEvents ?? new List<string>(),
            CharacterId = character.Id,
            CharacterName = character.Name,
            Health = character.Health,
            MaxHealth = character.MaxHealth,
            Sanity = character.Sanity,
            MaxSanity = character.MaxSanity,
            ActiveStatusEffects = character.ActiveEffects.Select(e => e.Name).ToList(),
            KeyInventoryItems = character.Inventory
                .Where(i => i.Item?.IsQuestItem == true)
                .Select(i => i.Item!.Name)
                .ToList(),
            ActiveQuestTitles = character.Quests
                .Where(q => q.Status == QuestStatus.Active)
                .Select(q => q.Quest?.Title ?? "Unknown")
                .ToList(),
            RecentActions = character.RecentActions.TakeLast(3).ToList(),
            DiscoveredClues = character.DiscoveredClues,
            CurrentMood = DetermineMood(character, location, threat)
        };
    }
    
    private static MoodType DetermineMood(Character character, Location location, ThreatLevel threat)
    {
        if (threat == ThreatLevel.ActiveHunt || threat == ThreatLevel.Critical)
            return MoodType.Terrifying;
        
        if (character.Sanity < 50 || character.ActiveEffects.Any(e => e.Type == EffectType.Psychological || e.Type == EffectType.Infection))
            return MoodType.Paranoid;
        
        if (character.Health < character.MaxHealth * 0.3)
            return MoodType.Desperate;
        
        if (threat == ThreatLevel.Contact || threat == ThreatLevel.Disturbance)
            return MoodType.Suspenseful;
            
        return location.DefaultMood;
    }
}
