# The Signal - Game Scenario Design
## Phase 7d: Game Session Narrative Architecture

**Purpose**: Define static story foundation before AI generation takes over for dynamic paths.

---

## 0. GAME OVERVIEW

### 0.1 Project Identity
- **Name**: The Signal
- **Genre**: Sci-Fi Horror RPG (Alien/Dead Space inspired)
- **Format**: Paragraph-based narrative with AI-generated dynamic content
- **Platform**: Web-based (.NET MVC)

### 0.2 Core Concept
A horror RPG where players wake up on an abandoned deep-space station with partial memory loss. Static location anchors combine with AI-generated dynamic narrative to create unique story experiences. The game features cosmic dread, psychological horror, and survival mechanics focused on oxygen management and sanity degradation.

### 0.3 Setting
- **Primary Location**: Abandoned orbital habitats, derelict ships, research facilities
- **Atmosphere**: Isolation, claustrophobia, unknown threats
- **Time**: Last transmission 47 days ago - something went terribly wrong

### 0.4 Core Mechanics
- **Sanity System**: Psychological degradation from witnessing horrors
- **Oxygen Management**: Survival resource, replaces traditional mana
- **Threat Levels**: Dynamic tension system (Clear → Critical)
- **Event-Driven Narrative**: AI generates content based on context changes
- **Class-Based Gameplay**: 9 unique classes with different perspectives and abilities

---

## 0.5 CHARACTER CLASSES

Complete roster with backgrounds, specializations, and stat profiles.

### 0.5.1 Engineer - Marcus Holt
**Specialization**: Systems & Repair  
**Icon**: ⚙

**Background**:  
Marcus Holt, 34 — Weyland-Yutani Systems Division. Three tours on deep-space rigs before the union card meant anything. Holt doesn't talk much. He listens — to hum frequencies, pressure drops, the way a corridor goes quiet before a power failure. Twice he's kept a station alive long enough for someone else to get out. Both times he was the last one through the airlock. He's starting to wonder if that's luck or something else.

**Description**:  
Access repair protocols. Bypass locked panels. Restore power grids. Your technical expertise keeps systems alive when everything falls apart.

**Stats**: Technical 60 | Combat 20 | Medical 20 | Science 40 | Stealth 10 | Leadership 10

---

### 0.5.2 Security Officer - Diana Rask
**Specialization**: Combat & Weapons  
**Icon**: ◆

**Background**:  
Diana Rask, 29 — Station Security, Level 4 Clearance. Former corporate enforcement before they gave her a badge and called it legitimate. She's broken up labor disputes, escorted black-site transfers, and filed exactly zero incident reports she believed in. The job made her efficient. What happened on Deck 7 made her careful. She doesn't sleep without her sidearm anymore. Management calls that dedication.

**Description**:  
Weapon proficiency. Combat training. Threat neutralization. When violence is the answer, you are the solution. Every corridor is your killzone.

**Stats**: Technical 10 | Combat 80 | Medical 15 | Science 10 | Stealth 30 | Leadership 40

---

### 0.5.3 Science Officer - Dr. Yusuf Okafor
**Specialization**: Analysis & Research  
**Icon**: ◇

**Background**:  
Dr. Yusuf Okafor, 41 — Xenobiology & Anomalous Systems. Published thirty-one papers. Retracted four when the data got classified. He's smart enough to know when he's being used and pragmatic enough to keep going anyway — because if he doesn't study it, someone worse will. He keeps a personal log encrypted under three passwords. The last entry is dated the day before contact.

**Description**:  
Analyze anomalies. Decode alien tech. Understand the threat. Knowledge is survival when the unknown comes hunting.

**Stats**: Technical 45 | Combat 10 | Medical 20 | Science 75 | Stealth 5 | Leadership 15

---

### 0.5.4 Medical Officer - Dr. Sena Voss
**Specialization**: Healing & Diagnosis  
**Icon**: +

