namespace ChroniclesOfTheVoid.Models.Entities;

public class LocationExit
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public Location? Location { get; set; }
    
    public int TargetLocationId { get; set; }
    public string Direction { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public List<ExitCondition> Conditions { get; set; } = new();
}
