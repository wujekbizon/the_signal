# AI-Powered Paragraph RPG - Architecture Plan
## Project: "The Signal"

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
TheSignal/
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

### ✅ Phase 4: Gemini Client Service (COMPLETED)
**What was done:**
- Created IGeminiClient interface
- Implemented GeminiClient using Google.GenAI SDK
- API authentication via appsettings (reads Gemini:ApiKey)
- Configurable model, temperature, max tokens
- Registered in DI container (Scoped)
- User secrets configured for Development

### ✅ Phase 5: Narrative Service (COMPLETED)
**What was done:**
- Created INarrativeService interface
- Implemented NarrativeService with context merging (NarrativeContext.Create)
- Cache lookup by ContextHash + LocationId before API calls
- Stores generated narratives to GeneratedNarratives table
- BuildPrompt() creates horror-themed prompts
- Registered in DI container (Scoped)

### ✅ Phase 6: Game Controller (COMPLETED)
**What was done:**
- Created GameController with 5 actions (Index, CharacterCreation, CreateCharacter, Move, Archive)
- Uses 3 dependencies: ICharacterRepository, ILocationRepository, INarrativeService
- All actions use ViewModels (CharacterCreationViewModel, GameSessionViewModel)
- Thin action pattern: orchestration only, business logic in services
- Private BuildGameSessionViewModel helper for entity-to-ViewModel mapping
- Async/await throughout for database operations
- Archive action created (ViewModel wiring pending in Phase 7c)

**Actions:**
- `Index(int characterId)` - Main game session view with narrative
- `CharacterCreation()` - Class selection screen
- `CreateCharacter(string name, CharacterClass characterClass)` - POST to create character
- `Move(int characterId, int targetLocationId)` - Navigation between locations
- `Archive()` - Archived logs view (Phase 7c)

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

### 🔄 Phase 7b: Character Creation UI (IN PROGRESS)
**What we're building:**
- Character creation screen with terminal/folder UI style
- Left sidebar: class selection (9 classes from CharacterClass enum)
- Right panel: class details, stats, name input
- Proper MVC pattern implementation

**Approach:**
- Static class definitions (Models/GameData/ClassDefinitions.cs)
- ViewModel for view data (CharacterCreationViewModel)
- GameController passes ViewModel to view
- View renders with minimal JS (UI state only)
- POST saves Character entity with selected class to DB

**Files:**
- Models/ViewModels/CharacterCreationViewModel.cs
- Models/GameData/ClassDefinitions.cs (static data)
- Controllers/GameController.cs
- Views/Game/CharacterCreation.cshtml

### ⏳ Phase 7c: Archive/Progress View (PENDING)
**Todo:**
- "Access Archived Logs" view
- Show previous character runs
- Death logs, discoveries
- Map placeholder

### ⏳ Phase 7d: Game Session Views (PENDING)
**Todo:**
- Main game interface
- Location display
- Narrative rendering
- Action buttons

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

### 6.8 Static vs Dynamic Game Data
**Decision**: Static data in code, dynamic data in database
**Reason**:
- **Static data** (class definitions, item templates, location templates): Hardcoded in static classes
  - Never changes at runtime
  - Faster access (no DB queries)
  - Type-safe, compile-time checked
  - Example: ClassDefinitions.cs with descriptions/stats
- **Dynamic data** (player state, choices, progress): Stored in SQLite
  - Changes during gameplay
  - Player-specific (Character.CharacterClass, inventory, location)
  - Needs persistence
- Standard RPG pattern: DB stores "player chose Engineer", code defines "what Engineer means"

---

## 7. SECRETS MANAGEMENT

### 7.1 Development Environment (User Secrets)

**Setup** (one-time):
```bash
dotnet user-secrets init
dotnet user-secrets set "Gemini:ApiKey" "YOUR_API_KEY_HERE"
```

**Verify**:
```bash
dotnet user-secrets list
```

