using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TheSignal.Data;
using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Models.GameData;
using TheSignal.Repositories.Interfaces;
using TheSignal.Services.Interfaces;

namespace TheSignal.Services.Implementations;

public class ExplorationService : IExplorationService
{
    private readonly ApplicationDbContext _context;
    private readonly IGeminiClient _geminiClient;
    private readonly ICharacterRepository _characterRepo;
    private readonly ILocationRepository _locationRepo;
    
    private const int MAX_HISTORY_SIZE = 3;
    private const int MAX_TWIST_LEVEL = 5;
    private const float HAZARD_CHANCE = 0.60f;
    private const float DISCOVERY_CHANCE = 0.30f;

    public ExplorationService(
        ApplicationDbContext context,
        IGeminiClient geminiClient,
        ICharacterRepository characterRepo,
        ILocationRepository locationRepo)
    {
        _context = context;
        _geminiClient = geminiClient;
        _characterRepo = characterRepo;
        _locationRepo = locationRepo;
    }

    public async Task<ExplorationState?> GetExplorationStateAsync(int characterId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<ExplorationState>()
            .Include(es => es.PreviousRoom)
            .FirstOrDefaultAsync(es => es.CharacterId == characterId, cancellationToken);
    }

    public async Task<bool> IsInProceduralGapAsync(int characterId, CancellationToken cancellationToken = default)
    {
        var state = await GetExplorationStateAsync(characterId, cancellationToken);
        return state?.IsInProceduralGap ?? false;
    }

