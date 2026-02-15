# AI-Powered Paragraph RPG - Architecture Plan
## Project: "Chronicles of the Void"

**Theme**: Sci-Fi Horror (Alien-inspired, event-driven narrative)

---

## 1. PROJECT OVERVIEW

**Core Concept**: Paragraph-based horror RPG where static location anchors combine with AI-generated dynamic narrative. Set in abandoned space stations, derelict ships, and dark colonies. Blood-boiling tension, cosmic dread, psychological horror.

**Tech Stack**:
- .NET 10 Web App (ASP.NET Core MVC) - UPDATED from .NET 9
- Google Gemini API for AI narrative generation
- SQLite (file-based, Code-First approach) - CHANGED from SQL Server
- Visual Studio (primary IDE)
- EF Core Migrations for database management

---

## 2. ARCHITECTURE LAYERS

### 2.1 Project Structure (Simplified Single-Project MVC)

**Decision**: Single project with clean folder separation (easier for learning, still follows Clean Architecture principles)

```
ChroniclesOfTheVoid/
├── Controllers/              # HTTP handlers only
│   ├── HomeController.cs    # Default scaffolded
│   └── GameController.cs    # Main game flow
│
├── Data/                     # Database context
│   ├── ApplicationDbContext.cs
│   └── Migrations/          # EF Core migrations
│
├── Models/
│   ├── Entities/            # Domain entities (POCOs)
│   │   ├── Location.cs
│   │   ├── LocationExit.cs
│   │   ├── ExitCondition.cs
│   │   ├── Character.cs
│   │   ├── InventoryItem.cs
│   │   ├── Item.cs
│   │   ├── StatusEffect.cs
│   │   ├── Quest.cs
│   │   ├── QuestObjective.cs
│   │   ├── CharacterQuest.cs
│   │   ├── NarrativeContext.cs
│   │   └── GeneratedNarrative.cs
│   │
│   ├── Enums/               # One enum per file
│   │   ├── BiomeType.cs     # SpaceStation, DerelictShip, Laboratory, etc.
│   │   ├── TimeOfDay.cs     # StandardCycle, EmergencyLighting, LightsOut
│   │   ├── AtmosphereType.cs # Normal, LowOxygen, Contaminated, Vacuum
│   │   ├── MoodType.cs      # Isolated, Claustrophobic, Terrifying, etc.
│   │   ├── CharacterClass.cs # Engineer, SecurityOfficer, ScienceOfficer, etc.
│   │   ├── ThreatLevel.cs   # Clear, Unknown, Disturbance, Contact, ActiveHunt, Critical
│   │   ├── QuestStatus.cs   # NotStarted, Active, Completed, Failed, Abandoned
│   │   ├── HorrorEventType.cs # Discovery, Encounter, Chase, Ambush, Psychological
│   │   ├── EffectType.cs    # PhysicalInjury, Infection, Psychological, Environmental, Beneficial
│   │   └── ItemType.cs      # Weapon, Tool, Medical, Keycard, Recording, etc.
│   │
│   ├── ViewModels/          # Data for Views (TODO)
│   └── ErrorViewModel.cs    # Default scaffolded
│
├── Services/                # Business logic (TODO)
│   ├── Interfaces/
│   └── Implementations/
│
├── Repositories/            # Data access (TODO)
│   ├── Interfaces/
│   └── Implementations/
│
├── Views/                   # Razor views
│   ├── Home/
│   ├── Shared/
│   └── _ViewImports.cshtml
│
├── wwwroot/                 # Static assets
├── Properties/
├── appsettings.json
├── Program.cs
└── ChroniclesOfTheVoid.csproj
```

---

## 3. GAME THEME: SCI-FI HORROR

### 3.1 Setting
- **Space Stations**: Abandoned orbital habitats
- **Derelict Ships**: Drifting vessels with dark secrets
- **Laboratories**: Research facilities gone wrong
- **Medical Bays**: Quarantine zones, evidence of outbreaks
- **Colony Outposts**: Isolated settlements on asteroids/moons

### 3.2 Horror Mechanics
- **Sanity System**: Psychological degradation from witnessing horrors
- **Oxygen Management**: Survival resource, replaces traditional mana
- **Threat Levels**: Dynamic tension system (Clear → Critical)
- **Event-Driven Narrative**: AI generates content based on context changes

### 3.3 Character Classes
- **Engineer**: Fix systems, access panels, bypass security
- **SecurityOfficer**: Combat, weapons proficiency
- **ScienceOfficer**: Analyze threats, use technical equipment
- **MedicalOfficer**: Healing, diagnose infections
- **Pilot**: Navigation, operate ship systems
- **CorporateRep**: Access codes, corporate secrets

