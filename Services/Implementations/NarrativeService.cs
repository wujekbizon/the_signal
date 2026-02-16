using TheSignal.Data;
using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Repositories.Interfaces;
using TheSignal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TheSignal.Services.Implementations;

public class NarrativeService : INarrativeService
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly IGeminiClient _geminiClient;
    private readonly ApplicationDbContext _context;

    public NarrativeService(
        ICharacterRepository characterRepo,
        ILocationRepository locationRepo,
        IGeminiClient geminiClient,
        ApplicationDbContext context)
    {
        _characterRepo = characterRepo;
        _locationRepo = locationRepo;
        _geminiClient = geminiClient;
        _context = context;
    }

    public async Task<string> GenerateNarrativeAsync(
        int characterId,
        int locationId,
        TimeOfDay timeOfDay,
        AtmosphereType atmosphere,
        ThreatLevel threatLevel,
        List<string>? worldEvents = null,
        CancellationToken cancellationToken = default)
    {
        // 1. Fetch character and location
        var character = await _characterRepo.GetByIdWithDetailsAsync(characterId)
            ?? throw new InvalidOperationException($"Character {characterId} not found");

        var location = await _locationRepo.GetByIdAsync(locationId)
            ?? throw new InvalidOperationException($"Location {locationId} not found");

        // 2. Create narrative context
        var context = NarrativeContext.Create(
            location,
            character,
            timeOfDay,
            atmosphere,
            threatLevel,
            worldEvents);

        // 3. Check cache
        var cached = await _context.GeneratedNarratives
            .FirstOrDefaultAsync(n => n.ContextHash == context.ContextHash && n.LocationId == locationId, cancellationToken);

        if (cached != null)
        {
            // Update cache stats
            cached.LastAccessedAt = DateTime.UtcNow;
            cached.AccessCount++;
            await _context.SaveChangesAsync(cancellationToken);
            return cached.GeneratedText;
        }

        // 4. Build prompt and generate
        var prompt = BuildPrompt(context);
        var generatedText = await _geminiClient.GenerateNarrativeAsync(prompt, cancellationToken);

        // 5. Store in database
        var narrative = new GeneratedNarrative
        {
            ContextHash = context.ContextHash,
            LocationId = locationId,
            GeneratedText = generatedText,
            ContextJson = JsonSerializer.Serialize(context),
            CreatedAt = DateTime.UtcNow,
            LastAccessedAt = DateTime.UtcNow,
            AccessCount = 1
        };

        _context.GeneratedNarratives.Add(narrative);
        await _context.SaveChangesAsync(cancellationToken);

        return generatedText;
    }

    private string BuildPrompt(NarrativeContext context)
    {
        return $"""
        You are a sci-fi horror narrative generator for a text-based RPG inspired by Alien and Dead Space.

        SETTING:
        Location: {context.LocationName} ({context.Biome})
        Description: {context.LocationBaseDescription}
        Time: {context.CurrentTime}
        Atmosphere: {context.CurrentAtmosphere}
        Threat Level: {context.CurrentThreat}
        Mood: {context.CurrentMood}

        CHARACTER:
        Name: {context.CharacterName}
        Health: {context.HealthPercent}%
        Sanity: {context.Sanity}/{context.MaxSanity}
        Status Effects: {string.Join(", ", context.ActiveStatusEffects)}
        Key Items: {string.Join(", ", context.KeyInventoryItems)}
        Active Quests: {string.Join(", ", context.ActiveQuestTitles)}

        CONTEXT:
        Recent Actions: {string.Join("; ", context.RecentActions)}
        World Events: {string.Join("; ", context.WorldEvents)}
        Last Horror Event: {context.LastEvent}
        Discovered Clues: {string.Join("; ", context.DiscoveredClues)}

        INSTRUCTIONS:
        Generate a 2-3 paragraph atmospheric narrative describing what the character experiences RIGHT NOW in this location.
        - Use present tense, second-person perspective ("You see...", "Your breath fogs...")
        - Build tension through environmental details (sounds, lighting, temperature, smells)
        - Reference character's physical/mental state naturally
        - Match the threat level (Clear = eerie calm, ActiveHunt = immediate danger)
        - Include cosmic horror elements: isolation, the unknown, body horror
        - NO dialogue, NO choices, NO combat mechanics - pure atmospheric description
        - End with a subtle hook or ominous detail

        Generate the narrative now:
        """;
    }
}
