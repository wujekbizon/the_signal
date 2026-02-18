# Interactive Discovery Mechanism Plan

## Problem Statement
Current system lists all interactables upfront in both narrative text and INTERACTABLES panel. This removes exploration/discovery element from gameplay.

**Current Flow:**
- Narrative lists: "You can interact with: * **Engineering Terminal:** A console..."
- INTERACTABLES panel shows all buttons immediately
- No discovery required

**Desired Flow:**
- Narrative describes environment subtly: "In the far right corner, a blinking light comes from a console embedded in the wall..."
- Player clicks discovery link (e.g., "[Examine the console]")
- Discovered item appears as styled button in INTERACTABLES panel
- Player can then click button to interact

## Roles & Responsibilities

### 1. NarrativeService (The Narrator)
- **Responsibility:** Generating atmospheric prose.
- **Action:** Injects `[DISCOVER:id]text[/DISCOVER]` markers based on the location's available interactables.
- **Logic:** Uses the character's class to determine *how* the item is described (e.g., Engineer sees "unstable voltage" while others see "flickering lights").

### 2. DiscoveryController (The State Keeper)
- **Responsibility:** Managing the player's knowledge state.
- **Action:** Records clicks on discovery markers via `/api/discovery/interactable`.
- **Logic:** Updates the `DiscoveredInteractables` table and triggers the "Recent Actions" log update.

### 3. GameSessionService (The Orchestrator)
- **Responsibility:** Building the View Model.
- **Action:** Filters the `LocationContent` so only "already discovered" items are sent as buttons to the UI.
- **Parsing:** Calls the `NarrativeParser` to turn raw AI text into HUD-style clickable links.


### 2. Prompt Engineering Changes

#### New Prompt Instructions
```
=== INTERACTABLE ELEMENTS - DISCOVERY STYLE ===
DO NOT list interactables as bullet points or bold headers.
Transform raw interactable data into professional observations based on CHARACTER CLASS.

Example (Engineer @ Engineering Bay):
"The air here hums with the reactor's heartbeat. In the vertical shadows, a 
[DISCOVER:engineering_terminal]distressed console pulses with amber diagnostics[/DISCOVER]. 
Further up, the [DISCOVER:reactor_controls]emergency shutdown lever[/DISCOVER] sits 
unattended, while [DISCOVER:crystal_formations]shimmering translucent growths[/DISCOVER] 
web across the primary power conduits."

Rules:
1. Hide interactables in environmental description.
2. Use Class Perspective: Engineer sees hardware/currents; Security sees threats/covers.
3. Discovery marker text must be descriptive (e.g., "blinking light") not just the name.
4. DO NOT reveal the name or action of the item until it appears in the panel.
5. 100% Prose. No markdown lists.
```

### 3. Backend Changes

#### B. Narrative Parsing Service
```csharp
public class NarrativeParserService
{
    // Parse [DISCOVER:id]text[/DISCOVER] tags
    // Convert to HUD-style links: <a class="discovery-hud" data-id="id">text</a>
}
```

### 4. Frontend Changes

#### B. CSS Styling for Discovery HUD
```css
.discovery-hud {
    color: #f0883e; /* Class Primary: Engineer Orange */
    border-bottom: 1px solid rgba(240, 136, 62, 0.3);
    cursor: pointer;
    font-weight: 500;
    padding: 0 2px;
    background: rgba(240, 136, 62, 0.05);
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.discovery-hud:hover {
    background: rgba(240, 136, 62, 0.2);
    border-bottom-color: #f0883e;
    text-shadow: 0 0 8px rgba(240, 136, 62, 0.6);
}

.discovery-hud.discovered {
    color: #7d8590;
    text-decoration: none;
    background: transparent;
    border-bottom: 1px solid #30363d;
    cursor: default;
    opacity: 0.8;
}
```

#### D. Update Interactable Button Styling (Tactical HUD)
```css
.interactable-btn {
    background: rgba(13, 17, 23, 0.9);
    border: 1px solid #30363d;
    border-left: 4px solid #f0883e; /* Tactical stripe */
    padding: 10px 15px;
    font-family: 'Share Tech Mono', monospace;
    letter-spacing: 1px;
    transition: all 0.2s ease;
}

.interactable-btn:hover {
    background: #f0883e;
    color: #0d1117;
    transform: scale(1.02);
    box-shadow: 0 0 20px rgba(240, 136, 62, 0.3);
}
```
=== INTERACTABLE ELEMENTS ===
- Engineering Terminal: A console embedded in a wall panel...
```

#### New Prompt Instructions
```
=== INTERACTABLE ELEMENTS - DISCOVERY STYLE ===
DO NOT list interactables as bullet points.
Instead, weave them into the narrative naturally:

