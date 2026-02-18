using TheSignal.Models.Enums;

namespace TheSignal.Models.Entities;

public class LocationExit
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public Location? Location { get; set; }
    
    public int TargetLocationId { get; set; }
    public string Direction { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public List<ExitCondition> Conditions { get; set; } = new();
    
    public CharacterClass? RequiredClass { get; set; }
    public int RequiredTechnical { get; set; }
    public int RequiredScience { get; set; }
    public int RequiredCombat { get; set; }
    
    public int GapSize { get; set; } = 0;
    public string GapType { get; set; } = string.Empty;
    public int? GapMinSteps { get; set; }
    public int? GapMaxSteps { get; set; }
}