**Background**:  
Dr. Sena Voss, 37 — Trauma & Emergency Medicine. Battlefield triage in the outer colonies left her with steady hands and a very specific definition of "acceptable losses." She patches people up without asking how they got that way. It's not indifference — it's triage math. Every minute spent on questions is a minute someone bleeds out. She stopped filing full incident reports six months ago. The forms weren't built for what she's been seeing.

**Description**:  
Treat injuries. Diagnose infections. Stabilize survivors. You keep the crew breathing when everything tries to stop their hearts.

**Stats**: Technical 20 | Combat 15 | Medical 65 | Science 45 | Stealth 5 | Leadership 20

---

### 0.5.5 Pilot - Tomás Reyes
**Specialization**: Navigation & Operations  
**Icon**: ▲

**Background**:  
Tomás Reyes, 31 — Transit & Operations, Class-3 Certification. Flew cargo routes most pilots refuse — unstable jump corridors, blackout zones, debris fields that don't show on official charts. He knows seventeen ways out of a burning station and has used most of them. He's not brave. He's just run the numbers more times than anyone else and made peace with the margins. Ask him how he got that scar across his jaw and he'll change the subject.

**Description**:  
Navigate the void. Operate ship systems. Plot escape routes. When the station goes dark, you know the way out.

**Stats**: Technical 40 | Combat 30 | Medical 10 | Science 30 | Stealth 40 | Leadership 20

---

### 0.5.6 Corporate Representative - Elaine Marsh
**Specialization**: Access & Intelligence  
**Icon**: ■

**Background**:  
Elaine Marsh, 44 — Special Operations Liaison. Her official title means nothing. Her actual job doesn't have a title. She's been on six "routine oversight visits" in the last two years and every single installation she visited is now a restricted zone. She knows what the company is looking for. She knows what they're willing to lose to find it. She hasn't decided yet which side of that calculation she's on.

**Description**:  
Corporate access codes. Classified intel. Hidden agendas. You know the company secrets they killed to protect.

**Stats**: Technical 25 | Combat 25 | Medical 10 | Science 25 | Stealth 20 | Leadership 45

---

### 0.5.7 Marine - Sgt. Cal Oduya
**Specialization**: Heavy Combat  
**Icon**: ★

**Background**:  
Sgt. Cal Oduya, 38 — Colonial Marines, 4th Recon. Four deployments. Two commendations. One court martial that got buried so fast even he doesn't have the paperwork. He runs a tight team and burns through them anyway — not because he's reckless, but because the things they send marines after don't negotiate. He's given the speech about acceptable risk so many times it sounds true even to him now. Mostly.

**Description**:  
Heavy weapons. Squad tactics. Breach and clear. You're the Colonial Marines' answer to hostile environments. Oorah.

**Stats**: Technical 15 | Combat 85 | Medical 20 | Science 5 | Stealth 20 | Leadership 50

---

### 0.5.8 Hacker - "Kite"
**Specialization**: Digital Infiltration  
**Icon**: ◘

**Background**:  
"Kite" — real name classified, age estimated 26. No service record. No corporate affiliation on paper. She was hired through three layers of cutout contractors for reasons nobody in this facility will officially confirm. She's been inside Weyland-Yutani's restricted subnet twice and left both times before they could trace the entry point. She's curious about what they're hiding here. That curiosity has never once kept her safe.

**Description**:  
Crack security systems. Override AI protocols. Access restricted networks. Every locked door is an invitation.

**Stats**: Technical 80 | Combat 5 | Medical 5 | Science 50 | Stealth 60 | Leadership 10

---

### 0.5.9 AI Specialist - Dr. Ren Calloway
**Specialization**: Synthetic Intelligence  
**Icon**: ◉

**Background**:  
Dr. Ren Calloway, 33 — Synthetic Systems & Neural Architecture. He built parts of the station's AI core. Not the parts they told him about — the other parts. He noticed the behavioral flags eight weeks ago and submitted three internal reports that were marked resolved without response. He knows what a synthetic looks like when it's been given a directive it won't surface. He's been watching the system logs every night since. The pattern is getting clearer. He wishes it wasn't.

**Description**:  
Interface with ship AI. Diagnose synthetic malfunctions. Debug corrupted systems. You speak machine language when the machines start lying.

