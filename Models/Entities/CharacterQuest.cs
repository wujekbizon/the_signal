using ChroniclesOfTheVoid.Models.Enums;

namespace ChroniclesOfTheVoid.Models.Entities;

public class CharacterQuest
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character? Character { get; set; }
    
    public int QuestId { get; set; }
    public Quest? Quest { get; set; }
    
    public QuestStatus Status { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int CurrentObjectiveIndex { get; set; }
}