---

## 4. ENTITY FRAMEWORK MIGRATIONS

### 4.1 Code-First Workflow

**Step 1: Install EF Core CLI Tools** (one-time)
```bash
dotnet tool install --global dotnet-ef
```

**Step 2: Install SQLite Package**
```bash
# Via NuGet Package Manager
Microsoft.EntityFrameworkCore.Sqlite (version 10.0.2)

# Or edit .csproj:
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.2" />
```

**Step 3: Configure Connection String** (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  }
}
```

**Step 4: Configure DbContext** (Program.cs)
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
```

### 4.2 Migration Commands

**Create Migration** (after entity changes):
```bash
dotnet ef migrations add InitialCreate
```

**Apply Migration** (create/update database):
```bash
dotnet ef database update
```

**Remove Last Migration** (if not applied):
```bash
dotnet ef migrations remove
```

**List All Migrations**:
```bash
dotnet ef migrations list
```

### 4.3 Database Schema Created

**Tables**:
- **AspNet*** tables (Identity framework)
- **Characters**: Player stats, location, sanity, oxygen
- **Locations**: Space station rooms, descriptions, biome data
- **LocationExits**: Navigation between locations
- **ExitConditions**: Conditional exits (keycards, quests)
- **Items**: Weapons, tools, medical supplies, keycards
- **InventoryItems**: Character inventory with quantities
- **StatusEffects**: Active buffs/debuffs on characters
- **Quests**: Mission definitions
- **QuestObjectives**: Quest steps
- **CharacterQuests**: Character-quest associations
- **GeneratedNarratives**: AI-generated content cache

**Indexes** (for performance):
- ContextHash, LocationId on GeneratedNarratives
- CharacterId on CharacterQuests
- CurrentLocationId on Characters
- LocationId on LocationExits

---

## 5. IMPLEMENTATION PHASES (UPDATED)

### ✅ Phase 1: Domain Models (COMPLETED)
**What was done:**
- Created 10 enum files (BiomeType, TimeOfDay, AtmosphereType, MoodType, CharacterClass, ThreatLevel, QuestStatus, HorrorEventType, EffectType, ItemType)
- Created 12 entity files with proper relationships
- Followed "one class per file" rule
- Used inline property initializers for defaults

### ✅ Phase 2: SQLite Setup (COMPLETED)
**What was done:**
- Replaced SQL Server with SQLite (Microsoft.EntityFrameworkCore.Sqlite)
- Updated appsettings.json with SQLite connection string
- Updated Program.cs to use UseSqlite()
- Configured ApplicationDbContext with all entities
- Set up Fluent API relationships in OnModelCreating()
- Created and applied initial migration
- Verified database creation (app.db file generated)

### ✅ Phase 3: Repository Pattern (COMPLETED)
**What was done:**
- Created ILocationRepository interface with CRUD operations
- Created LocationRepository implementation with async methods
- Created ICharacterRepository interface with character-specific operations
- Created CharacterRepository implementation
- Registered both repositories in DI container (Program.cs)
- Repositories abstract database access from controllers

### ⏳ Phase 4: Gemini Client Service (PENDING)
**Todo:**
- Create IGeminiClient interface
- Implement Google Gemini API client
- Handle API authentication
- Implement prompt builders
- Add caching layer

### ⏳ Phase 5: Narrative Service (PENDING)
**Todo:**
- Create INarrativeService interface
- Implement context merging logic
- Cache lookup before API calls
- Store generated narratives in SQLite

### ⏳ Phase 6: Game Controller (PENDING)
**Todo:**
- Create GameController with thin actions
- Implement async Index action
- Use ViewModels (not entities)
- Max 3 dependencies, max 10 lines per action

### ✅ Phase 7: Landing Page UI (COMPLETED)
**What was done:**
- Completely redesigned landing page with dark sci-fi horror theme
- Terminal-style interface with CRT scanline effects
- CSS Grid + Flexbox layout for consistent positioning
- Atmospheric elements: rotating rings, pulsing dots, SOS morse signal, warning lights
- Horror-themed status card with critical alerts
- "The Signal" branding with typing cursor effect
- Removed Bootstrap dependency, custom CSS only
- Hot Reload configured in Visual Studio for rapid development

### ⏳ Phase 7b: Game Interface Views (PENDING)
**Todo:**
- Create game session views
- Use ViewModels for data binding
- No code-behind logic

### ⏳ Phase 8: Dependency Injection (PENDING)
**Todo:**
- Register all services in Program.cs
- Configure lifetimes (Scoped/Singleton)
- Verify DI container builds correctly

