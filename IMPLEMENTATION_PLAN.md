# The Signal - Implementation Plan
## Phase-by-Phase Development Roadmap

**Status**: Ready for Implementation  
**Estimated Time**: 14-16 hours  
**Priority**: Engineer Class MVP First

---

## Overview

Transform the 38,000-word scenario document into functional game infrastructure. Each phase builds on previous work, culminating in a playable vertical slice.

---

## Phase 1: Content Structure & Data Models (1-2 hours)

**Goal**: Create infrastructure for scenario text injection

### Tasks:
1. **Create `Models/GameData/ScenarioContent.cs`**
   - Static text containers for narrative content
   - Properties: Title, VisualDescription, AudioDescription, Atmosphere, Psychological

2. **Create `Models/GameData/StartingScenario.cs`**
   - Class-specific starting data structure
   - Properties: CharacterClass, StartingLocationId, OpeningNarrative, InitialItems[], FirstObjective

3. **Create `Models/GameData/LocationContent.cs`**
   - Rich location description container
   - Properties: LocationId, Visual, Audio, Atmosphere, Psychological, Interactables[]

4. **Create `Models/GameData/AudioLog.cs`**
   - Log data structure
   - Properties: LogId, Day, Speaker, Transcript, LocationId, IsDiscovered

5. **Extend `Location` Entity**
   - Add `ContentKey` field (string) - links to scenario text
   - Add `IsClassExclusive` field (bool)
   - Add `RequiredClass` field (CharacterClass? nullable)

---

## Phase 2: Database Seeding System (2-3 hours)

**Goal**: Populate database with all scenario content

### Tasks:
1. **Create `Data/SeedData.cs`**
   - All static content as code constants
   - 15 Location definitions (6 core + 9 class-exclusive)
   - Location connection mappings
   - 10 Item definitions
   - 4 Key NPC records
   - 10 Audio Log transcripts (copy from scenario Section 6)

2. **Create `Data/ScenarioSeeder.cs`**
   - `SeedLocations()` - Insert all locations
   - `SeedLocationExits()` - Create navigation graph
   - `SeedItems()` - Starting equipment
   - `SeedAudioLogs()` - All 10 logs with transcripts
   - `SeedQuests()` - Main storyline quests

3. **Database Migration**
   - `dotnet ef migrations add SeedScenarioContent`
   - `dotnet ef database update`
   - Verify: 15 locations, ~30 exits, 10 items, 10 logs

### Content Mapping:
| Content | Count | Source Section |
|---------|-------|----------------|
| Core Locations | 6 | 1.2 (Cryo Bay, Medical, Engineering, Bridge, Research Lab, Sub-Level 3) |
| Class-Exclusive | 9 | 1.3 (one per class) |
| Audio Logs | 10 | Section 6 (Logs 01-10) |
| Items | 10+ | Starting scenarios + interactables |
| NPCs | 4 | 5.1 (Jennings, Voss, Morrison, MOTHER) |

---

## Phase 3: Class-Specific Starting System (2 hours)

**Goal**: Implement 9 unique starting scenarios

### Tasks:
1. **Extend `Character` Entity**
   - Add `StartingLocationId` (int, foreign key)
   - Add `CurrentStoryBeat` (StoryBeat enum)

2. **Create `Services/StartingScenarioService.cs`**
   - `GetStartingLocation(CharacterClass class)` - Returns LocationId
   - `GetOpeningNarrative(CharacterClass class)` - Returns opening text
   - `GetStartingItems(CharacterClass class)` - Returns item list

3. **Modify `GameController.CreateCharacter()`**
   - After character creation, set StartingLocationId based on class
   - Add starting items to inventory
   - Inject class-specific opening narrative into session
   - Set CurrentStoryBeat = StoryBeat.WakeUp

4. **Create 9 Starting Scenario Data Objects**
   Copy from scenario Section 2.x:
   - **Engineer**: Engineering Bay - Cryo malfunction, Jennings' note
   - **Security**: Armory - Emergency wake, one round missing
   - **Science**: Research Lab - Time anomaly, containment vessel
   - **Medical**: Medical Bay - Quarantine lockdown, empty beds
   - **Pilot**: Bridge - Moving station, impossible trajectory
   - **Corporate**: Executive Quarters - Secured archive, evidence
   - **Marine**: Barracks - Last stand aftermath, squad gone
   - **Hacker**: Server Room - Neural interface, 47 days online
   - **AI Specialist**: AI Core - MOTHER speaking, consciousness

