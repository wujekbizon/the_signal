namespace TheSignal.Models.Entities;

public class DiscoveredInteractable
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public string InteractableId { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
    public bool HasInteracted { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public Character? Character { get; set; }
}
