namespace ChroniclesOfTheVoid.Models.Entities;

public class Quest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<QuestObjective> Objectives { get; set; } = new();
    public int? RewardExperience { get; set; }
    public int? RewardCredits { get; set; }
}
