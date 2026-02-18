using TheSignal.Models.Entities;
using TheSignal.Models.ViewModels;
using TheSignal.Models.Enums;
using TheSignal.Data;
using TheSignal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TheSignal.Services;

public interface IGameSessionService
{
    Task<GameSessionViewModel> BuildSessionViewModelAsync(int characterId);
    Task<GameSessionViewModel> BuildSessionViewModelWithNarrativeAsync(int characterId, string narrativeText);
}

public class GameSessionService : IGameSessionService
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly INarrativeParserService _narrativeParser;
    private readonly ApplicationDbContext _context;

    public GameSessionService(
        ICharacterRepository characterRepo,
        ILocationRepository locationRepo,
        INarrativeParserService narrativeParser,
        ApplicationDbContext context)
    {
        _characterRepo = characterRepo;
        _locationRepo = locationRepo;
        _narrativeParser = narrativeParser;
        _context = context;
    }

    public async Task<GameSessionViewModel> BuildSessionViewModelAsync(int characterId)
    {
        var character = await _characterRepo.GetByIdWithDetailsAsync(characterId);
        if (character == null) throw new ArgumentException($"Character {characterId} not found");

        var location = await _locationRepo.GetByIdWithExitsAsync(character.CurrentLocationId);
        if (location == null) throw new ArgumentException($"Location {character.CurrentLocationId} not found");

        return await BuildViewModel(character, location, string.Empty);
    }

    public async Task<GameSessionViewModel> BuildSessionViewModelWithNarrativeAsync(int characterId, string narrativeText)
    {
        var character = await _characterRepo.GetByIdWithDetailsAsync(characterId);
        if (character == null) throw new ArgumentException($"Character {characterId} not found");

        var location = await _locationRepo.GetByIdWithExitsAsync(character.CurrentLocationId);
        if (location == null) throw new ArgumentException($"Location {character.CurrentLocationId} not found");

        return await BuildViewModel(character, location, narrativeText);
    }

    private async Task<GameSessionViewModel> BuildViewModel(Character character, Location location, string narrativeText)
    {
        // Get location content if available
        var locationContent = SeedData.GetLocationContents()
            .FirstOrDefault(lc => lc.LocationId == location.Id);

        // Get all interactables for this location
        var allInteractables = locationContent?.Interactables.Select(i => new InteractableViewModel
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            Action = i.Action,
            IsKeyItem = i.IsKeyItem
        }).ToList() ?? new List<InteractableViewModel>();

        // Parse narrative to HTML with discovery markers
        var parsedNarrative = _narrativeParser.ParseNarrative(narrativeText);

        // Get discovered interactable IDs from database
        var discoveredIds = await _context.DiscoveredInteractables
            .Where(di => di.CharacterId == character.Id && di.LocationId == location.Id)
            .Select(di => di.InteractableId)
            .ToListAsync();

        // Filter interactables - show only discovered ones
        var discoveredInteractables = allInteractables
            .Where(i => discoveredIds.Contains(i.Id))
            .ToList();

        return new GameSessionViewModel
        {
            // Character Info
            CharacterId = character.Id,
            CharacterName = character.Name,
            CharacterClass = character.Class,
            Health = character.Health,
            MaxHealth = character.MaxHealth,
            Oxygen = character.Oxygen,
            MaxOxygen = character.MaxOxygen,
            Sanity = character.Sanity,
            MaxSanity = character.MaxSanity,
            Technical = character.Technical,
            Combat = character.Combat,
            Medical = character.Medical,
            Science = character.Science,

            // Location Info
            LocationId = location.Id,
            LocationName = location.Name,
            LocationDescription = location.BaseDescription,
            LocationVisual = locationContent?.Visual ?? location.BaseDescription,
            LocationAudio = locationContent?.Audio ?? string.Empty,
            LocationAtmosphere = locationContent?.Atmosphere ?? string.Empty,
            LocationPsychological = locationContent?.Psychological ?? string.Empty,
            Biome = location.Biome,
            Atmosphere = location.DefaultAtmosphere,
            ThreatLevel = location.ThreatLevel,
            CurrentStoryBeat = character.CurrentStoryBeat,

            // Narrative
            NarrativeText = narrativeText,
            ParsedNarrativeHtml = parsedNarrative.Html,

            // Exits
            AvailableExits = location.Exits.Select(e => new ExitViewModel
            {
                TargetLocationId = e.TargetLocationId,
                Direction = e.Direction,
                Description = e.Description,
                IsAvailable = !e.Conditions.Any()
            }).ToList(),

            // Interactables - all available in location
            Interactables = allInteractables,

            // Discovered interactables - only those the player has found
            DiscoveredInteractables = discoveredInteractables,

            // Inventory
            Inventory = character.Inventory.Select(i => new InventoryItemViewModel
            {
                ItemId = i.ItemId,
                Name = i.Item?.Name ?? "Unknown",
                Description = i.Item?.Description ?? "",
                Type = i.Item?.Type ?? ItemType.Tool,
                Quantity = i.Quantity,
                UsesRemaining = i.Item?.UsesRemaining
            }).ToList(),

            // Status Effects
            ActiveEffects = character.ActiveEffects.Select(e => new StatusEffectViewModel
            {
                Name = e.Name,
                Type = e.Type,
                DurationMinutes = e.DurationMinutes,
                AppliedAt = e.AppliedAt,
                Description = e.Description
            }).ToList(),

            // Recent Actions
            RecentActions = character.RecentActions.TakeLast(5).ToList()
        };
    }
}
