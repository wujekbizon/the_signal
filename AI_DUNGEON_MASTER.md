# AI Dungeon Master System - Procedural Locations
## The Signal - Infinite Exploration Architecture

**Status**: Design Phase Complete  
**Priority**: Phase 9 - Post-MVP Implementation  
**Estimated Time**: 8-10 hours

---

## Overview

Transform The Signal from a static 7-location game into an **infinite exploration experience** by having Gemini AI act as a Dungeon Master. AI generates procedural "filler" locations between hardcoded story anchors, creating unique paths every playthrough while maintaining narrative coherence.

**Core Philosophy**: 
- Static locations are movie set pieces. AI filler is the journey between them.
- **The Dread of the Lost**: There is no map. If you lose your way, the station warps. Backtracking is never safe. 
- **Shifting Reality**: Each time you enter a corridor, even one you just left, it has changed. The Signal is rewriting the station's geometry.

---

## Architecture

### Two-Tier Location System

```
┌─────────────────────────────────────────────────────────────┐
│                    LOCATION HIERARCHY                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  STATIC ANCHORS (15 total)                                   │
│  ├── Core Locations (6)                                      │
│  │   ├── Engineering Bay                                     │
│  │   ├── Cryo Bay                                            │
│  │   ├── Medical Bay                                         │
│  │   ├── Bridge                                              │
│  │   ├── Research Lab Alpha                                  │
│  │   └── Sub-Level 3                                         │
│  │                                                           │
│  └── Class-Exclusive (9)                                     │
│      ├── Maintenance Shaft (Engineer)                       │
│      ├── Security Hub (Security Officer)                    │
│      └── ... etc for all 9 classes                           │
│                                                              │
│  AI-GENERATED FILLER (Infinite & Shifting)                    │
│  ├── Corridors (A-12, B-7, Gamma-4)                         │
│  ├── Storage Rooms (Storage Bay 4, Supply Closet C-9)       │
│  ├── Maintenance Areas (Shaft Beta, Junction Box 7)         │
│  └── Damaged Sections (Debris Field, Breached Hull)         │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### The "Loop of Despair" Mechanism

If a player attempts to backtrack or enters a loop:
1. **The Shift**: The AI generates a new description. The "Corridor A-1" you just left is now "Corridor A-1 (Altered)".
2. **Escalation**: Each repeated visit increases the `TwistLevel` (1-5).
   - **Level 1**: Eerie silence, slight lighting change.
   - **Level 3**: Geometric frost on walls, whispers in the character's voice.
   - **Level 5**: Non-Euclidean geometry, total darkness, heavy stat drain.
3. **Lethality**: Getting lost is a death sentence. Oxygen and Sanity drain faster in "Shifted" locations.

---

## AI Dungeon Master Role

### Prompt Template

```
You are the AI Dungeon Master for "The Signal" - a sci-fi horror RPG set on an abandoned deep-space station.

=== SCENARIO RULES (MUST FOLLOW) ===
• Setting: Outpost Theta-7, Kuiper Belt
• Timeline: 47 days since last transmission
• The Signal: Mathematical pattern that transforms human consciousness
• The Changed: Former crew members who embraced transformation
• Atmosphere: Cosmic dread, isolation, body horror, shifting reality

=== CURRENT GAME STATE ===
Character: {Name}, {Class}
Current Story Beat: {Beat}
Target Location: {NextStaticLocation}
Current Twist Level: {1-5} (Higher = more distorted/horrifying)

Character State:
- Health: {X}% | Sanity: {X}/100 | Oxygen: {X}%

=== GENERATION REQUEST ===
Create a space station "filler" location. 

REQUIREMENTS:
1. Name: Short, atmospheric. If TwistLevel > 1, append (Altered).
2. Visual: 2-3 sentences. If TwistLevel is high, describe impossible geometry or body horror.
3. Audio: Sounds that reflect the player's isolation or the Signal's harmonics.
4. Environmental: Temperature, smells (ozone, copper, rot).
5. Discovery/Hazard/Boon:
   - 60% Hazard: AI must describe the event (e.g., "A jagged plate cuts your suit—hissing oxygen escapes into the void"). Stat loss is handled in the background and reflected in the UI Sidebar.
   - 30% Discovery: Subtle clue (Notes, blood trails, echoes of the 47).
   - 10% Boon: Small resource described in text (e.g., "You find a half-spent oxygen canister discarded in the corner").

CONSTRAINTS:
• NO system notifications for stat changes (e.g., no "-10% Oxygen" text).
• Stat changes MUST be felt through the sensory description.
• The UI Sidebar (Health/Oxygen/Sanity) is the only indicator of current status.
• NO map UI. The player must feel lost if they don't move toward {Target}.
• Backtracking MUST result in a more horrifying description than the previous room.
• Sensory details priority: Sound > Touch > Smell > Sight.
```

---

## Technical Implementation

### Data Models

**ExplorationState Entity**
```csharp
public class ExplorationState
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    
    // Tracks the current procedural chain
    public int StepsFromLastAnchor { get; set; }
    public int CurrentTwistLevel { get; set; } // Increases if player backtracks or loops
    
    // The intended destination
    public int TargetStaticLocationId { get; set; }
}
```

**Hazard/Boon Result**
```csharp
public class ProceduralResult
{
    public string Description { get; set; } = string.Empty;
    public int HealthChange { get; set; }
    public int SanityChange { get; set; }
    public int OxygenChange { get; set; }
    public string? FoundItemName { get; set; }
}
```

---

## Caching Strategy (The "Short-Term Memory")

1. **Procedural History**: We store the last 3 procedural locations for a character.
2. **Shift Logic**: If the player moves back to a location in their history, the `CurrentTwistLevel` increments and the AI is asked to "Shift" the description.
3. **Reset**: Upon reaching a **Static Anchor**, the `ExplorationState` resets (Steps = 0, Twist = 0).

---

## Summary

The AI Dungeon Master system transforms The Signal into a **claustrophobic maze** where:

1. **Static anchors** provide story progression and safety.
2. **AI filler** is a lethal gauntlet where getting lost leads to madness and death.
3. **The Station Shifts**: Reality is not consistent. Backtracking is an invitation to horror.
4. **Hazard Rolls**: Every room has a 60% chance to drain resources, forcing players to move with purpose.

**Result**: Every journey between story beats is a unique, high-stakes survival challenge.

---
**Last Updated**: 2026-02-18  
**Status**: Ready for Implementation (Phase 9)