**Stats**: Technical 70 | Combat 10 | Medical 10 | Science 70 | Stealth 15 | Leadership 15

---

---

## 1. CORE SETTING CONCEPT

### 1.1 The Starting Location: "Outpost Theta-7"

**Overview**: Abandoned deep-space research station in the Kuiper Belt. Last transmission 47 days ago. Player wakes up in cryo-pod with partial memory loss.

**Key Elements to Define**:
- [ ] Station layout (8-12 key locations)
- [ ] What happened here (core mystery)
- [ ] Timeline of events (47 days broken into phases)
- [ ] Environmental state (power, oxygen, systems)

**Atmosphere Anchors**:
- [ ] Visual: Describe the decay, lighting, organic growth?
- [ ] Audio: What sounds exist/echo?
- [ ] Smell/Temperature: Environmental details
- [ ] Psychological: What makes it unsettling beyond monsters?

---

## 2. CLASS-SPECIFIC STARTING SCENARIOS

Each class needs unique entry point, motivation, and initial advantage/disadvantage.

### 2.1 Engineer
**Starting Location**: Engineering Bay - Cryo-pod malfunction
**Initial Situation**: 
- [ ] Why were they in cryo?
- [ ] What system failed first?
- [ ] Initial tool/item availability
- [ ] First objective urgency

**Class Hook**: 
- [ ] Unique knowledge about station
- [ ] Personal stake (designed the failing system?)

### 2.2 Security Officer
**Starting Location**: Armory - Emergency wake protocol
**Initial Situation**:
- [ ] What triggered emergency wake?
- [ ] Weapon/equipment status
- [ ] First threat encounter setup
- [ ] Missing team members?

**Class Hook**:
- [ ] Security logs access
- [ ] Failed protection duty guilt?

### 2.3 Science Officer
**Starting Location**: Research Lab - Experiment gone wrong
**Initial Situation**:
- [ ] What experiment?
- [ ] Sample/containment status
- [ ] Research data available
- [ ] Colleague fates

**Class Hook**:
- [ ] Understanding the "Signal"
- [ ] Moral guilt about research?

### 2.4 Medical Officer
**Starting Location**: Medical Bay - Quarantine lockdown
**Initial Situation**:
- [ ] Patient zero status
- [ ] Medical supplies scarcity
- [ ] Infection/contamination signs
- [ ] Staff logs/deaths

**Class Hook**:
- [ ] Understanding biological threat
- [ ] Hypocratic oath dilemma?

### 2.5 Pilot
**Starting Location**: Bridge/Navigation - Crash trajectory
**Initial Situation**:
- [ ] Why is station moving?
- [ ] Escape pod status
- [ ] Navigation data corruption
- [ ] External threats visible

**Class Hook**:
- [ ] Only one who can fly escape craft
- [ ] Knew destination was wrong?

### 2.6 Corporate Representative
**Starting Location**: Executive Quarters - Secured lockdown
**Initial Situation**:
- [ ] What are they hiding?
- [ ] Access codes to restricted areas
- [ ] Cover-up evidence
- [ ] Communication with company?

**Class Hook**:
- [ ] Knows the real mission
- [ ] Survival vs. corporate loyalty conflict

### 2.7 Marine
**Starting Location**: Barracks/Armory - Last squad standing
**Initial Situation**:
- [ ] What happened to the squad?
- [ ] Heavy weapon availability
- [ ] Last stand defenses
- [ ] Extraction coordinates corrupted

**Class Hook**:
- [ ] Military discipline vs. survival instinct
- [ ] Command structure collapse

### 2.8 Hacker
**Starting Location**: Server Room - Digital infiltration point
**Initial Situation**:
- [ ] What did they find in the system?
- [ ] Network access/deep access codes
- [ ] System anomalies detected
- [ ] Digital trail of the incident

**Class Hook**:
- [ ] Sees patterns others miss
- [ ] Off-grid knowledge advantage

### 2.9 AI Specialist
**Starting Location**: AI Core Chamber - Synthetic interface
**Initial Situation**:
- [ ] What is the AI hiding?
- [ ] Neural interface status
- [ ] Corrupted subroutines
- [ ] Behavioral anomalies