---

## Phase 4: Location Content Delivery (2 hours)

**Goal**: Display rich location descriptions

### Tasks:
1. **Extend `GameSessionViewModel`**
   ```csharp
   public string LocationVisual { get; set; }
   public string LocationAudio { get; set; }
   public string LocationAtmosphere { get; set; }
   public string LocationPsychological { get; set; }
   public List<InteractableViewModel> Interactables { get; set; }
   public List<ExitViewModel> AvailableExits { get; set; }
   ```

2. **Create `Services/LocationContentService.cs`**
   - `GetLocationContent(int locationId)` - Returns LocationContent
   - `GetInteractables(int locationId, CharacterClass characterClass)` - Filters by class
   - `GetAvailableExits(int locationId, Character character)` - Checks requirements

3. **Update `GameController.Index(int characterId)`**
   - Load character with current location
   - Populate LocationContent from service
   - Filter exits based on class/stats requirements
   - Build GameSessionViewModel with full content

4. **Create `Views/Shared/_LocationDescription.cshtml`**
   - Visual description (rich text, possibly with atmospheric effects)
   - Audio cues (text-based sound descriptions)
   - Atmosphere paragraph
   - Psychological impact text
   - Interactables list with action buttons

---

## Phase 5: Audio Log System (1.5 hours)

**Goal**: Implement collectible audio logs

### Tasks:
1. **Create `Models/Entities/DiscoveredLog.cs`**
   ```csharp
   public class DiscoveredLog
   {
       public int Id { get; set; }
       public int CharacterId { get; set; }
       public int AudioLogId { get; set; }
       public DateTime DiscoveredAt { get; set; }
   }
   ```

2. **Extend `Character` Entity**
   - Add `public List<DiscoveredLog> DiscoveredLogs { get; set; }`

3. **Create `Controllers/AudioLogController.cs`**
   - `Index(int characterId)` - List all discovered logs
   - `Play(int characterId, int logId)` - Show log transcript
   - `Discover(int characterId, int logId)` - Mark as found

4. **Create `Views/AudioLog/Index.cshtml`**
   - List view with Day/Speaker/Preview
   - Detail view with full transcript
   - Atmospheric styling (terminal/CRT aesthetic)

5. **Seed Logs in Locations**
   - Assign each of 10 logs to specific locations
   - Engineer: Logs 4, 9A-F (technical focus)
   - Security: Logs 5, 8 (security focus)
   - Bridge: Log 3, 6, 9 (command focus)
   - etc.

---

## Phase 6: Story Beat Tracking (2 hours)

**Goal**: Track player progress through 10 story beats

### Tasks:
1. **Create `Models/Enums/StoryBeat.cs`**
   ```csharp
   public enum StoryBeat
   {
       WakeUp = 1,           // Phase 1 - Tutorial
       FirstDiscovery,       // Phase 2 - First exploration
       MOTHERContact,        // Phase 3 - AI interaction
       FirstEncounter,       // Phase 4 - Meet the Changed
       MidpointRevelation,   // Phase 5 - Containment vessel
       TheChoice,            // Phase 6 - Accept/Resist/Escape
       Consequences,         // Phase 7 - Experience choice results
       CrisisPoint,          // Phase 8 - Point of no return
       Climax,               // Phase 9 - Final confrontation
       Resolution            // Phase 10 - Ending
   }
   ```

2. **Create `Services/StoryProgressionService.cs`**
   - `AdvanceBeat(int characterId, StoryBeat newBeat)` - Updates progress
   - `CheckBeatTriggers(Character character)` - Evaluates trigger conditions
   - `GetBeatNarrative(StoryBeat beat, CharacterClass class)` - Returns beat-specific text

