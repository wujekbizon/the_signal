using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheSignal.Data;
using TheSignal.Models.Entities;
using TheSignal.Repositories.Interfaces;

namespace TheSignal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscoveryController : ControllerBase
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ApplicationDbContext _context;

    public DiscoveryController(ICharacterRepository characterRepo, ApplicationDbContext context)
    {
        _characterRepo = characterRepo;
        _context = context;
    }

    [HttpPost("record")]
    public async Task<IActionResult> RecordDiscovery([FromBody] DiscoveryRequest request)
    {
        var character = await _characterRepo.GetByIdAsync(request.CharacterId);
        if (character == null)
        {
            return NotFound(new { error = "Character not found" });
        }

        // Add to discovered clues if not already present
        if (!character.DiscoveredClues.Contains(request.ClueId))
        {
            character.DiscoveredClues.Add(request.ClueId);
            character.RecentActions.Add($"Discovered: {request.Title}");
            await _characterRepo.UpdateAsync(character);
        }

        return Ok(new
        {
            success = true,
            message = "Discovery recorded",
            totalDiscovered = character.DiscoveredClues.Count
        });
    }

    [HttpGet("list/{characterId}")]
    public async Task<IActionResult> GetDiscoveries(int characterId)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null)
        {
            return NotFound(new { error = "Character not found" });
        }

        return Ok(new
        {
            discoveries = character.DiscoveredClues,
            total = character.DiscoveredClues.Count
        });
    }

    [HttpPost("interactable")]
    public async Task<IActionResult> DiscoverInteractable([FromBody] InteractableDiscoveryRequest request)
    {
        var character = await _characterRepo.GetByIdAsync(request.CharacterId);
        if (character == null)
        {
            return NotFound(new { error = "Character not found" });
        }

        // Check if already discovered
        var existingDiscovery = await _context.DiscoveredInteractables
            .FirstOrDefaultAsync(di => di.CharacterId == request.CharacterId && di.InteractableId == request.InteractableId);

        if (existingDiscovery != null)
        {
            return Ok(new
            {
                success = true,
                alreadyDiscovered = true,
                message = "Already discovered"
            });
        }

        // Find interactable details from location content
        var locationContent = SeedData.GetLocationContents()
            .FirstOrDefault(lc => lc.LocationId == request.LocationId);

        var interactable = locationContent?.Interactables
            .FirstOrDefault(i => i.Id == request.InteractableId);

        if (interactable == null)
        {
            return NotFound(new { error = "Interactable not found" });
        }

        // Record discovery
        var discovery = new DiscoveredInteractable
        {
            CharacterId = request.CharacterId,
            InteractableId = request.InteractableId,
            LocationId = request.LocationId,
            DiscoveredAt = DateTime.UtcNow,
            HasInteracted = false
        };

        _context.DiscoveredInteractables.Add(discovery);
        character.RecentActions.Add($"Discovered: {interactable.Name}");
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            alreadyDiscovered = false,
            message = "Discovery recorded",
            interactable = new
            {
                id = interactable.Id,
                name = interactable.Name,
                description = interactable.Description,
                action = interactable.Action,
                isKeyItem = interactable.IsKeyItem
            }
        });
    }

    [HttpGet("interactables/{characterId}/{locationId}")]
    public async Task<IActionResult> GetDiscoveredInteractables(int characterId, int locationId)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null)
        {
            return NotFound(new { error = "Character not found" });
        }

        var discoveredIds = await _context.DiscoveredInteractables
            .Where(di => di.CharacterId == characterId && di.LocationId == locationId)
            .Select(di => di.InteractableId)
            .ToListAsync();

        return Ok(new
        {
            discoveredIds,
            count = discoveredIds.Count
        });
    }

    [HttpPost("clear-cache")]
    public async Task<IActionResult> ClearNarrativeCache()
    {
        var count = await _context.GeneratedNarratives.CountAsync();
        _context.GeneratedNarratives.RemoveRange(_context.GeneratedNarratives);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = $"Cleared {count} cached narratives"
        });
    }
}

public class DiscoveryRequest
{
    public int CharacterId { get; set; }
    public string ClueId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class InteractableDiscoveryRequest
{
    public int CharacterId { get; set; }
    public string InteractableId { get; set; } = string.Empty;
    public int LocationId { get; set; }
}