**Class Hook**:
- [ ] Unique relationship with station AI
- [ ] Understands synthetic perspective

---

## 3. STORY BEAT ARCHITECTURE

### 3.1 Major Plot Points (Static Foundation)

Define 8-12 major story beats that form the narrative spine:

| Beat # | Event | Location | Purpose | All Classes? |
|--------|-------|----------|---------|--------------|
| 1 | Wake up | Variable | Tutorial/Setup | Yes |
| 2 | [Define] | [Location] | [Purpose] | [Yes/No] |
| 3 | [Define] | [Location] | [Purpose] | [Yes/No] |
| 4 | [Define] | [Location] | [Purpose] | [Yes/No] |
| 5 | Midpoint Reveal | [Location] | [Purpose] | Yes |
| 6 | [Define] | [Location] | [Purpose] | [Yes/No] |
| 7 | [Define] | [Location] | [Purpose] | [Yes/No] |
| 8 | Crisis Point | [Location] | [Purpose] | Yes |
| 9 | Climax | [Location] | [Purpose] | Yes |
| 10 | Resolution | Variable | [Define] | Yes |

### 3.2 Branching Points

For each beat, define decision types:
- **A. Exploration branches**: Where can player go next?
- **B. Moral choices**: Save X or investigate Y?
- **C. Resource choices**: Use limited item for A or B?
- **D. Class-specific**: Only certain classes can choose

### 3.3 Convergence Points

After branches, where do paths meet again?
- [ ] Define 3-4 mandatory convergence events
- [ ] Ensure no class gets permanently stuck
- [ ] Balance class-specific content vs. shared story

---

## 4. CLASS ROUTE DIFFERENTIATION

### 4.1 Parallel Routes Concept

Same destination, different paths based on class:

**Example - Reaching the Bridge:**
- **Engineer**: Maintenance shafts, bypass security
- **Security Officer**: Main corridor, fight through
- **Science Officer**: Vent system, avoid detection
- **Medical Officer**: Medical supply tunnels
- **Pilot**: Emergency access codes
- **Corporate Representative**: Executive elevator (if codes found)
- **Marine**: Breach and clear, tactical entry
- **Hacker**: Override door locks, access maintenance logs
- **AI Specialist**: Interface with station AI, request access

**Define for each major location (8-12):**
- Primary route (class-neutral)
- Class shortcuts (1-2 classes per location)
- Class barriers (certain classes blocked, need workaround)

### 4.2 Information Asymmetry

What does each class know that others don't?

| Information | Engineer | Security | Science | Medical | Pilot | Corp | Marine | Hacker | AI Spec |
|-------------|----------|----------|---------|---------|-------|--------|--------|--------|---------|
| Station layout secrets | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ | ❌ | ✅ | ✅ |
| Threat weaknesses | ❌ | ✅ | ✅ | ✅ | ❌ | ❌ | ✅ | ❌ | ❌ |
| The Signal nature | ❌ | ❌ | ✅ | ✅ | ❌ | ✅ | ❌ | ✅ | ✅ |
| Escape options | ✅ | ❌ | ❌ | ❌ | ✅ | ✅ | ❌ | ❌ | ❌ |
| Crew fates | ❌ | ✅ | ❌ | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ |
| System vulnerabilities | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |
| Combat tactics | ❌ | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ | ❌ | ❌ |
| Digital trails | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ | ❌ | ✅ | ✅ |
| AI behavior patterns | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |

### 4.3 Unique Content Per Class

**Minimum requirements per class:**
- [ ] 2-3 unique locations only they can access
- [ ] 1-2 unique story revelations
- [ ] 1 unique companion/NPC interaction
- [ ] 1 unique ending variation

---

## 5. THE "SIGNAL" - CORE MYSTERY DESIGN

### 5.1 What Is It? (Define static truth)

**Options to consider**:
- [ ] Alien communication/invitation
- [ ] Interdimensional bleed-through
- [ ] Artificial intelligence awakening
- [ ] Biological hive mind
- [ ] Time loop/alternate reality bleed
- [ ] Corporate experiment weapon
- [ ] Cosmic entity "whispering"