---

## 6. DESIGN DECISIONS

### 6.1 Single vs Multi-Project
**Decision**: Single project with organized folders
**Reason**: Easier for learning MVC pattern, still follows Clean Architecture principles

### 6.2 SQL Server vs SQLite
**Decision**: SQLite
**Reason**: 
- File-based (no SSMS required)
- Easier deployment
- Perfect for single-player games
- Code-First migrations work seamlessly

### 6.3 Property Initializers vs Constructors
**Decision**: Inline property initializers
**Reason**:
- Cleaner syntax for simple defaults
- EF Core standard pattern
- No logic in constructors
- Example: `public string Name { get; set; } = string.Empty;`

### 6.4 One Collection for Quests
**Decision**: Character.Quests (single collection)
**Reason**:
- EF cannot handle multiple relationships to same type
- Filter by QuestStatus.Active when needed
- Simpler database schema

### 6.5 UI Framework: Custom CSS vs Bootstrap
**Decision**: Custom CSS, removed Bootstrap
**Reason**:
- Complete control over horror atmosphere
- Terminal/cyberpunk aesthetic requires custom styling
- Smaller bundle size
- Flexbox + Grid for modern layouts
- Easier to create immersive sci-fi horror feel

### 6.6 Frontend Architecture
**Decision**: Razor views with Hot Reload
**Reason**:
- Server-side rendering perfect for text-based RPG
- Hot Reload for rapid UI iteration
- Can migrate to React/HTMX later if needed
- Current MVC pattern is clean and maintainable

### 6.7 Horror UI Elements
**Implemented**:
- CRT scanline effects for retro terminal feel
- Pulsing dots and warning lights for tension
- SOS morse code signal animation
- Red/Orange/Green status indicators (Dead Space inspired)
- Static noise overlay
- Rotating geometric shapes
- Typing cursor and text animations
- "The Signal" branding - mysterious and evocative

---

## 7. FILE NAMING CONVENTIONS

**Classes**: PascalCase, one per file, filename matches class name
```
Character.cs (contains class Character)
BiomeType.cs (contains enum BiomeType)
```

**Folders**: PascalCase, plural for collections
```
Models/Entities/
Models/Enums/
Services/Interfaces/
```

**Namespaces**: Match folder structure
```csharp
namespace ChroniclesOfTheVoid.Models.Entities;
namespace ChroniclesOfTheVoid.Models.Enums;
```

---

## 8. LESSONS LEARNED

### 8.1 EF Core Migrations
- Always delete old migrations when switching database providers (SQL Server → SQLite)
- Use `dotnet ef migrations remove` to undo last migration
- Shadow properties error = relationship configuration issue in OnModelCreating()
- Two collections of same type on one entity causes EF confusion

### 8.2 Code Organization
- One class per file is worth the extra files
- Enums deserve their own files
- Inline initializers are cleaner than constructors for POCOs
- Fluent API (OnModelCreating) preferred over data annotations

### 8.3 Theme Evolution
- Original: Fantasy RPG (Dungeon Master style)
- Current: Sci-Fi Horror (Alien/Dead Space inspired)
- Benefits: Better fit for paragraph-based tension, event-driven horror
- "The Signal" evokes mystery and cosmic dread

### 8.4 UI Development with Hot Reload
- Visual Studio Hot Reload on File Save works perfectly
- Enable "Hot Reload on File Save" in toolbar dropdown
- No need for `dotnet watch` when debugging in VS
- CSS changes apply instantly - perfect for iterative design
- Razor view changes reload browser automatically
- Use F5 (Start Debugging) not Ctrl+F5 for Hot Reload

---

## 9. NEXT STEPS

1. ✅ Phase 1 Complete (Domain Models)
2. ✅ Phase 2 Complete (SQLite Setup)
3. ✅ Phase 3 Complete (Repository Pattern)
4. ✅ Phase 7a Complete (Landing Page UI)
5. ⏳ Phase 4: Gemini API Integration
6. ⏳ Phase 5: Narrative Generation Service
7. ⏳ Phase 6: Game Controller
8. ⏳ Phase 7b: Game Interface Views
9. ⏳ Phase 8: Dependency Injection verification

**Status**: Backend architecture complete. Beautiful horror-themed landing page done. Ready for AI integration.

---

**Last Updated**: 2026-02-15
**Current Phase**: Phase 4 - Gemini Client Service
**Database**: SQLite (app.db) with 12 tables, Code-First approach
**UI**: Dark sci-fi terminal theme with atmospheric effects