    public async Task ResetExplorationAsync(int characterId, int targetLocationId, CancellationToken cancellationToken = default)
    {
        var state = await _context.Set<ExplorationState>()
            .FirstOrDefaultAsync(es => es.CharacterId == characterId, cancellationToken);
        
        if (state != null)
        {
            state.StepsFromLastAnchor = 0;
            state.CurrentTwistLevel = 0;
            state.TargetStaticLocationId = targetLocationId;
            state.IsInProceduralGap = false;
            state.RemainingGapSteps = 0;
            state.PreviousRoomId = null;
            state.LastUpdated = DateTime.UtcNow;
        }
        else
        {
            state = new ExplorationState
            {
                CharacterId = characterId,
                StepsFromLastAnchor = 0,
                CurrentTwistLevel = 0,
                TargetStaticLocationId = targetLocationId,
                IsInProceduralGap = false,
                RemainingGapSteps = 0,
                LastUpdated = DateTime.UtcNow
            };
            _context.Set<ExplorationState>().Add(state);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(ProceduralRoom Room, bool IsNew)> EnterProceduralGapAsync(
        int characterId,
        int sourceLocationId,
        int targetLocationId,
        int gapSize,
        string gapType,
        CancellationToken cancellationToken = default)
    {
        var state = await GetExplorationStateAsync(characterId, cancellationToken);
        var character = await _characterRepo.GetByIdAsync(characterId)
            ?? throw new InvalidOperationException($"Character {characterId} not found");
        
        if (state == null)
        {
            state = new ExplorationState
            {
                CharacterId = characterId,
                TargetStaticLocationId = targetLocationId,
                StepsFromLastAnchor = 0,
                CurrentTwistLevel = 0
            };
            _context.Set<ExplorationState>().Add(state);
        }
        
        state.IsInProceduralGap = true;
        state.LastUpdated = DateTime.UtcNow;
        
        var actualGapSize = gapSize;
        if (gapSize == 0 && gapType != "Direct")
        {
            var random = new Random();
            actualGapSize = random.Next(1, 4);
        }
        state.RemainingGapSteps = actualGapSize;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        var room = await GenerateOrRetrieveRoomAsync(
            characterId, 
            sourceLocationId, 
            targetLocationId, 
            1, 
            gapType, 
            state.CurrentTwistLevel,
            false,
            cancellationToken);
        
        state.PreviousRoomId = room.Id;
        state.StepsFromLastAnchor = 1;
        await _context.SaveChangesAsync(cancellationToken);
        
        return (room, room.VisitCount == 1);
    }

    public async Task<(ProceduralRoom? Room, bool ReachedTarget, ExplorationState State)> MoveInGapAsync(
        int characterId,
        bool moveForward,
        CancellationToken cancellationToken = default)
    {
        var state = await GetExplorationStateAsync(characterId, cancellationToken)
            ?? throw new InvalidOperationException($"No exploration state for character {characterId}");
        
        var character = await _characterRepo.GetByIdAsync(characterId)
            ?? throw new InvalidOperationException($"Character {characterId} not found");
        
        if (moveForward)
        {
            state.RemainingGapSteps--;
            state.StepsFromLastAnchor++;
            
            if (state.RemainingGapSteps <= 0)
            {
                state.IsInProceduralGap = false;
                state.StepsFromLastAnchor = 0;
                state.CurrentTwistLevel = 0;
                state.PreviousRoomId = null;
                await _context.SaveChangesAsync(cancellationToken);
                
                await _characterRepo.UpdateLocationAsync(characterId, state.TargetStaticLocationId);
                
                return (null, true, state);
            }
            
            var room = await GenerateOrRetrieveRoomAsync(
                characterId,
                state.PreviousRoom?.SourceAnchorId ?? character.CurrentLocationId,
                state.TargetStaticLocationId,
                state.StepsFromLastAnchor,
                state.PreviousRoom?.GapType ?? "Corridor",
                state.CurrentTwistLevel,
                false,
                cancellationToken);
            
            state.PreviousRoomId = room.Id;
            await _context.SaveChangesAsync(cancellationToken);
            
            await ApplyRoomEffectsAsync(characterId, room, cancellationToken);
            
            return (room, false, state);
        }
        else
        {
            state.CurrentTwistLevel = Math.Min(state.CurrentTwistLevel + 1, MAX_TWIST_LEVEL);
            state.StepsFromLastAnchor = Math.Max(0, state.StepsFromLastAnchor - 1);
            
            if (state.CurrentTwistLevel >= MAX_TWIST_LEVEL)
            {
                await ConsumePlayerAsync(characterId, cancellationToken);
                return (null, false, state);
            }
            
            var room = await GenerateOrRetrieveRoomAsync(
                characterId,
                state.TargetStaticLocationId,
                state.PreviousRoom?.SourceAnchorId ?? character.CurrentLocationId,
                state.StepsFromLastAnchor,
                state.PreviousRoom?.GapType ?? "Corridor",
                state.CurrentTwistLevel,
                true,
                cancellationToken);
            
            state.PreviousRoomId = room.Id;
            state.RemainingGapSteps++;
            await _context.SaveChangesAsync(cancellationToken);
            
            await ApplyRoomEffectsAsync(characterId, room, cancellationToken);
            
            return (room, false, state);
        }
    }

    public async Task ConsumePlayerAsync(int characterId, CancellationToken cancellationToken = default)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character != null)
        {
            character.Health = 0;
            character.Sanity = 0;
            character.Oxygen = 0;
            await _characterRepo.UpdateAsync(character);
        }
    }

