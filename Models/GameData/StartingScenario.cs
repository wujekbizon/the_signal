using TheSignal.Models.Enums;

namespace TheSignal.Models.GameData;

public class StartingScenario
{
    public CharacterClass CharacterClass { get; set; }
    public int StartingLocationId { get; set; }
    public string OpeningNarrative { get; set; } = string.Empty;
    public string StartingSituation { get; set; } = string.Empty;
    public string WhyInCryo { get; set; } = string.Empty;
    public List<string> InitialItems { get; set; } = new();
    public string FirstObjective { get; set; } = string.Empty;
    public string ClassHookTechnical { get; set; } = string.Empty;
    public string ClassHookPersonal { get; set; } = string.Empty;
}
