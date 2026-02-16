using TheSignal.Models.Enums;

namespace TheSignal.Models.Entities;

public class StatusEffect
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character? Character { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EffectType Type { get; set; }
    public int? DurationMinutes { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}
