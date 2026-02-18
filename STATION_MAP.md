# Outpost Theta-7: Station Map & Navigation System

**Status**: Design Phase Complete  
**Priority**: Infrastructure for Phase 9  

---

## 1. STATION ARCHITECTURE OVERVIEW

Outpost Theta-7 is a modular deep-space platform. Navigation is based on **Static Anchors** (hardcoded story locations) and **Procedural Gaps** (AI-generated filler).

### Deck Hierarchy
- **Deck 2: Habitation & Biosystems** (Lockdown/Restricted)
- **Deck 1: Command, Personnel & Science** (Primary Gameplay Area)
- **Deck 0: The Core & Sub-Levels** (The Source of the Signal)
- **Deck 3: Hangar & External** (Depressurized/Restricted)

---

## 2. THE MASTER MAP (Deck 1 & 0)

### Deck 1: Command & Operations (Sector B & A)
```text
[Executive Archive] <---> [Captain's Qtrs] <---> [BRIDGE] <---> [Comm Array]
      (Corporate)              (Sealed)             ^             (Destroyed)
                                                    |
                                [Corridor B] <------+
                                      |
[Security Hub] <---> [ARMORY] <---> [Tactical Ops] <---> [Barracks]
                                      |
                                [Corridor B]
                                      |
[Life Support Hub] <---> [ENGINEERING] <---> [Maint Catwalks]
                                |
                          [Corridor A]
                                |
[CRYO BAY] <-----------> [Main Corridor A] <-----------> [MEDICAL BAY]
                                |                           |
                          [Corridor C] <--- [Quarantine] ---+
                                |
[Xenoarch Lab] <---> [RESEARCH LAB ALPHA] <---> [Sample Storage]
```

### Deck 0: Sub-Levels (Sector S)
```text
      [MOTHER Core] <-------------------> [Server Core]
            ^                                   ^
            | (Data Conduits)                   | (Neural Link)
            v                                   v
[Sub-Level Engineering] <--------------> [SUB-LEVEL 3] <--> [Sub-Level Research]
                                                |
                                         [Vessel Chamber]
                                                |
                                         [The Deep Shaft]
                                                |
                                     [Vault: RESTRICTED]
```

---

## 3. NAVIGATION MECHANICS

### Static Connections (Direct)
Some locations are physically adjacent and require no filler.
- **Example**: `Research Lab Alpha` <-> `Xenoarchaeology Lab`.
- **Logic**: Immediate transition. No stat drain.

### Procedural Gaps (Corridors/Veins)
The gaps between major sectors require 1-5 rooms of AI-generated content.
- **Corridor A (The Industrial Spine)**: Connects Engineering to Cryo/Medical. (Heat, Vibration, Steam).
- **Corridor B (The Security Buffer)**: Connects Armory to Bridge. (Sterile, Monitoring, Silent).
- **Corridor C (The Research Loop)**: Connects Medical to Research Lab. (Chemicals, Bio-lights, Equations).
- **Maintenance Shafts (The Veins)**: Hidden vertical gaps between Deck 1 and Deck 0.

### Navigation Logic
1. **The Target**: Character always has a `TargetAnchorId`.
2. **The Distance**: `StepsFromAnchor` vs `TotalDistance`. 
   - Moving *Toward* target: `StepsFromAnchor` increments until `StaticAnchor` is reached.
   - Moving *Away* (Backtracking): `CurrentTwistLevel` increments. AI generates "Shifted" versions of previous rooms.
3. **The "Lost" State**: If `CurrentTwistLevel` reaches 5, the player is "Consumed by the Signal" (Game Over).

---

## 4. FUTURE EXPANSION (Inaccessible)

### Sector D: Habitation (Deck 2)
- Accessible via: Elevator in Corridor A (Currently: *Biological Obstruction*)
- Content: Canteen, Botanical Garden, Residential Pods.

### Sector E: Hangar (Deck 3)
- Accessible via: Air-lock in Corridor B (Currently: *Depressurized*)
- Content: Flight Deck, Cargo Holds, Shuttle Maintenance.

---

## 5. DATABASE IMPLEMENTATION: `LocationExits` & Gaps

Instead of a static C# class, the map registry is stored in the database. `LocationExits` define the paths, and "Gaps" define the procedural spaces.

### Entity Updates
- **Location**: Add `Deck` (int), `Sector` (string), `IsStatic` (bool).
- **LocationExit**: Add `GapSize` (int) - Number of AI rooms. `GapType` (string) - Flavor for AI.

### Example Logic
1. **Direct Connection**: `GapSize = 0`. Immediate move.
2. **Procedural Path**: `GapSize > 0`. `ExplorationService` handles the filler generation rooms one-by-one.
3. **Hazard Logic**: 60% chance for the AI to describe a hazard (Health/Oxygen loss) integrated into the narrative. No system popups; changes appear on the UI side panel.


---
**Last Updated**: 2026-02-18  
**Document Type**: Architectural Specification
