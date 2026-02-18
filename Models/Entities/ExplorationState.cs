using TheSignal.Models.Enums;

namespace TheSignal.Models.Entities;

public class ExplorationState
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public Character? Character { get; set; }
    
    public int StepsFromLastAnchor { get; set; }
    public int CurrentTwistLevel { get; set; }
    public int TargetStaticLocationId { get; set; }
    
    public int? PreviousRoomId { get; set; }
    public ProceduralRoom? PreviousRoom { get; set; }
    
    public bool IsInProceduralGap { get; set; }
    public int RemainingGapSteps { get; set; }
    
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    public bool IsConsumed => CurrentTwistLevel >= 5;
}
