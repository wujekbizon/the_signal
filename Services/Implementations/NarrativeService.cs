using TheSignal.Data;
using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Models.GameData;
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
        var isStartingLocation = context.StartingLocationId == context.LocationId;
        var prompt = BuildPrompt(context, isStartingLocation);
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

    public async Task<string> GenerateSynthesizedNarrativeAsync(
        int characterId,
        int locationId,
        LocationContent locationContent,
        CancellationToken cancellationToken = default)
    {
        var character = await _characterRepo.GetByIdWithDetailsAsync(characterId)
            ?? throw new InvalidOperationException($"Character {characterId} not found");

        var location = await _locationRepo.GetByIdAsync(locationId)
            ?? throw new InvalidOperationException($"Location {locationId} not found");

        var isStartingLocation = character.StartingLocationId == locationId;
        var cacheHash = $"synth_{characterId}_{locationId}_{locationContent.ContentKey}_{character.CurrentStoryBeat}_v2";
        
        var cached = await _context.GeneratedNarratives
            .FirstOrDefaultAsync(n => n.ContextHash == cacheHash && n.LocationId == locationId, cancellationToken);

        if (cached != null)
        {
            cached.LastAccessedAt = DateTime.UtcNow;
            cached.AccessCount++;
            await _context.SaveChangesAsync(cancellationToken);
            return cached.GeneratedText;
        }

        var prompt = BuildSynthesisPrompt(character, location, locationContent, isStartingLocation);
        var generatedText = await _geminiClient.GenerateNarrativeAsync(prompt, 0.75f, cancellationToken);

        var narrative = new GeneratedNarrative
        {
            ContextHash = cacheHash,
            LocationId = locationId,
            GeneratedText = generatedText,
            ContextJson = JsonSerializer.Serialize(new { characterId, locationId, locationContent }),
            CreatedAt = DateTime.UtcNow,
            LastAccessedAt = DateTime.UtcNow,
            AccessCount = 1
        };

        _context.GeneratedNarratives.Add(narrative);
        await _context.SaveChangesAsync(cancellationToken);

        return generatedText;
    }

    private string BuildSynthesisPrompt(Character character, Location location, LocationContent content, bool isStartingLocation)
    {
        var sb = new System.Text.StringBuilder();
        
        sb.AppendLine("You are an immersive sci-fi horror narrative writer. Transform structured location data into flowing, atmospheric prose.");
        sb.AppendLine();
        sb.AppendLine("=== CRITICAL: PERSPECTIVE RULES ===");
        sb.AppendLine("- ALWAYS write in SECOND PERSON ('you', 'your') - NEVER use the character's name");
        sb.AppendLine("- The player IS the character - describe what THEY experience directly");
        sb.AppendLine("- WRONG: 'Bizon feels pain' or 'systems tearing through Bizon\\'s skull'");
        sb.AppendLine("- RIGHT: 'You feel pain' or 'systems tearing through your skull'");
        sb.AppendLine("- NEVER refer to the character by name - only use 'you', 'your', 'yours'");
        sb.AppendLine();
        
        if (isStartingLocation)
        {
            sb.AppendLine("=== EMERGENCY REVIVAL PROTOCOL ===");
            sb.AppendLine("This is the starting location (cryo-pod awakening).");
            sb.AppendLine("Begin with the brutal awakening experience:");
            sb.AppendLine("- You wake to the screech of failing systems");
            sb.AppendLine("- The cryo-pod's emergency revival wasn't gentle—something malfunctioned or was altered");
            sb.AppendLine("- The display shows your pod was supposed to wake you six weeks ago; backup circuits kept you under");
            sb.AppendLine("- Your professional instincts activate before conscious thought");
            sb.AppendLine("- Catalog the problems before processing the fear: primary power rerouted, life support on minimal, cooling systems modified beyond specs");
            sb.AppendLine("- Someone rebuilt parts of this station—someone who understood the systems intimately");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine("=== ARRIVAL PROTOCOL ===");
            sb.AppendLine("This is NOT the starting location. You have traveled here from another section.");
            sb.AppendLine("DO NOT describe waking up from cryo-pod. Describe your entry into this area.");
            sb.AppendLine();
        }

        sb.AppendLine("=== LOCATION DATA ===");
        sb.AppendLine($"Location: {location.Name}");
        sb.AppendLine($"Character Class: {character.Class}");
        sb.AppendLine($"Story Beat: {character.CurrentStoryBeat}");
        sb.AppendLine();
        sb.AppendLine("VISUAL:");
        sb.AppendLine(content.Visual);
        sb.AppendLine();
        sb.AppendLine("AUDIO:");
        sb.AppendLine(content.Audio);
        sb.AppendLine();
        sb.AppendLine("ATMOSPHERE:");
        sb.AppendLine(content.Atmosphere);
        sb.AppendLine();
        sb.AppendLine("PSYCHOLOGICAL:");
        sb.AppendLine(content.Psychological);
        sb.AppendLine();
        sb.AppendLine("=== INTERACTABLE ELEMENTS - CRITICAL INSTRUCTIONS ===");
        sb.AppendLine("CRITICAL: NEVER output a section titled 'WHAT YOU CAN INTERACT WITH' or any bullet list of interactables.");
        sb.AppendLine("CRITICAL: Use ONLY [DISCOVER:id]descriptive text[/DISCOVER] markers within the Location Description paragraphs.");
        sb.AppendLine("NO EXCEPTIONS. NO BULLET LISTS. NO HEADERS.");
        sb.AppendLine();
        sb.AppendLine("Example CORRECT output:");
        sb.AppendLine("\"The air here hums with the reactor's heartbeat. In the vertical shadows, a ");
        sb.AppendLine("[DISCOVER:engineering_terminal]distressed console pulses with amber diagnostics[/DISCOVER]. ");
        sb.AppendLine("Further up, the [DISCOVER:reactor_controls]emergency shutdown lever[/DISCOVER] sits ");
        sb.AppendLine("unattended, while [DISCOVER:crystal_formations]shimmering translucent growths[/DISCOVER] ");
        sb.AppendLine("web across the primary power conduits.\"");
        sb.AppendLine();
        sb.AppendLine("MANDATORY RULES:");
        sb.AppendLine("1. EVERY interactable MUST be wrapped in [DISCOVER:id]...[/DISCOVER] markers.");
        sb.AppendLine("2. Place markers WITHIN flowing prose paragraphs, never as standalone lists.");
        sb.AppendLine("3. Use descriptive text inside markers, not item names.");
        sb.AppendLine("4. If you output 'WHAT YOU CAN INTERACT WITH' or bullet lists, you have FAILED.");
        sb.AppendLine();
        sb.AppendLine("Available Interactables (wrap ALL of these in discovery markers):");
        if (content.Interactables.Any())
        {
            foreach (var interactable in content.Interactables)
            {
                sb.AppendLine($"- {interactable.Id}: {interactable.Description}");
            }
        }
        else
        {
            sb.AppendLine("No specific interactable elements defined.");
        }
        sb.AppendLine();
        sb.AppendLine("=== CHARACTER CONTEXT ===");
        sb.AppendLine($"Health: {character.Health}% | Sanity: {character.Sanity}/100 | Oxygen: {character.Oxygen}%");
        sb.AppendLine($"Class Perspective: {character.Class} - view this location through their professional lens");
        if (character.DiscoveredClues.Any())
        {
            sb.AppendLine($"Known Information: {string.Join("; ", character.DiscoveredClues.TakeLast(3))}");
        }
        sb.AppendLine();
        sb.AppendLine("=== WRITING INSTRUCTIONS ===");
        sb.AppendLine("Structure the output in this order:");
        sb.AppendLine();
        sb.AppendLine("**1. AWAKENING/ARRIVAL** (if starting location - 1 paragraph)");
        sb.AppendLine("- Describe the brutal awakening from cryo-sleep");
        sb.AppendLine("- Include physical sensations, confusion, alarm signals");
        sb.AppendLine("- Reference the delayed revival, the malfunction");
        sb.AppendLine("- Show the character's professional instincts kicking in");
        sb.AppendLine();
        sb.AppendLine("**2. LOCATION DESCRIPTION** (2-3 paragraphs)");
        sb.AppendLine("- Transform raw sensory data into English prose");
        sb.AppendLine("- NEVER use numeric temperature values like '12°C' or '45 degrees' - use purely descriptive sensory language");
        sb.AppendLine("- Example: Instead of 'humidity: 85%', write 'the air feels thick, heavy with moisture that beads on every surface'");
        sb.AppendLine("- Layer visual, audio, and atmospheric elements organically");
        sb.AppendLine("- Include psychological reactions naturally (don't state 'you feel afraid', show it)");
        sb.AppendLine("- Reference class-specific observations (Engineer notices systems, Security notices threats)");
        sb.AppendLine("- Show the modifications to the station systems");
        sb.AppendLine("- Build cosmic horror atmosphere: isolation, unknown, transformation");
        sb.AppendLine();
        sb.AppendLine("**3. YOUR FIRST OBJECTIVE** (1 short paragraph)");
        sb.AppendLine("- State clearly what needs to be done");
        sb.AppendLine("- Add urgency and context");
        sb.AppendLine("- Mention the 72-hour deadline if applicable");
        sb.AppendLine();
        sb.AppendLine("=== TONE GUIDELINES ===");
        sb.AppendLine("- Present tense, immediate");
        sb.AppendLine("- Cosmic horror undertones (unknown, isolation, body horror hints)");
        sb.AppendLine("- Professional competence mixed with growing unease");
        sb.AppendLine("- NO game mechanics, NO system messages, NO raw stats in narrative sections");
        sb.AppendLine();
        sb.AppendLine("OUTPUT: Generate the narrative, 300-400 words total.");
        
        return sb.ToString();
    }

    private string BuildPrompt(NarrativeContext context, bool isStartingLocation)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"""
        You are a sci-fi horror narrative generator for a text-based RPG inspired by Alien and Dead Space.

        === CRITICAL: PERSPECTIVE RULES ===
        - ALWAYS write in SECOND PERSON ('you', 'your') - NEVER use the character's name
        - The player IS the character - describe what THEY experience directly
        - WRONG: 'Bizon feels pain' or 'systems tearing through Bizon's skull'
        - RIGHT: 'You feel pain' or 'systems tearing through your skull'
        - NEVER refer to the character by name - only use 'you', 'your', 'yours'
        """);

        if (isStartingLocation)
        {
            sb.AppendLine($"""
            === EMERGENCY REVIVAL PROTOCOL ===
            This is the starting location (cryo-pod awakening).
            Begin with the brutal awakening experience:
            - You wake to the screech of failing systems
            - The cryo-pod's emergency revival wasn't gentle—something malfunctioned or was altered
            - The display shows your pod was supposed to wake you six weeks ago; backup circuits kept you under
            - Your professional instincts activate before conscious thought
            - Catalog the problems before processing the fear: primary power rerouted, life support on minimal, cooling systems modified beyond specs
            - Someone rebuilt parts of this station—someone who understood the systems intimately
            """);
        }
        else
        {
            sb.AppendLine($"""
            === ARRIVAL PROTOCOL ===
            This is NOT the starting location. You have traveled here from another section.
            DO NOT describe waking up from cryo-pod. Describe your entry into this area.
            """);
        }

        sb.AppendLine($"""
        === LOCATION DESCRIPTION ===
        Location: {context.LocationName} ({context.Biome})
        Base Description: {context.LocationBaseDescription}
        Time: {context.CurrentTime}
        Atmosphere: {context.CurrentAtmosphere}
        Threat Level: {context.CurrentThreat}
        Mood: {context.CurrentMood}
        """);

        return sb.ToString() + $"""
        IMPORTANT - Translate all sensory data to English prose:
        - NEVER use numeric temperature values like '12°C' or '45 degrees' - use purely descriptive sensory language
        - Instead of "humidity: 85%" write "the air feels thick, heavy with moisture that beads on every surface"
        - Instead of "lighting: 20%" write "shadows pool in the corners, barely held back by the dim emergency lighting"
        - Describe sounds, smells, textures through character experience
        - Show environmental conditions through physical reactions (shivering, condensation, difficulty breathing)

        Key Environmental Elements:
        {string.Join("\n", context.LocationKeyElements.Select(e => $"- {e}"))}

        === CHARACTER STATE ===
        Health: {context.HealthPercent}%
        Sanity: {context.Sanity}/{context.MaxSanity}
        Status Effects: {string.Join(", ", context.ActiveStatusEffects)}
        Key Items: {string.Join(", ", context.KeyInventoryItems)}
        Active Quests: {string.Join(", ", context.ActiveQuestTitles)}

        === CONTEXT ===
        Recent Actions: {string.Join("; ", context.RecentActions)}
        World Events: {string.Join("; ", context.WorldEvents)}
        Last Horror Event: {context.LastEvent}
        Discovered Clues: {string.Join("; ", context.DiscoveredClues)}

        === INTERACTABLE ELEMENTS - CRITICAL ===
        NEVER output bullet lists or "WHAT YOU CAN INTERACT WITH" sections.
        ONLY use [DISCOVER:id]descriptive text[/DISCOVER] markers within LOCATION DESCRIPTION paragraphs.

        WRONG (will be rejected):
        "**WHAT YOU CAN INTERACT WITH**
        - Engineering Terminal: A console..."

        RIGHT:
        "...your eyes catch a [DISCOVER:engineering_terminal]console blinking with amber light[/DISCOVER] 
        in the corner, while [DISCOVER:jennings_workstation]a cluttered workstation[/DISCOVER] 
        nearby shows signs of recent use..."

        MANDATORY:
        - ALL interactables MUST use [DISCOVER:id]...[/DISCOVER] format
        - NEVER output bullet lists under any circumstances
        - Integrate markers into flowing prose only

        === CURRENT OBJECTIVE ===
        Based on Active Quests and current situation, state what the character needs to do next.
        Add urgency appropriate to the context (e.g., "Life support fails in 72 hours")

        === WRITING INSTRUCTIONS ===
        Structure the output in this order:
        1. **AWAKENING/ARRIVAL** (1 paragraph describing the waking/arrival experience)
        2. **LOCATION DESCRIPTION** (2-3 paragraphs of atmospheric sensory description)
           - Use present tense, second-person perspective ("You see...", "Your breath fogs...")
           - Build tension through environmental details (translate all stats to sensory experience)
           - Reference character's physical/mental state naturally
           - Match the threat level (Clear = eerie calm, ActiveHunt = immediate danger)
           - Include cosmic horror elements: isolation, the unknown, body horror, the station being rewired
           - Professional competence mixed with growing dread
        3. **YOUR OBJECTIVE** (1 short paragraph stating current goal with urgency)

        TONE:
        - Present tense, immediate
        - Cosmic horror undertones (unknown, isolation, transformation hints)
        - Professional competence mixed with growing dread
        - NO game mechanics language, NO system messages, NO inventory references in narrative sections

        Generate the narrative now:
        """;
    }
}