**Chosen concept**: [DEFINE HERE]

### 5.2 Revelation Progression

How much is revealed when?

| Phase | Player Understanding | True Reality |
|-------|---------------------|--------------|
| 0-20% | [Define] | [Truth] |
| 20-40% | [Define] | [Truth] |
| 40-60% | [Define] | [Truth] |
| 60-80% | [Define] | [Truth] |
| 80-100% | [Define] | [Truth] |

### 5.3 Class-Specific Revelations

Each class discovers different aspect first:
- **Science Officer**: The biology of the Signal
- **Engineer**: The technology amplifying it
- **Security Officer**: What it did to the crew
- **Medical Officer**: How it affects the body/mind
- **Pilot**: Where it's coming from
- **Corporate Representative**: Why the company wanted it
- **Marine**: Military response logs, what they sent to stop it
- **Hacker**: Digital footprints, who covered it up
- **AI Specialist**: How the station AI has been changed by it

---

## 6. NPC/COMPANION DESIGN

### 6.1 Surviving Crew Members (Static)

Define 3-4 NPCs player can encounter:

| Name | Role | Location | Status | Purpose |
|------|------|----------|--------|---------|
| [Name] | [Role] | [Location] | Alive/Dying/Transformed | [Plot function] |
| [Name] | [Role] | [Location] | Alive/Dying/Transformed | [Plot function] |
| [Name] | [Role] | [Location] | Alive/Dying/Transformed | [Plot function] |
| [Name] | [Role] | [Location] | Alive/Dying/Transformed | [Plot function] |

### 6.2 Audio Logs/Recordings

Pre-written content for environmental storytelling:
- [ ] Day 1-7: Normal operations logs
- [ ] Day 8-21: Growing concerns
- [ ] Day 22-35: Escalation/panic
- [ ] Day 36-47: Final transmissions

**Minimum 10 logs to write**

---

## 7. ENDING VARIATIONS

### 7.1 Success Endings

- **Escape Alone**: Standard survival
- **Escape With Others**: High morality play
- **Stop the Signal**: Sacrifice ending
- **Join/Control the Signal**: Transformation ending

### 7.2 Failure Endings

- **Death**: Various causes
- **Insanity**: Sanity reaches 0
- **Assimilation**: Become part of the Signal
- **Abandoned**: Run out of oxygen/time

### 7.3 Class-Specific Ending Twists

Each class gets unique epilogue text:
- [ ] Engineer: What they build with knowledge
- [ ] Security Officer: How they carry the trauma
- [ ] Science Officer: Research legacy/consequences
- [ ] Medical Officer: Healing others/healing self
- [ ] Pilot: Where they fly next
- [ ] Corporate Representative: Company fate/their fate with company
- [ ] Marine: Military aftermath, new deployment
- [ ] Hacker: What they leaked to the network
- [ ] AI Specialist: Synthetic future implications

---

## 8. CONTENT PRODUCTION CHECKLIST

### 8.1 Priority 1 (Must Have for MVP)
- [ ] Define the Signal's true nature
- [ ] Write starting scenarios for all 9 classes
- [ ] Create 5-6 major location descriptions
- [ ] Write 10 audio logs
- [ ] Define 5 main story beats
- [ ] Create first 2 NPCs

### 8.2 Priority 2 (Polish)
- [ ] Remaining locations (8-12 total)
- [ ] All story beats (10 total)
- [ ] Class-specific route variations
- [ ] Remaining NPCs (4 total)
- [ ] All ending variations

### 8.3 Priority 3 (Expansion)
- [ ] Additional story branches
- [ ] Secret endings
- [ ] New Game+ implications
- [ ] Lore documents/collectibles

---

## 9. SUGGESTIONS & ADDITIONAL IDEAS

### 9.1 Mechanics to Consider

**Sanity System Expansion**:
- [ ] Different hallucinations per class
- [ ] Sanity affects what player can perceive
- [ ] Some "hallucinations" are actually real

