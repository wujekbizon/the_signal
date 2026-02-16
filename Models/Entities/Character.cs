using TheSignal.Models.Enums;

namespace TheSignal.Models.Entities;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CharacterClass Class { get; set; }
    
    public int CurrentLocationId { get; set; }
    public Location? CurrentLocation { get; set; }
    
    public int Level { get; set; } = 1;
    public int Experience { get; set; }
    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
    public int Oxygen { get; set; } = 100;
    public int MaxOxygen { get; set; } = 100;
    
    public int Sanity { get; set; } = 100;
    public int MaxSanity { get; set; } = 100;
    
    public int Technical { get; set; } = 10;
    public int Combat { get; set; } = 10;
    public int Medical { get; set; } = 10;
    public int Science { get; set; } = 10;
    public int Stealth { get; set; } = 10;
    public int Leadership { get; set; } = 10;
    
    public List<InventoryItem> Inventory { get; set; } = new();
    public List<StatusEffect> ActiveEffects { get; set; } = new();
    public List<CharacterQuest> Quests { get; set; } = new();
    public List<string> DiscoveredClues { get; set; } = new();
    public List<string> RecentActions { get; set; } = new();
    
    public int Credits { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastPlayedAt { get; set; } = DateTime.UtcNow;
}
