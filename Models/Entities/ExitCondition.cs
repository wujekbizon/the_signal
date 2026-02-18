namespace TheSignal.Models.Entities;

public class ExitCondition
{
    public int Id { get; set; }
    public int LocationExitId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public LocationExit? LocationExit { get; set; }
    
    public string ConditionType { get; set; } = string.Empty;
    public string ConditionValue { get; set; } = string.Empty;
}