**How it works:**
- UserSecretsId configured in TheSignal.csproj
- Secrets stored outside project directory (won't commit to Git)
- appsettings.json has empty values (safe for GitHub)
- Development environment auto-loads user secrets

**Current secrets:**
- `Gemini:ApiKey` - Google Gemini API key
- `ConnectionStrings:DatabaseConnection` - Alternative DB connection

### 7.2 Production Deployment

**Options for production:**

1. **Environment Variables** (Recommended for containers/cloud):
   ```bash
   # Linux/Mac
   export Gemini__ApiKey="YOUR_API_KEY"

   # Windows
   set Gemini__ApiKey=YOUR_API_KEY

   # Docker
   docker run -e Gemini__ApiKey="YOUR_API_KEY" ...
   ```

2. **appsettings.Production.json** (server deployment):
   ```json
   {
     "Gemini": {
       "ApiKey": "YOUR_API_KEY"
     }
   }
   ```
   ⚠️ **NEVER commit this file** - add to .gitignore

3. **Azure App Service**:
   - Use Application Settings in Azure Portal
   - Automatically overrides appsettings values
   - Format: `Gemini:ApiKey` or `Gemini__ApiKey`

4. **AWS/GCP**:
   - Use AWS Secrets Manager / GCP Secret Manager
   - Integrate via SDK or environment variables

**Security checklist:**
- ✅ appsettings.json has empty ApiKey
- ✅ appsettings.Production.json in .gitignore (if used)
- ✅ User secrets for local dev
- ✅ Environment variables or cloud secrets for production

---

## 8. FILE NAMING CONVENTIONS

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

## 9. LESSONS LEARNED

### 9.1 EF Core Migrations
- Always delete old migrations when switching database providers (SQL Server → SQLite)
- Use `dotnet ef migrations remove` to undo last migration
- Shadow properties error = relationship configuration issue in OnModelCreating()
- Two collections of same type on one entity causes EF confusion

### 9.2 Code Organization
- One class per file is worth the extra files
- Enums deserve their own files
- Inline initializers are cleaner than constructors for POCOs
- Fluent API (OnModelCreating) preferred over data annotations

### 9.3 Theme Evolution
- Original: Fantasy RPG (Dungeon Master style)
- Current: Sci-Fi Horror (Alien/Dead Space inspired)
- Benefits: Better fit for paragraph-based tension, event-driven horror
- "The Signal" evokes mystery and cosmic dread

### 9.4 UI Development with Hot Reload
- Visual Studio Hot Reload on File Save works perfectly
- Enable "Hot Reload on File Save" in toolbar dropdown
- No need for `dotnet watch` when debugging in VS
- CSS changes apply instantly - perfect for iterative design
- Razor view changes reload browser automatically
- Use F5 (Start Debugging) not Ctrl+F5 for Hot Reload

### 9.5 Secrets Management
- Use `dotnet user-secrets` for development API keys
- Never commit appsettings.Production.json with real keys
- Keep appsettings.json values empty for GitHub safety
- UserSecretsId auto-generated in .csproj
- Production: use environment variables or cloud secret managers

### 9.6 MVC Pattern for Game UI
- Keep business logic out of views (no complex JS data objects in Razor)
- Use ViewModels to pass data from Controller to View
- Static game definitions belong in code, not DB or view
- View JS should only handle UI state (active buttons, animations)
- Follow separation: Controller builds data → ViewModel carries it → View displays it

---

## 10. NEXT STEPS

1. ✅ Phase 1 Complete (Domain Models)
2. ✅ Phase 2 Complete (SQLite Setup)
3. ✅ Phase 3 Complete (Repository Pattern)
4. ✅ Phase 4 Complete (Gemini API Integration)
5. ✅ Phase 5 Complete (Narrative Generation Service)
6. ✅ Phase 6 Complete (Game Controller)
7. ✅ Phase 7a Complete (Landing Page UI)
8. ✅ Phase 7b Complete (Character Creation UI)
9. 🔄 Phase 9: AI Dungeon Master (IN PROGRESS)
10. ⏳ Phase 7c: Archive/Progress View
11. ⏳ Phase 7d: Game Session Views

**Status**: Implementing Phase 9 (AI-generated filler). Building a lethal, shifting station geometry without a visual map. Hazards integrated into text, stats visible in UI side panel.

---

**Last Updated**: 2026-02-18
**Current Phase**: Phase 9 - AI Dungeon Master
**Database**: SQLite (app.db) with expanded map system entities
**UI**: Dark sci-fi terminal theme with Player Stat side panel
**AI**: Gemini Flash Dungeon Master, dynamic Hazards, and TwistLevels
**Architecture**: Proper MVC - Data-driven Map Registry in DB