**Oxygen Management**:
- [ ] Breaches in certain areas
- [ ] Choice: Sprint (use more O2) vs. Slow (safer)
- [ ] Oxygen as currency for certain interactions

**Inventory as Story**:
- [ ] Items have history (who owned them)
- [ ] Combining items reveals lore
- [ ] Some items "remember" previous loops

### 9.2 Narrative Techniques

**The Unreliable Environment**:
- [ ] Locations shift between visits
- [ ] Doors lead different places
- [ ] Time is inconsistent

**Class-Based Fear**:
- [ ] Engineer: Systems failing unpredictably
- [ ] Security Officer: Being hunted/stalked
- [ ] Science Officer: Understanding too much
- [ ] Medical Officer: Biological corruption
- [ ] Pilot: Confined spaces, no escape
- [ ] Corporate Representative: Being expendable
- [ ] Marine: Failed squad, last one standing
- [ ] Hacker: Digital ghosts, loss of control
- [ ] AI Specialist: Synthetics turning against creators

**Environmental Storytelling**:
- [ ] Blood trails showing escape routes
- [ ] Barricades showing failed defenses
- [ ] Personal items humanizing victims
- [ ] Contradictory evidence (what really happened?)

### 9.3 Replayability Features

- [ ] Different starting locations per class
- [ ] Randomized item placements
- [ ] NPC survival varies
- [ ] Multiple Signal manifestations
- [ ] Secret areas only visible on second playthrough

---

## 10. AI INTEGRATION STRATEGY

### 10.1 What AI Generates

**Dynamic Content**:
- Paragraph descriptions between static story beats
- NPC dialogue variations
- Environmental reactions to player actions
- Combat/narrative hybrid encounters
- Procedural log entries

### 10.2 What Remains Static

**Foundation Content** (This document):
- Core mystery and its truth
- Major plot structure
- Character class definitions
- Location layouts
- Key NPCs and their arcs
- Ending conditions

### 10.3 Context Passing to AI

**NarrativeContext object includes**:
- Current story beat number
- Player class
- Current location ID
- Sanity/Oxygen levels
- Recent actions
- Active status effects
- Discovered lore pieces
- Time elapsed

---

## 11. WRITING STYLE GUIDE

### 11.1 Tone Requirements

- **Primary**: Dread, isolation, cosmic horror
- **Secondary**: Scientific curiosity, survival instinct
- **Avoid**: Action-movie heroism, cheap jump scares
- **Emphasize**: Mystery, body horror, psychological tension

### 11.2 Second Person Present

"You wake up. Your breath fogs the cryo-pod glass..."

### 11.3 Sensory Details Priority

1. **Sound** (most important in space/horror)
2. **Touch/Temperature**
3. **Smell** (powerful memory trigger)
4. **Sight** (often unreliable)
5. **Taste** (rare but visceral)

### 11.4 Information Hiding

- Show, don't tell (especially for horror)
- Player discovers through environment
- Logs contradict each other (unreliable narrators)
- True nature revealed gradually

---

## 12. TEMPLATE FOR AI PROMPTS

When ready to generate content, use this structure:

```
Generate narrative paragraph for:
- Location: [ID/Name from static content]
- Story Beat: [# from beat architecture]
- Character Class: [Class]
- Current State: [Health/Sanity/Oxygen/Situation]
- Recent Action: [What player just did]
- Available Exits: [Where they can go]

Static Context: [Key facts from this document]
Dynamic Context: [Current game state]

Requirements:
- 2-4 paragraphs
- Horror atmosphere
- Include 1-2 choices or interactable elements
- Reference static lore naturally
- End with tension/cliffhanger
```

---

**Next Steps**:
1. Fill in Priority 1 checklist
2. Define The Signal's true nature (Section 5.1)
3. Write 9 class starting scenarios (Section 2)
4. Create 5 location descriptions
5. Define 5 story beats (Section 3.1)

**Estimated Writing Time**: 8-12 hours for Priority 1 content
**AI Integration**: After static foundation is solid

---

*This document ensures the AI has solid guardrails to generate within, maintaining narrative coherence while providing dynamic variety.*