3. **Implement Beat Triggers**
   - **Beat 1**: Auto-trigger on character creation
   - **Beat 2**: Trigger on first location movement
   - **Beat 3**: Trigger on visiting any terminal (Engineering/Bridge)
   - **Beat 4**: Trigger on entering Main Corridor Hub
   - **Beat 5**: Trigger on entering Research Lab Alpha
   - **Beat 6**: Trigger on reaching Bridge with full context
   - **Beat 7-10**: Choice-dependent triggers

4. **Modify Narrative Prompts**
   - Update `NarrativeService.BuildPrompt()` to include CurrentStoryBeat
   - Beat context helps Gemini generate appropriate narrative progression

---

## Phase 7: Quest/Objective System (2 hours)

**Goal**: Implement main storyline quests

### Tasks:
1. **Create 5 Main Quests** (from scenario Section 3)
   - **Quest 1**: "Awakening" - Wake up, understand situation
   - **Quest 2**: "The Investigation" - Find what happened to crew
   - **Quest 3**: "First Contact" - Encounter the Changed
   - **Quest 4**: "The Truth" - Learn about the Signal
   - **Quest 5**: "The Choice" - Accept, resist, or escape

2. **Extend `Quest` Entity**
   - Add `StoryBeat` property (links quest to narrative phase)
   - Add `IsMainQuest` property (bool)

3. **Create `Controllers/QuestController.cs`**
   - `Index(int characterId)` - Show active/completed quests
   - `UpdateProgress(int questId, int objectiveId)` - Mark objectives complete

4. **Auto-Assign Starting Quest**
   - On character creation, assign Quest 1
   - Update quest progress as beats advance
   - Auto-complete objectives based on story triggers

5. **Quest Display in Game Session**
   - Add active quest to GameSessionViewModel
   - Show current objective in sidebar
   - Update dynamically as player progresses

---

## Phase 8: Class-Exclusive Content (2 hours)

**Goal**: Lock/unlock content based on class

### Tasks:
1. **Extend `LocationExit` Entity**
   - Add `RequiredClass` (CharacterClass? nullable)
   - Add `RequiredTechnical` (int, default 0)
   - Add `RequiredScience` (int, default 0)
   - Add `RequiredCombat` (int, default 0)

2. **Modify `GameController.Move()`**
   - Check if exit has class requirements
   - If yes, verify character class matches
   - If stat requirements, check character stats
   - Return error message if requirements not met
   - Suggest alternative routes

3. **Create Class-Specific Content Loaders**
   - **Engineer**: Access to Maintenance Shafts, Jennings' logs
   - **Security**: Access to Surveillance Hub, full footage
   - **Hacker**: Access to Server Core, corporate files
   - **Science**: Access to Xenoarchaeology Lab, vessel analysis
   - **Medical**: Access to Quarantine Ward, patient records
   - **Pilot**: Access to Navigation Bay, trajectory data
   - **Corporate**: Access to Executive Archive, conspiracy files
   - **Marine**: Access to Tactical Center, squad status
   - **AI Specialist**: Access to MOTHER Core, AI consciousness

4. **Update Views**
   - Show locked exits with requirement hint
   - Display "Requires: Technical 60+" or "Security Clearance"
   - Hide class-exclusive interactables for wrong classes

---

## MVP Strategy: Engineer Class First

**Recommended Approach**: Implement Engineer class completely before others

**Why Engineer First**:
1. **Technical Focus** - Aligns with your background
2. **Central Location** - Engineering Bay connects to many areas
3. **Key NPC** - Jennings provides emotional anchor
4. **Complete Arc** - Full starting scenario to ending path
5. **Copy Template** - Other 8 classes follow same pattern

**Engineer MVP Scope**:
- [ ] Cryo Bay → Engineering Bay starting path
- [ ] 3 locations: Cryo Bay, Engineering Bay, Maintenance Shaft (exclusive)
- [ ] 3 audio logs: Jennings' logs, technical focus
- [ ] 1 NPC: Dr. Marcus Jennings (transformed)
- [ ] Full Engineer starting scenario from Section 2.1
- [ ] Working movement, narrative generation, beat tracking

**After Engineer MVP**:
Copy-paste pattern for remaining 8 classes (each ~30 minutes once infrastructure exists)

---

## Content Extraction Guide

### Copy-Paste Sources from Scenario Document:

**Locations** (Section 1.2 & 1.3):
- Cryo Bay: Lines 222-246
- Medical Bay: Lines 248-273
- Engineering Bay: Lines 275-301
- Bridge: Lines 303-329
- Research Lab Alpha: Lines 331-357
- Sub-Level 3: Lines 359-384
- [9 class-exclusive locations follow]

**Audio Logs** (Section 6):
- Log 01 (Captain Reeves): Lines 1223-1236
- Log 02 (Dr. Okafor): Lines 1239-1258
- [Logs 03-10 follow]

**Starting Scenarios** (Section 2.x):
- Engineer: Lines 691-724
- Security: Lines 727-757
- [7 more classes follow]

**NPCs** (Section 5.1):
- Dr. Marcus Jennings: Lines 1134-1150
- Dr. Sena Voss: Lines 1153-1170
- Corporal James Morrison: Lines 1173-1190
- MOTHER AI: Lines 1193-1218

---

## Success Criteria

**Phase Complete When**:
- [ ] Database contains all seeded content
- [ ] Engineer can start game and see opening narrative
- [ ] Player can move between connected locations
- [ ] Location descriptions display full scenario text
- [ ] Audio logs can be discovered and played
- [ ] Story beat advances trigger appropriately
- [ ] Active quest updates as player progresses
- [ ] Class-exclusive content blocks/allows access correctly

**MVP Complete When**:
- [ ] Engineer class has full playable loop
- [ ] From Wake Up to First Choice (Beat 6)
- [ ] AI narrative generation provides dynamic content
- [ ] All scenario text displays correctly

---

## Implementation Order

**Priority 1** (MVP - Engineer Only):
1. Phase 1: Data models
2. Phase 2: Seed 6 core locations + 1 exclusive
3. Phase 3: Engineer starting scenario only
4. Phase 4: Location display
5. Phase 5: 3 Engineer-relevant audio logs

**Priority 2** (All Classes):
6. Phase 6: Story beat tracking
7. Phase 7: Quest system
8. Phase 8: All 9 class-exclusive locations
9. Seed all 9 starting scenarios
10. Seed all 10 audio logs

---

## Notes

- **Images**: Not required for MVP. Text descriptions provide sufficient atmosphere.
- **Audio**: No audio files needed. Text-based sound descriptions work.
- **AI Generation**: Gemini provides dynamic narrative between static story beats.
- **Testing**: After each phase, verify with `dotnet run` and manual testing.

---

## Phase 9: AI Dungeon Master - Procedural Locations (8-10 hours)

**Goal**: Transform static travel into a lethal, shifting gauntlet.

### Tasks:
1. **Phase 9A: Foundation (Database)**
   - Update `Location` entity (Deck, Sector, IsStatic).
   - Update `LocationExit` entity (GapSize, GapType).
   - Create `ExplorationState` entity (CharacterId, StepsFromAnchor, TwistLevel).
   - Create `GeneratedLocation` entity (Text storage, HazardRoll results).
   - `dotnet ef migrations add ExpandMapSystem`

2. **Phase 9B: Core Service (The Brain)**
   - Implement `ExplorationService`.
   - Implement `DungeonMasterService` (Gemini API Integration).
   - Build Prompt Template with:
     - Class Lens (Engineer focus on systems).
     - Sensory description requirements (Sound/Touch first).
     - Hazard integration (No system notifications, pure narrative).

3. **Phase 9C: Integration & UI**
   - Update `GameController.Move()` to use `ExplorationService`.
   - Update `Game/Index.cshtml` side panel to show player stats (Health/Oxygen/Sanity).
   - Implement "Dread of the Lost" backtracking logic (TwistLevel escalation).

4. **Phase 9D: Verification (Engineer Only)**
   - Test path from Engineering Bay to Cryo Bay (GapSize = 3).
   - Verify stat loss occurs through narrative descriptions.
   - Verify backtracking leads to "Altered" horrifying versions of rooms.

---

## Success Criteria Phase 9
- [ ] No visual map; player navigates by text and relative direction.
- [ ] Backtracking increments TwistLevel and regenerates "shifted" content.
- [ ] Hazards are integrated into the AI text, not shown as system popups.
- [ ] UI side panel accurately reflects background stat changes.
