using Microsoft.AspNetCore.Mvc;
using TheSignal.Models.GameData;
using TheSignal.Models.ViewModels;
using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Repositories.Interfaces;
using TheSignal.Services.Interfaces;

namespace TheSignal.Controllers;

public class GameController : Controller
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly INarrativeService _narrativeService;

    public GameController(
        ICharacterRepository characterRepo,
        ILocationRepository locationRepo,
        INarrativeService narrativeService)
    {
        _characterRepo = characterRepo;
        _locationRepo = locationRepo;
        _narrativeService = narrativeService;
    }

    public IActionResult CharacterCreation()
    {
        var viewModel = new CharacterCreationViewModel
        {
            Classes = ClassDefinitions.GetAll().Values.ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCharacter(string name, CharacterClass characterClass)
    {
        var character = new Character
        {
            Name = name,
            Class = characterClass,
            CurrentLocationId = 1 // Starting location
        };

        await _characterRepo.AddAsync(character);
        return RedirectToAction(nameof(Index), new { characterId = character.Id });
    }

    public async Task<IActionResult> Index(int characterId)
    {
        var character = await _characterRepo.GetByIdWithDetailsAsync(characterId);
        if (character == null) return NotFound();

        var location = await _locationRepo.GetByIdWithExitsAsync(character.CurrentLocationId);
        if (location == null) return NotFound();

        var narrative = await _narrativeService.GenerateNarrativeAsync(
            character.Id, location.Id, TimeOfDay.StandardCycle, 
            location.DefaultAtmosphere, location.ThreatLevel);

        var viewModel = BuildGameSessionViewModel(character, location, narrative);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Move(int characterId, int targetLocationId)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null) return NotFound();

        var currentLocation = await _locationRepo.GetByIdWithExitsAsync(character.CurrentLocationId);
        if (currentLocation == null) return NotFound();

        var exit = currentLocation.Exits.FirstOrDefault(e => e.TargetLocationId == targetLocationId);
        if (exit == null) return BadRequest("Exit not found");

        await _characterRepo.UpdateLocationAsync(characterId, targetLocationId);
        return RedirectToAction(nameof(Index), new { characterId });
    }

    public IActionResult Archive()
    {
        return View();
    }

    private GameSessionViewModel BuildGameSessionViewModel(
        Character character, Location location, string narrative)
    {
        return new GameSessionViewModel
        {
            CharacterId = character.Id,
            CharacterName = character.Name,
            CharacterClass = character.Class,
            Health = character.Health,
            MaxHealth = character.MaxHealth,
            Oxygen = character.Oxygen,
            MaxOxygen = character.MaxOxygen,
            Sanity = character.Sanity,
            MaxSanity = character.MaxSanity,
            LocationId = location.Id,
            LocationName = location.Name,
            LocationDescription = location.BaseDescription,
            Biome = location.Biome,
            Atmosphere = location.DefaultAtmosphere,
            ThreatLevel = location.ThreatLevel,
            NarrativeText = narrative,
            AvailableExits = location.Exits.Select(e => new ExitViewModel
            {
                TargetLocationId = e.TargetLocationId,
                Direction = e.Direction,
                Description = e.Description,
                IsAvailable = !e.Conditions.Any()
            }).ToList(),
            Inventory = character.Inventory.Select(i => new InventoryItemViewModel
            {
                ItemId = i.ItemId,
                Name = i.Item?.Name ?? "Unknown",
                Description = i.Item?.Description ?? "",
                Type = i.Item?.Type ?? ItemType.Tool,
                Quantity = i.Quantity,
                UsesRemaining = i.Item?.UsesRemaining
            }).ToList(),
            ActiveEffects = character.ActiveEffects.Select(e => new StatusEffectViewModel
            {
                Name = e.Name,
                Type = e.Type,
                DurationMinutes = e.DurationMinutes,
                AppliedAt = e.AppliedAt,
                Description = e.Description
            }).ToList(),
            RecentActions = character.RecentActions.TakeLast(5).ToList()
        };
    }
}
