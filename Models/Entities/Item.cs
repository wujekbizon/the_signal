using ChroniclesOfTheVoid.Models.Enums;

namespace ChroniclesOfTheVoid.Models.Entities;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public int? Value { get; set; }
    public bool IsQuestItem { get; set; }
    
    public int? Damage { get; set; }
    public int? Defense { get; set; }
    public int? UsesRemaining { get; set; }
}