    private async Task<ProceduralRoom> GenerateOrRetrieveRoomAsync(
        int characterId,
        int sourceLocationId,
        int targetLocationId,
        int sequenceNumber,
        string gapType,
        int twistLevel,
        bool isBacktrack,
        CancellationToken cancellationToken)
    {
        var contextHash = GenerateContextHash(characterId, sourceLocationId, targetLocationId, sequenceNumber, twistLevel, isBacktrack);
        
        var existingRoom = await _context.Set<ProceduralRoom>()
            .FirstOrDefaultAsync(r => 
                r.CharacterId == characterId && 
                r.ContextHash == contextHash && 
                r.TwistLevel == twistLevel,
                cancellationToken);
        
        if (existingRoom != null && !isBacktrack)
        {
            existingRoom.VisitCount++;
            existingRoom.LastVisitedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return existingRoom;
        }
        
        var character = await _characterRepo.GetByIdWithDetailsAsync(characterId)
            ?? throw new InvalidOperationException($"Character {characterId} not found");
        
        var sourceLocation = await _locationRepo.GetByIdAsync(sourceLocationId)
            ?? throw new InvalidOperationException($"Location {sourceLocationId} not found");
        
        var targetLocation = await _locationRepo.GetByIdAsync(targetLocationId)
            ?? throw new InvalidOperationException($"Location {targetLocationId} not found");
        
        var result = await GenerateRoomWithAIAsync(
            character, 
            sourceLocation, 
            targetLocation, 
            sequenceNumber, 
            gapType, 
            twistLevel,
            cancellationToken);
        
        var room = new ProceduralRoom
        {
            CharacterId = characterId,
            Name = result.Name,
            Description = result.Description,
            AudioDescription = result.AudioDescription,
            EnvironmentalDescription = result.EnvironmentalDescription,
            TwistLevel = twistLevel,
            GapSequenceNumber = sequenceNumber,
            GapType = gapType,
            HealthChange = result.HealthChange,
            SanityChange = result.SanityChange,
            OxygenChange = result.OxygenChange,
            FoundItemName = result.FoundItemName,
            DiscoveryClue = result.DiscoveryClue,
            SourceAnchorId = sourceLocationId,
            TargetAnchorId = targetLocationId,
            IsBacktrack = isBacktrack,
            VisitCount = 1,
            ContextHash = contextHash,
            GeneratedAt = DateTime.UtcNow,
            LastVisitedAt = DateTime.UtcNow
        };
        
        _context.Set<ProceduralRoom>().Add(room);
        await _context.SaveChangesAsync(cancellationToken);
        
        await CleanupOldRoomsAsync(characterId, cancellationToken);
        
        return room;
    }

    private async Task<ProceduralResult> GenerateRoomWithAIAsync(
        Character character,
        Location sourceLocation,
        Location targetLocation,
        int sequenceNumber,
        string gapType,
        int twistLevel,
        CancellationToken cancellationToken)
    {
        var prompt = BuildDungeonMasterPrompt(
            character, 
            sourceLocation, 
            targetLocation, 
            sequenceNumber, 
            gapType, 
            twistLevel);
        
        var random = new Random();
        var roll = random.NextDouble();
        string eventType;
        if (roll < HAZARD_CHANCE)
            eventType = "Hazard";
        else if (roll < HAZARD_CHANCE + DISCOVERY_CHANCE)
            eventType = "Discovery";
        else
            eventType = "Boon";
        
        prompt += $"\n\nEVENT_TYPE_OVERRIDE: {eventType}\n";
        
        var response = await _geminiClient.GenerateNarrativeAsync(prompt, 0.8f, cancellationToken);
        
        return ParseAIResponse(response, eventType);
    }

