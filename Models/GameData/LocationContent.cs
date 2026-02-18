namespace TheSignal.Models.GameData;

public class LocationContent
{
    public int LocationId { get; set; }
    public string ContentKey { get; set; } = string.Empty;
    
    public string Visual { get; set; } = string.Empty;
    public string Audio { get; set; } = string.Empty;
    public string Atmosphere { get; set; } = string.Empty;
    public string Psychological { get; set; } = string.Empty;
    
    public List<InteractableInfo> Interactables { get; set; } = new();
}

public class InteractableInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public bool IsKeyItem { get; set; }
    public string? RequiredClass { get; set; }
}