Example (GOOD):
"The emergency lighting casts long shadows across the chamber. In the far corner, 
a faint amber glow pulses rhythmically from a console half-buried under fallen conduits. 
Nearby, something crystalline catches the light - irregular formations clustering around 
a workstation covered in dust and old schematics."

Example (BAD):
"You can interact with:
- Engineering Terminal: A console embedded in a wall panel..."

For each interactable, include a discovery marker:
[DISCOVER:engineering_terminal]the blinking console[/DISCOVER]
[DISCOVER:jennings_workstation]the cluttered desk[/DISCOVER]
[DISCOVER:crystal_formations]crystalline growths[/DISCOVER]

Rules:
1. Hide interactables in environmental description
2. Use sensory details (visual, audio, smell)
3. Place discovery markers around interactable mentions
4. Do NOT reveal what the item does until clicked
5. Never use markdown bullet lists for interactables
```

### 3. Backend Changes

#### A. New API Endpoint
```csharp
POST /api/discovery/interactable
{
    "characterId": 1,
    "interactableId": "engineering_terminal",
    "locationId": 2
}
```

**Response:**
```json
{
    "success": true,
    "interactable": {
        "id": "engineering_terminal",
        "name": "Engineering Terminal",
        "description": "A console embedded in a wall panel...",
        "action": "Access Terminal",
        "isKeyItem": false
    }
}
```

#### B. Narrative Parsing Service
```csharp
public class NarrativeParserService
{
    // Parse [DISCOVER:id]text[/DISCOVER] tags
    // Convert to HTML: <span class="discover-link" data-id="id">text</span>
    
    public ParsedNarrative ParseNarrative(string rawNarrative)
    {
        // Extract discovery markers
        // Return clean HTML + list of discovery IDs
    }
}
```

#### C. Update GameSessionViewModel
```csharp
public class GameSessionViewModel
{
    // Existing fields...
    
    // NEW: Track discovered vs all interactables
    public List<InteractableViewModel> DiscoveredInteractables { get; set; } = new();
    public List<InteractableViewModel> AllLocationInteractables { get; set; } = new();
    
    // Parsed narrative with discovery links
    public string ParsedNarrativeHtml { get; set; } = string.Empty;
}
```

#### D. Update GameSessionService
- Query `DiscoveredInteractables` table for current character/location
- Populate `DiscoveredInteractables` list (only discovered ones go to UI)
- Keep `AllLocationInteractables` for validation

### 4. Frontend Changes

#### A. Update Index.cshtml

**Replace narrative rendering:**
```html
<!-- OLD -->
<div class="narrative-content synthesized">
    @Html.Raw(FormatNarrative(Model.NarrativeText))
</div>

<!-- NEW -->
<div class="narrative-content synthesized" id="narrative-content">
    @Html.Raw(Model.ParsedNarrativeHtml)
</div>
```

**Replace interactables section:**
```html
<!-- OLD: Show all -->
<div class="interactables-grid">
    @foreach (var interactable in Model.Interactables)
    
<!-- NEW: Show only discovered -->
<div class="interactables-grid" id="discovered-interactables">
    @foreach (var interactable in Model.DiscoveredInteractables)
```

#### B. CSS Styling for Discovery Links
```css
.discover-link {
    color: #f0883e;
    text-decoration: underline;
    text-decoration-style: dotted;
    cursor: pointer;
    transition: all 0.2s ease;
    border-bottom: 1px dashed rgba(240, 136, 62, 0.5);
}

.discover-link:hover {
    color: #ff9f5e;
    text-decoration: none;
    background: rgba(240, 136, 62, 0.1);
    border-bottom: 1px solid #f0883e;
}

.discover-link.discovered {
    color: #7d8590;
    text-decoration: line-through;
    cursor: default;
    border-bottom: none;
}
```

#### C. JavaScript for Discovery
```javascript
// Add to modal.js or new file: discovery.js