    private string BuildDungeonMasterPrompt(
        Character character,
        Location sourceLocation,
        Location targetLocation,
        int sequenceNumber,
        string gapType,
        int twistLevel)
    {
        var twistDescription = twistLevel switch
        {
            1 => "Slight eerie silence, subtle lighting changes. Reality feels thin.",
            2 => "Frost patterns on walls, distant whispers, temperature drops noticeably.",
            3 => "Geometric frost covers surfaces, whispers in the character's own voice, shadows move independently.",
            4 => "Non-Euclidean geometry, walls breathe, time feels distorted, heavy dread.",
            5 => "Total darkness encroaches, impossible architecture, the Signal is rewriting reality itself.",
            _ => "Normal conditions."
        };
        
        var nameSuffix = twistLevel > 1 ? " (Altered)" : "";
        
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("You are the AI Dungeon Master for 'The Signal' - a sci-fi horror RPG set on an abandoned deep-space station.");
        sb.AppendLine();
        sb.AppendLine("=== SCENARIO RULES (MUST FOLLOW) ===");
        sb.AppendLine("Setting: Outpost Theta-7, Kuiper Belt");
        sb.AppendLine("Timeline: 47 days since last transmission");
        sb.AppendLine("The Signal: Mathematical pattern that transforms human consciousness");
        sb.AppendLine("The Changed: Former crew members who embraced transformation");
        sb.AppendLine("Atmosphere: Cosmic dread, isolation, body horror, shifting reality");
        sb.AppendLine();
        sb.AppendLine("=== CURRENT GAME STATE ===");
        sb.AppendLine($"Character: {character.Name}, {character.Class}");
        sb.AppendLine($"Current Story Beat: {character.CurrentStoryBeat}");
        sb.AppendLine($"Source Location: {sourceLocation.Name}");
        sb.AppendLine($"Target Location: {targetLocation.Name}");
        sb.AppendLine($"Gap Type: {gapType}");
        sb.AppendLine($"Room Sequence: {sequenceNumber}");
        sb.AppendLine($"Current Twist Level: {twistLevel}/5 (Higher = more distorted/horrifying)");
        sb.AppendLine($"Twist Effects: {twistDescription}");
        sb.AppendLine();
        sb.AppendLine("Character State:");
        sb.AppendLine($"- Health: {character.Health}% | Sanity: {character.Sanity}/100 | Oxygen: {character.Oxygen}%");
        sb.AppendLine();
        sb.AppendLine("=== GENERATION REQUEST ===");
        sb.AppendLine($"Create a space station filler location in the {gapType} between {sourceLocation.Name} and {targetLocation.Name}.");
        sb.AppendLine();
        sb.AppendLine("REQUIREMENTS - Return ONLY a JSON object with these exact fields:");
        sb.AppendLine("{");
        sb.AppendLine($"    \"Name\": \"Short, atmospheric name{nameSuffix}\",");
        sb.AppendLine("    \"Description\": \"2-3 sentences. If TwistLevel is high, describe impossible geometry or body horror. Present tense, second-person.\",");
        sb.AppendLine("    \"AudioDescription\": \"Sounds that reflect isolation or the Signal's harmonics.\",");
        sb.AppendLine("    \"EnvironmentalDescription\": \"Temperature, smells (ozone, copper, rot).\",");
        sb.AppendLine("    \"HealthChange\": integer (-15 to 0 for hazards, 0 for discovery, 5-10 for boon),");
        sb.AppendLine("    \"SanityChange\": integer (-20 to 0 for hazards at high twist levels, 0 for boon),");
        sb.AppendLine("    \"OxygenChange\": integer (-10 to 0 for hazards involving suit damage),");
        sb.AppendLine("    \"FoundItemName\": \"Name of item if boon, null otherwise\",");
        sb.AppendLine("    \"DiscoveryClue\": \"Subtle clue text if discovery event, null otherwise\",");
        sb.AppendLine("    \"EventType\": \"Hazard | Discovery | Boon\"");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("CONSTRAINTS:");
        sb.AppendLine("Return ONLY valid JSON. No markdown, no explanation, no code blocks.");
        sb.AppendLine("Stat changes MUST be felt through sensory description in the Description field.");
        sb.AppendLine("NO system notifications for stat changes.");
        sb.AppendLine("Backtracking/altered rooms MUST be more horrifying.");
        sb.AppendLine("Sensory priority: Sound > Touch > Smell > Sight.");
        sb.AppendLine("Description should imply the stat change without stating it directly.");
        
        return sb.ToString();
    }

