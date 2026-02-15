namespace ChroniclesOfTheVoid.Models.Entities;

public class QuestObjective
{
    public int Id { get; set; }
    public int QuestId { get; set; }
    public Quest? Quest { get; set; }
    
    public int OrderIndex { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ObjectiveType { get; set; } = string.Empty;
    public string TargetValue { get; set; } = string.Empty;
    public int RequiredAmount { get; set; } = 1;
}
