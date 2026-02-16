using TheSignal.Models.GameData;

namespace TheSignal.Models.ViewModels;

public class CharacterCreationViewModel
{
    public List<ClassInfo> Classes { get; set; } = new();
}
