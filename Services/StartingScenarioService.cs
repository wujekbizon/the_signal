using TheSignal.Data;
using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Models.GameData;

namespace TheSignal.Services;

public interface IStartingScenarioService
{
    StartingScenario GetScenario(CharacterClass characterClass);
    int GetStartingLocationId(CharacterClass characterClass);
    List<Item> GetStartingItems(CharacterClass characterClass);
}

public class StartingScenarioService : IStartingScenarioService
{
    private readonly ApplicationDbContext _context;
    
    public StartingScenarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public StartingScenario GetScenario(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Engineer => SeedData.GetEngineerStartingScenario(),
            CharacterClass.SecurityOfficer => SeedData.GetSecurityStartingScenario(),
            CharacterClass.ScienceOfficer => SeedData.GetScienceStartingScenario(),
            CharacterClass.MedicalOfficer => SeedData.GetMedicalStartingScenario(),
            CharacterClass.Pilot => SeedData.GetPilotStartingScenario(),
            CharacterClass.CorporateRep => SeedData.GetCorporateStartingScenario(),
            CharacterClass.Marine => SeedData.GetMarineStartingScenario(),
            CharacterClass.Hacker => SeedData.GetHackerStartingScenario(),
            CharacterClass.AISpecialist => SeedData.GetAISpecialistStartingScenario(),
            _ => SeedData.GetEngineerStartingScenario()
        };
    }

    public int GetStartingLocationId(CharacterClass characterClass)
    {
        var scenario = GetScenario(characterClass);
        return scenario.StartingLocationId;
    }

    public List<Item> GetStartingItems(CharacterClass characterClass)
    {
        var scenario = GetScenario(characterClass);
        var items = new List<Item>();
        
        foreach (var itemName in scenario.InitialItems)
        {
            var item = _context.Items.FirstOrDefault(i => i.Name == itemName);
            if (item != null)
            {
                items.Add(item);
            }
        }
        
        return items;
    }
}
