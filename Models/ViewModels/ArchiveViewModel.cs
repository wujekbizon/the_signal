using TheSignal.Models.Enums;

namespace TheSignal.Models.ViewModels;

public class ArchiveViewModel
{
    public List<LogEntryViewModel> Logs { get; set; } = new();
    public int TotalLogs { get; set; }
    public int ActiveMissions { get; set; }
    public int LostSignals { get; set; }
    public string DatabaseStatus { get; set; } = "ONLINE";
}

public class LogEntryViewModel
{
    public int Id { get; set; }
    public string PersonnelId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public CharacterClass Class { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public LogStatus Status { get; set; }
    public string CurrentLocation { get; set; } = string.Empty;
    public int LocationsDiscovered { get; set; }
    public TimeSpan TimeSurvived { get; set; }
    public DateTime LastTransmission { get; set; }
    public int Health { get; set; }
    public int Sanity { get; set; }
    public string BriefSummary { get; set; } = string.Empty;
    public List<string> KeyEvents { get; set; } = new();
    public bool IsCorrupted { get; set; }
}

public enum LogStatus
{
    Active,
    MIA,
    KIA,
    Corrupted,
    Escaped
}
