using Microsoft.AspNetCore.Mvc;
using TheSignal.Data;
using TheSignal.Models.GameData;
using TheSignal.Models.ViewModels;
using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Repositories.Interfaces;
using TheSignal.Services.Interfaces;
using TheSignal.Services;

namespace TheSignal.Controllers;

public class GameController : Controller
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly INarrativeService _narrativeService;
    private readonly IStartingScenarioService _scenarioService;
    private readonly IGameSessionService _sessionService;
    private readonly IStoryProgressionService _storyService;
    private readonly IExplorationService _explorationService;

    public GameController(
        ICharacterRepository characterRepo,
        ILocationRepository locationRepo,
        INarrativeService narrativeService,
        IStartingScenarioService scenarioService,
        IGameSessionService sessionService,
        IStoryProgressionService storyService,
        IExplorationService explorationService)
    {
        _characterRepo = characterRepo;
        _locationRepo = locationRepo;
        _narrativeService = narrativeService;
        _scenarioService = scenarioService;
        _sessionService = sessionService;
        _storyService = storyService;
        _explorationService = explorationService;
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
        // Get class-specific starting scenario
        var startingLocationId = _scenarioService.GetStartingLocationId(characterClass);
        var startingItems = _scenarioService.GetStartingItems(characterClass);
        var classInfo = ClassDefinitions.Get(characterClass);
        
        var character = new Character
        {
            Name = name,
            Class = characterClass,
            CurrentLocationId = startingLocationId,
            StartingLocationId = startingLocationId,
            CurrentStoryBeat = StoryBeat.WakeUp,
            Technical = classInfo.TechnicalBonus,
            Combat = classInfo.CombatBonus,
            Medical = classInfo.MedicalBonus,
            Science = classInfo.ScienceBonus,
            Stealth = classInfo.StealthBonus,
            Leadership = classInfo.LeadershipBonus
        };

        // Add starting items to inventory
        foreach (var item in startingItems)
        {
            character.Inventory.Add(new InventoryItem
            {
                ItemId = item.Id,
                Quantity = 1,
                Character = character
            });
        }

        await _characterRepo.AddAsync(character);
        
        return RedirectToAction(nameof(Index), new { characterId = character.Id });
    }

    public async Task<IActionResult> Index(int characterId)
    {
        // Check for story beat advancement
        string? beatAdvancement = TempData["BeatAdvancement"] as string;
        
        GameSessionViewModel viewModel;
        
        // Always get character and location for narrative generation
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null) return NotFound();
        
        var location = await _locationRepo.GetByIdAsync(character.CurrentLocationId);
        if (location == null) return NotFound();
        
        // Get location content and generate synthesized narrative
        var locationContent = Data.SeedData.GetLocationContents()
            .FirstOrDefault(lc => lc.LocationId == location.Id);
        
        string narrative;
        if (locationContent != null)
        {
            // Synthesize structured content into immersive prose
            narrative = await _narrativeService.GenerateSynthesizedNarrativeAsync(
                character.Id,
                location.Id,
                locationContent);
        }
        else
        {
            // Fallback to basic description if no content available
            narrative = location.BaseDescription;
        }
        
        viewModel = await _sessionService.BuildSessionViewModelWithNarrativeAsync(characterId, narrative);
        
        // If beat just advanced, include that narrative
        if (!string.IsNullOrEmpty(beatAdvancement))
        {
            viewModel.OpeningNarrative = beatAdvancement;
        }
        
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

        // Check for class/stats requirements
        if (exit.RequiredClass.HasValue && exit.RequiredClass != character.Class)
        {
            TempData["Error"] = $"This area requires {exit.RequiredClass} clearance.";
            return RedirectToAction(nameof(Index), new { characterId });
        }

        // Check if there's a procedural gap between locations
        if (exit.GapSize > 0 || !string.IsNullOrEmpty(exit.GapType))
        {
            // Enter procedural exploration mode
            var (room, isNew) = await _explorationService.EnterProceduralGapAsync(
                characterId,
                currentLocation.Id,
                targetLocationId,
                exit.GapSize,
                exit.GapType);

            // Store the procedural room in TempData for display
            TempData["ProceduralRoom"] = System.Text.Json.JsonSerializer.Serialize(room);
            TempData["InProceduralGap"] = true;
            
            return RedirectToAction(nameof(Exploration), new { characterId });
        }

        // Direct connection - no gap
        await _characterRepo.UpdateLocationAsync(characterId, targetLocationId);

        // Check for story beat advancement
        await CheckAndAdvanceBeatAsync(characterId, targetLocationId);

        return RedirectToAction(nameof(Index), new { characterId });
    }

    [HttpGet]
    public async Task<IActionResult> Exploration(int characterId)
    {
        var state = await _explorationService.GetExplorationStateAsync(characterId);
        if (state == null || !state.IsInProceduralGap)
        {
            return RedirectToAction(nameof(Index), new { characterId });
        }

        var room = state.PreviousRoom;
        if (room == null)
        {
            return RedirectToAction(nameof(Index), new { characterId });
        }

        var viewModel = await _sessionService.BuildSessionViewModelAsync(characterId);
        viewModel.IsInProceduralGap = true;
        viewModel.ProceduralRoom = room;
        viewModel.RemainingGapSteps = state.RemainingGapSteps;
        viewModel.TwistLevel = state.CurrentTwistLevel;

        return View("Exploration", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> MoveInGap(int characterId, bool moveForward)
    {
        var (room, reachedTarget, state) = await _explorationService.MoveInGapAsync(characterId, moveForward);

        if (state.IsConsumed)
        {
            // Player was consumed by the Signal
            return RedirectToAction(nameof(Consumed), new { characterId });
        }

        if (reachedTarget)
        {
            // Reached the target location - check for story advancement
            await CheckAndAdvanceBeatAsync(characterId, state.TargetStaticLocationId);
            return RedirectToAction(nameof(Index), new { characterId });
        }

        // Still in the gap - show the next room
        return RedirectToAction(nameof(Exploration), new { characterId });
    }

    [HttpGet]
    public IActionResult Consumed(int characterId)
    {
        return View("Consumed");
    }

    private async Task CheckAndAdvanceBeatAsync(int characterId, int targetLocationId)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null) return;

        // Check triggers based on current beat and target location
        var currentBeat = character.CurrentStoryBeat;
        StoryBeat? newBeat = null;

        switch (currentBeat)
        {
            case StoryBeat.WakeUp:
                // First movement - always advance to FirstDiscovery
                newBeat = StoryBeat.FirstDiscovery;
                break;

            case StoryBeat.FirstDiscovery:
                // Entering Engineering (2) or Bridge (4) triggers MOTHER contact
                if (targetLocationId == 2 || targetLocationId == 4)
                {
                    newBeat = StoryBeat.MOTHERContact;
                }
                break;

            case StoryBeat.MOTHERContact:
                // Entering Sub-Level 3 (6) triggers FirstEncounter
                if (targetLocationId == 6)
                {
                    newBeat = StoryBeat.FirstEncounter;
                }
                break;

            case StoryBeat.FirstEncounter:
                // Entering Research Lab (5) triggers MidpointRevelation
                if (targetLocationId == 5)
                {
                    newBeat = StoryBeat.MidpointRevelation;
                }
                break;

            case StoryBeat.MidpointRevelation:
                // Entering Bridge (4) triggers TheChoice
                if (targetLocationId == 4)
                {
                    newBeat = StoryBeat.TheChoice;
                }
                break;
        }

        if (newBeat.HasValue)
        {
            var result = await _storyService.AdvanceBeatAsync(characterId, newBeat.Value);
            if (result.Success)
            {
                // Keep the beat advancement message for the next request
                TempData["BeatAdvancement"] = result.Narrative ?? $"Advanced to {newBeat.Value}";
            }
        }
    }

    public IActionResult Archive()
    {
        return View();
    }
}
