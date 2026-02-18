namespace TheSignal.Models.GameData;

public class AudioLog
{
    public int Id { get; set; }
    public string LogId { get; set; } = string.Empty;
    public int Day { get; set; }
    public string Speaker { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Transcript { get; set; } = string.Empty;
    public int? LocationId { get; set; }
    public bool IsClassSpecific { get; set; }
    public string? RequiredClass { get; set; }
}

public class DiscoveredLog
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int AudioLogId { get; set; }
    public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
}