class DiscoveryManager {
    constructor(characterId, locationId) {
        this.characterId = characterId;
        this.locationId = locationId;
        this.discovered = new Set();
    }
    
    init() {
        // Attach click handlers to all .discover-link elements
        document.querySelectorAll('.discover-link').forEach(link => {
            link.addEventListener('click', (e) => this.handleDiscovery(e));
        });
    }
    
    async handleDiscovery(event) {
        const link = event.target;
        const interactableId = link.dataset.id;
        
        if (this.discovered.has(interactableId)) return;
        
        // Call API to discover
        const response = await fetch('/api/discovery/interactable', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                characterId: this.characterId,
                interactableId: interactableId,
                locationId: this.locationId
            })
        });
        
        if (response.ok) {
            const data = await response.json();
            this.addToInteractables(data.interactable);
            this.markDiscovered(link);
            this.showDiscoveryToast(data.interactable.name);
        }
    }
    
    addToInteractables(interactable) {
        const grid = document.getElementById('discovered-interactables');
        const button = document.createElement('button');
        button.className = `interactable-btn ${interactable.isKeyItem ? 'key-item' : ''}`;
        button.onclick = () => interact(interactable.id);
        button.innerHTML = `
            <span class="interactable-name">${interactable.name}</span>
            <span class="interactable-action">${interactable.action}</span>
        `;
        grid.appendChild(button);
        
        // Animate in
        button.style.opacity = '0';
        button.style.transform = 'translateY(10px)';
        setTimeout(() => {
            button.style.transition = 'all 0.3s ease';
            button.style.opacity = '1';
            button.style.transform = 'translateY(0)';
        }, 10);
    }
    
    markDiscovered(link) {
        link.classList.add('discovered');
        link.textContent += ' ✓';
        this.discovered.add(link.dataset.id);
    }
}
```

#### D. Update Interactable Button Styling
```css
.interactable-btn {
    background: linear-gradient(135deg, #21262d 0%, #161b22 100%);
    border: 1px solid #30363d;
    border-radius: 8px;
    padding: 12px 16px;
    cursor: pointer;
    transition: all 0.2s ease;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    gap: 4px;
    position: relative;
    overflow: hidden;
}

.interactable-btn::before {
    content: '';
    position: absolute;
    left: 0;
    top: 0;
    bottom: 0;
    width: 3px;
    background: #f0883e;
    opacity: 0;
    transition: opacity 0.2s ease;
}

.interactable-btn:hover {
    background: linear-gradient(135deg, #30363d 0%, #21262d 100%);
    border-color: #f0883e;
    transform: translateY(-2px);
    box-shadow: 0 4px 12px rgba(240, 136, 62, 0.2);
}

.interactable-btn:hover::before {
    opacity: 1;
}

.interactable-btn.key-item {
    border-color: #da3633;
    background: linear-gradient(135deg, #2d1f1f 0%, #161b22 100%);
}

.interactable-btn.key-item::before {
    background: #da3633;
}

.interactable-name {
    font-weight: 600;
    color: #e6edf3;
    font-size: 14px;
}

.interactable-action {
    font-size: 12px;
    color: #7d8590;
    font-style: italic;
}
```

### 5. Database Migration

```csharp
// Migration file
public partial class AddDiscoveredInteractables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DiscoveredInteractables",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CharacterId = table.Column<int>(type: "int", nullable: false),
                InteractableId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                LocationId = table.Column<int>(type: "int", nullable: false),
                DiscoveredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                HasInteracted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DiscoveredInteractables", x => x.Id);
                table.ForeignKey(
                    name: "FK_DiscoveredInteractables_Characters_CharacterId",
                    column: x => x.CharacterId,
                    principalTable: "Characters",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DiscoveredInteractables_CharacterId_InteractableId",
            table: "DiscoveredInteractables",
            columns: new[] { "CharacterId", "InteractableId" },
            unique: true);
    }
}
```

### 6. Complete User Flow

1. **Player enters location**
   - Backend generates narrative with `[DISCOVER:id]` markers
   - Only previously discovered interactables shown in INTERACTABLES panel

2. **Player reads narrative**
   - Sees: "...in the corner, [DISCOVER:terminal]a console blinks with amber light[/DISCOVER]..."
   - Renders as clickable link: "a console blinks with amber light"

3. **Player clicks discovery link**
   - Frontend calls API to record discovery
   - Backend saves to `DiscoveredInteractables` table
   - Returns interactable details

4. **UI updates**
   - Link marked as discovered (strikethrough, checkmark)
   - New button appears in INTERACTABLES panel with animation
   - Toast notification: "Discovered: Engineering Terminal"

5. **Player clicks interactable button**
   - Modal opens with interaction content
   - `HasInteracted` flag set to true
   - Any discoveries from interaction recorded

### 7. Edge Cases

| Case | Solution |
|------|----------|
| Player revisits location | Load all previously discovered interactables from DB |
| Narrative regenerated | Preserve discovered state, only new discoveries highlighted |
| Discovery link clicked twice | Ignore duplicate, already in Set |
| Character moves locations | Clear/refresh discovered list for new location |
| Key items | Always highlighted with red border, discovered or not |

### 8. Files to Modify

| File | Changes |
|------|---------|
| `Models/Entities/Character.cs` | Add `DiscoveredInteractables` collection |
| `Models/Entities/DiscoveredInteractable.cs` | **NEW** - Track discoveries |
| `Models/GameData/InteractableInfo.cs` | Add `DiscoveryHint` property |
| `Services/NarrativeService.cs` | Update prompts, add marker parsing |
| `Services/GameSessionService.cs` | Filter interactables by discovery state |
| `Controllers/DiscoveryController.cs` | Add `/api/discovery/interactable` endpoint |
| `Data/ApplicationDbContext.cs` | Add `DiscoveredInteractables` DbSet |
| `Views/Game/Index.cshtml` | Update narrative rendering, interactables grid |
| `wwwroot/css/game-session.css` | Add discovery link styles, button styles |
| `wwwroot/js/discovery.js` | **NEW** - DiscoveryManager class |
| `Data/SeedData.cs` | Update interactable data with discovery hints |

### 9. Migration Steps

1. Create new entity files
2. Create database migration
3. Update prompts in NarrativeService
4. Implement NarrativeParserService
5. Add API endpoint
6. Update GameSessionService filtering logic
7. Update frontend HTML/CSS/JS
8. Test with existing locations
9. Deploy migration

### 10. Testing Checklist

- [ ] Discovery link renders correctly in narrative
- [ ] Clicking link calls API successfully
- [ ] Button appears in INTERACTABLES panel
- [ ] Button has proper styling (gradient, hover, key-item variant)
- [ ] Button opens modal on click
- [ ] Discovery persists on page refresh
- [ ] Previously discovered items load on location entry
- [ ] No duplicate discoveries possible
- [ ] Key items visually distinct
- [ ] Mobile responsive styling

## Benefits

1. **Immersion**: Players explore environment rather than reading lists
2. **Pacing**: Controls information flow - not overwhelming upfront
3. **Discovery**: Sense of achievement when finding interactables
4. **Replayability**: Different playthroughs may discover items in different order
5. **Atmosphere**: Mystery preserved - don't know what's important until examined

## 11. Refinements & Safety Nets

### A. Fallback for Key Items
To prevent soft-locks where the AI fails to generate a discovery link for a critical item:
- **Validation:** `GameSessionService` checks if `IsKeyItem` interactables are present in the parsed narrative.
- **Fallback:** If a Key Item is missing from the narrative (and not yet discovered), automatically append a system message or force-render the button in the panel with a "Sensors detect..." notification.

### B. State-Aware Narrative Generation
The prompt passed to the AI should include the list of *already discovered* items vs *undiscovered* items.
- **Undiscovered:** "A blinking console..." (Vague, clickable)
- **Discovered:** "The Engineering Terminal you accessed earlier..." (Specific, non-clickable or clickable for flavor)

### C. Immediate Feedback Loop
When a player clicks a discovery link:
- **Action:** Frontend shows a toast notification *and* a "flavor text" log entry (e.g., "You brush away the dust to reveal the terminal's interface").
- **Benefit:** Bridges the gap between "clicking a link" and "button appearing".

### D. Technical Considerations
- **Dependency Injection:** Register `NarrativeParserService` in `Program.cs`.
- **Validation:** Ensure `NarrativeParser` validates extracted IDs against `AllLocationInteractables` to prevent "hallucinated" buttons.
- **Accessibility:** Ensure discovery links have `aria-label` describing the action (e.g., "Inspect the blinking console").