    private ProceduralResult ParseAIResponse(string response, string expectedEventType)
    {
        try
        {
            var jsonStart = response.IndexOf('{');
            var jsonEnd = response.LastIndexOf('}');
            
            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                var json = response.Substring(jsonStart, jsonEnd - jsonStart + 1);
                var result = JsonSerializer.Deserialize<ProceduralResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                if (result != null)
                {
                    result.EventType = expectedEventType;
                    return result;
                }
            }
        }
        catch { }
        
        return GenerateFallbackResult(expectedEventType);
    }

    private ProceduralResult GenerateFallbackResult(string eventType)
    {
        return eventType switch
        {
            "Hazard" => new ProceduralResult
            {
                Name = "Corridor Junction",
                Description = "The corridor narrows unexpectedly. Jagged metal edges protrude from torn wall panels, remnants of some violent rupture. Your suit's integrity alarm flickers as you squeeze past.",
                AudioDescription = "The metallic groan of stressed hull plates and the hiss of escaping pressure from hairline fractures.",
                EnvironmentalDescription = "Freezing air, sharp ozone sting, metallic dust coating every surface.",
                HealthChange = -10,
                SanityChange = -5,
                OxygenChange = -5,
                EventType = "Hazard"
            },
            "Discovery" => new ProceduralResult
            {
                Name = "Abandoned Nook",
                Description = "You find a small alcove where someone tried to make a shelter. Tattered blankets, empty ration packs, and scratched equations cover the walls. The math makes your head hurt to look at.",
                AudioDescription = "Silence, but for the distant rhythmic pulse that might be machinery or something else entirely.",
                EnvironmentalDescription = "Stagnant air, faint copper smell, bioluminescent mold providing sickly green light.",
                HealthChange = 0,
                SanityChange = -3,
                OxygenChange = 0,
                DiscoveryClue = "Scrawled on the wall: 'The 47 are not dead. They harmonize.'",
                EventType = "Discovery"
            },
            _ => new ProceduralResult
            {
                Name = "Supply Cache",
                Description = "A forgotten maintenance locker swings open at your touch. Inside, emergency supplies remain untouched—whoever left them never returned.",
                AudioDescription = "The squeak of unused hinges and the reassuring click of sealed containers.",
                EnvironmentalDescription = "Preserved air, plastic and lubricant smells, organized cold storage.",
                HealthChange = 0,
                SanityChange = 0,
                OxygenChange = 15,
                FoundItemName = "Emergency Oxygen Canister",
                EventType = "Boon"
            }
        };
    }

    private async Task ApplyRoomEffectsAsync(int characterId, ProceduralRoom room, CancellationToken cancellationToken)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null) return;
        
        character.Health = Math.Max(0, Math.Min(character.MaxHealth, character.Health + room.HealthChange));
        character.Sanity = Math.Max(0, Math.Min(character.MaxSanity, character.Sanity + room.SanityChange));
        character.Oxygen = Math.Max(0, Math.Min(character.MaxOxygen, character.Oxygen + room.OxygenChange));
        
        if (!string.IsNullOrEmpty(room.DiscoveryClue))
        {
            if (!character.DiscoveredClues.Contains(room.DiscoveryClue))
            {
                character.DiscoveredClues.Add(room.DiscoveryClue);
            }
        }
        
        await _characterRepo.UpdateAsync(character);
    }

    private async Task CleanupOldRoomsAsync(int characterId, CancellationToken cancellationToken)
    {
        var oldRooms = await _context.Set<ProceduralRoom>()
            .Where(r => r.CharacterId == characterId)
            .OrderByDescending(r => r.LastVisitedAt)
            .Skip(MAX_HISTORY_SIZE * 2)
            .ToListAsync(cancellationToken);
        
        if (oldRooms.Any())
        {
            _context.Set<ProceduralRoom>().RemoveRange(oldRooms);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private string GenerateContextHash(int characterId, int sourceId, int targetId, int sequence, int twistLevel, bool isBacktrack)
    {
        var input = $"{characterId}:{sourceId}:{targetId}:{sequence}:{twistLevel}:{isBacktrack}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
    }
}
