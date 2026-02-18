using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Models.GameData;

namespace TheSignal.Data;

public static class SeedData
{
    public static List<Location> GetLocations()
    {
        return new List<Location>
        {
            // Core Locations
            new Location
            {
                Id = 1,
                Name = "Cryo Bay - Sector A1",
                ContentKey = "cryo_bay",
                BaseDescription = "Thirty-two cryo-pods arranged in four rows. Frost clings to every surface. Most pods show green status lights—occupied, stable. Yours is open, glass fogged with condensation.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Isolated,
                ThreatLevel = ThreatLevel.Unknown,
                KeyElements = new List<string> { "cryo_pods", "emergency_locker", "damaged_pod", "terminal" },
                IsActive = true,
                Deck = 1,
                Sector = "A1"
            },
            new Location
            {
                Id = 2,
                Name = "Engineering Bay - Sector A3",
                ContentKey = "engineering_bay",
                BaseDescription = "Cathedral of machinery. Three-story vertical space dominated by the primary reactor column, pulsing with dim blue light. Catwalks crisscross overhead.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Claustrophobic,
                ThreatLevel = ThreatLevel.Disturbance,
                KeyElements = new List<string> { "reactor", "crystal_formations", "engineering_terminal", "jennings_workstation" },
                IsActive = true,
                Deck = 1,
                Sector = "A3"
            },
            new Location
            {
                Id = 3,
                Name = "Medical Bay - Sector A2",
                ContentKey = "medical_bay",
                BaseDescription = "White walls now stained with chemical burns and rust-brown streaks. Examination beds line both sides, restraints hanging loose.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Contaminated,
                DefaultMood = MoodType.Terrifying,
                ThreatLevel = ThreatLevel.Disturbance,
                KeyElements = new List<string> { "medical_terminal", "supply_cabinet", "wall_notes", "biohazard_door" },
                IsActive = true,
                Deck = 1,
                Sector = "A2"
            },
            new Location
            {
                Id = 4,
                Name = "Bridge / Command Center - Sector B1",
                ContentKey = "bridge",
                BaseDescription = "Wraparound windows show the Kuiper Belt's dark—distant ice catching starlight like broken glass. Command chairs face dead viewscreens.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.LightsOut,
                DefaultAtmosphere = AtmosphereType.Vacuum,
                DefaultMood = MoodType.Isolated,
                ThreatLevel = ThreatLevel.Clear,
                KeyElements = new List<string> { "captain_terminal", "navigation_console", "external_sensors" },
                IsActive = true,
                Deck = 1,
                Sector = "B1"
            },
            new Location
            {
                Id = 5,
                Name = "Research Lab Alpha - Sector C2",
                ContentKey = "research_lab",
                BaseDescription = "Where it started. The containment vessel sits open in the center of the lab, surrounded by sensor equipment and scorch marks.",
                Biome = BiomeType.Laboratory,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Contaminated,
                DefaultMood = MoodType.Terrifying,
                ThreatLevel = ThreatLevel.Contact,
                KeyElements = new List<string> { "containment_vessel", "research_terminals", "sample_storage", "emergency_purge" },
                IsActive = true,
                Deck = 1,
                Sector = "C2"
            },
            new Location
            {
                Id = 6,
                Name = "Sub-Level 3 - The Changed Sector",
                ContentKey = "sub_level_3",
                BaseDescription = "No longer recognizably human architecture. The walls have been rebuilt—station materials melted and reformed into smooth, organic curves following non-Euclidean geometry.",
                Biome = BiomeType.DerelictShip,
                DefaultTime = TimeOfDay.LightsOut,
                DefaultAtmosphere = AtmosphereType.Contaminated,
                DefaultMood = MoodType.Terrifying,
                ThreatLevel = ThreatLevel.Contact,
                KeyElements = new List<string> { "the_changed", "construction_sites", "organic_terminals", "deep_shaft" },
                IsActive = true,
                Deck = 0,
                Sector = "S"
            },
            new Location
            {
                Id = 7,
                Name = "Armory - Sector B2",
                ContentKey = "armory",
                BaseDescription = "Sealed on emergency lockdown. Red lights and proximity alarms. Weapon racks mostly stripped.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Suspenseful,
                ThreatLevel = ThreatLevel.Disturbance,
                KeyElements = new List<string> { "weapon_racks", "security_locker", "proximity_alarm", "sealed_door" },
                IsActive = true,
                Deck = 1,
                Sector = "B2"
            },
            new Location
            {
                Id = 8,
                Name = "Barracks - Sector B2",
                ContentKey = "barracks",
                BaseDescription = "Fortified living quarters. Furniture barricades the doors. Empty magazines litter the floor.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Paranoid,
                ThreatLevel = ThreatLevel.Disturbance,
                KeyElements = new List<string> { "fortified_positions", "discarded_magazines", "personal_lockers", "ration_packs" },
                IsActive = true,
                Deck = 1,
                Sector = "B2"
            },
            
            // Class-Exclusive Locations
            new Location
            {
                Id = 9,
                Name = "Sub-Level Maintenance - Engineering Access",
                ContentKey = "maintenance_shaft",
                BaseDescription = "Cramped crawlspace running beneath Engineering Bay. Exposed conduits line every surface.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Claustrophobic,
                ThreatLevel = ThreatLevel.Disturbance,
                KeyElements = new List<string> { "jennings_modified_systems", "engineering_logs", "crystal_integration" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.Engineer,
                IsActive = true,
                Deck = 0,
                Sector = "S1"
            },
            new Location
            {
                Id = 10,
                Name = "Security Hub - Sector B2",
                ContentKey = "security_hub",
                BaseDescription = "Surveillance nerve center. Sixteen monitors displaying feeds from every station section.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Isolated,
                ThreatLevel = ThreatLevel.Clear,
                KeyElements = new List<string> { "surveillance_monitors", "security_logs", "tactical_vest", "comms_recording" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.SecurityOfficer,
                IsActive = true,
                Deck = 1,
                Sector = "B2"
            },
            new Location
            {
                Id = 11,
                Name = "Xenoarchaeological Analysis Lab - Sector C3",
                ContentKey = "xenoarch_lab",
                BaseDescription = "Sterile chamber filled with analysis equipment and shifting holographic molecular structures.",
                Biome = BiomeType.Laboratory,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Suspenseful,
                ThreatLevel = ThreatLevel.Clear,
                KeyElements = new List<string> { "vessel_fragments", "spectral_analyzers", "okafor_workstation", "sample_containers" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.ScienceOfficer,
                IsActive = true,
                Deck = 1,
                Sector = "C3"
            },
            new Location
            {
                Id = 12,
                Name = "Quarantine Ward - Sector C1",
                ContentKey = "quarantine_ward",
                BaseDescription = "Isolation ward with twelve beds. Patients in various stages of transformation.",
                Biome = BiomeType.MedicalBay,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Contaminated,
                DefaultMood = MoodType.Terrifying,
                ThreatLevel = ThreatLevel.Disturbance,
                KeyElements = new List<string> { "transformed_patients", "medical_monitors", "voss_notes", "neural_suppressant" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.MedicalOfficer,
                IsActive = true,
                Deck = 1,
                Sector = "C1"
            },
            new Location
            {
                Id = 13,
                Name = "Navigation Control Bay - Sector B1",
                ContentKey = "navigation_bay",
                BaseDescription = "Holographic star charts and trajectory calculations. Three pilot chairs face instrument clusters.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Suspenseful,
                ThreatLevel = ThreatLevel.Clear,
                KeyElements = new List<string> { "star_charts", "instrument_clusters", "reyes_chair", "trajectory_data" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.Pilot,
                IsActive = true,
                Deck = 1,
                Sector = "B1"
            },
            new Location
            {
                Id = 14,
                Name = "Executive Archive - Sector B3",
                ContentKey = "executive_archive",
                BaseDescription = "Secure data vault. Black walls lined with encrypted storage units. Corporate classification headers.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Isolated,
                ThreatLevel = ThreatLevel.Clear,
                KeyElements = new List<string> { "encrypted_data_vault", "corporate_terminals", "marsh_workstation", "physical_files" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.CorporateRep,
                IsActive = true,
                Deck = 1,
                Sector = "B3"
            },
            new Location
            {
                Id = 15,
                Name = "Server Core - Sector S1",
                ContentKey = "server_core",
                BaseDescription = "Digital fortress. Server banks line the walls, cooling systems maintaining optimal temperature for quantum processors.",
                Biome = BiomeType.Laboratory,
                DefaultTime = TimeOfDay.LightsOut,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Isolated,
                ThreatLevel = ThreatLevel.Clear,
                KeyElements = new List<string> { "server_banks", "quantum_processors", "kite_workstation", "mother_access" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.Hacker,
                IsActive = true,
                Deck = 0,
                Sector = "S1"
            },
            new Location
            {
                Id = 16,
                Name = "MOTHER AI Core - Sector S2",
                ContentKey = "mother_core",
                BaseDescription = "Heart of the station. Quantum processor arrays suspended in electromagnetic containment fields.",
                Biome = BiomeType.Laboratory,
                DefaultTime = TimeOfDay.LightsOut,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Isolated,
                ThreatLevel = ThreatLevel.Clear,
                KeyElements = new List<string> { "quantum_array", "mother_thought_screens", "calloway_workstation", "neural_interface" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.AISpecialist,
                IsActive = true,
                Deck = 0,
                Sector = "S2"
            },
            new Location
            {
                Id = 17,
                Name = "Tactical Operations Center - Sector B2",
                ContentKey = "tactical_ops",
                BaseDescription = "War room. Tactical displays showing station layout with threat assessment overlays.",
                Biome = BiomeType.SpaceStation,
                DefaultTime = TimeOfDay.EmergencyLighting,
                DefaultAtmosphere = AtmosphereType.Normal,
                DefaultMood = MoodType.Suspenseful,
                ThreatLevel = ThreatLevel.Disturbance,
                KeyElements = new List<string> { "holotable", "squad_status_displays", "tactical_logs", "weapons_vault" },
                IsClassExclusive = true,
                RequiredClass = CharacterClass.Marine,
                IsActive = true,
                Deck = 1,
                Sector = "B2"
            }
        };
    }
    
    public static List<LocationExit> GetLocationExits()
    {
        return new List<LocationExit>
        {
            // Deck 1 - Sector A (Industrial/Medical)
            new LocationExit { Id = 1, LocationId = 1, TargetLocationId = 2, Direction = "North", Description = "Main Corridor A leads toward Engineering Bay", GapSize = 3, GapType = "Corridor A" },
            new LocationExit { Id = 2, LocationId = 1, TargetLocationId = 3, Direction = "East", Description = "Medical Bay via Corridor A", GapSize = 3, GapType = "Corridor A" },
            new LocationExit { Id = 3, LocationId = 2, TargetLocationId = 1, Direction = "South", Description = "Return to Cryo Bay", GapSize = 3, GapType = "Corridor A" },
            new LocationExit { Id = 4, LocationId = 3, TargetLocationId = 1, Direction = "West", Description = "Return to Cryo Bay", GapSize = 3, GapType = "Corridor A" },
            
            // Deck 1 - Sector B (Command/Security)
            new LocationExit { Id = 5, LocationId = 4, TargetLocationId = 2, Direction = "South", Description = "Main Corridor B to Engineering", GapSize = 4, GapType = "Corridor B" },
            new LocationExit { Id = 6, LocationId = 2, TargetLocationId = 4, Direction = "North", Description = "Main Corridor B to Bridge", GapSize = 4, GapType = "Corridor B" },
            
            new LocationExit { Id = 7, LocationId = 7, TargetLocationId = 8, Direction = "East", Description = "Barracks", GapSize = 0 },
            new LocationExit { Id = 8, LocationId = 8, TargetLocationId = 7, Direction = "West", Description = "Armory", GapSize = 0 },
            new LocationExit { Id = 9, LocationId = 7, TargetLocationId = 2, Direction = "South", Description = "Engineering Bay via Corridor B", GapSize = 3, GapType = "Corridor B" },
            new LocationExit { Id = 10, LocationId = 2, TargetLocationId = 7, Direction = "North", Description = "Armory Access via Corridor B", GapSize = 3, GapType = "Corridor B" },
            
            // Deck 1 - Sector C (Science)
            new LocationExit { Id = 11, LocationId = 3, TargetLocationId = 5, Direction = "East", Description = "Research Lab Alpha via Corridor C", GapSize = 3, GapType = "Corridor C" },
            new LocationExit { Id = 12, LocationId = 5, TargetLocationId = 3, Direction = "West", Description = "Medical Bay via Corridor C", GapSize = 3, GapType = "Corridor C" },
            
            // Deck 0 (Sub-Levels)
            new LocationExit { Id = 13, LocationId = 2, TargetLocationId = 6, Direction = "Down", Description = "Sub-Level 3 Access", GapSize = 2, GapType = "Maintenance Shaft" },
            new LocationExit { Id = 14, LocationId = 6, TargetLocationId = 2, Direction = "Up", Description = "Engineering Bay", GapSize = 2, GapType = "Maintenance Shaft" },
            new LocationExit { Id = 15, LocationId = 5, TargetLocationId = 6, Direction = "Down", Description = "Sub-Level 3 Research", GapSize = 2, GapType = "Maintenance Shaft" },
            new LocationExit { Id = 16, LocationId = 6, TargetLocationId = 5, Direction = "Up", Description = "Research Lab Alpha", GapSize = 2, GapType = "Maintenance Shaft" },

            // Class-Exclusive Connections
            // Engineer
            new LocationExit { Id = 17, LocationId = 2, TargetLocationId = 9, Direction = "Maintenance", Description = "Maintenance Shaft Access", RequiredClass = CharacterClass.Engineer, RequiredTechnical = 60, GapSize = 1, GapType = "Maintenance Shaft" },
            new LocationExit { Id = 18, LocationId = 9, TargetLocationId = 2, Direction = "Up", Description = "Return to Engineering Bay", GapSize = 1, GapType = "Maintenance Shaft" },
            
            // Security
            new LocationExit { Id = 19, LocationId = 7, TargetLocationId = 10, Direction = "West", Description = "Security Hub", RequiredClass = CharacterClass.SecurityOfficer, GapSize = 0 },
            new LocationExit { Id = 20, LocationId = 10, TargetLocationId = 7, Direction = "East", Description = "Armory", GapSize = 0 },
            
            // Science
            new LocationExit { Id = 21, LocationId = 5, TargetLocationId = 11, Direction = "West", Description = "Analysis Lab", RequiredClass = CharacterClass.ScienceOfficer, GapSize = 0 },
            new LocationExit { Id = 22, LocationId = 11, TargetLocationId = 5, Direction = "East", Description = "Research Lab Alpha", GapSize = 0 },
            
            // Medical
            new LocationExit { Id = 23, LocationId = 3, TargetLocationId = 12, Direction = "East", Description = "Quarantine Ward", RequiredClass = CharacterClass.MedicalOfficer, GapSize = 0 },
            new LocationExit { Id = 24, LocationId = 12, TargetLocationId = 3, Direction = "West", Description = "Medical Bay", GapSize = 0 },
            
            // Pilot
            new LocationExit { Id = 25, LocationId = 4, TargetLocationId = 13, Direction = "West", Description = "Navigation Bay", RequiredClass = CharacterClass.Pilot, GapSize = 0 },
            new LocationExit { Id = 26, LocationId = 13, TargetLocationId = 4, Direction = "East", Description = "Bridge", GapSize = 0 },
            
            // Corporate
            new LocationExit { Id = 27, LocationId = 4, TargetLocationId = 14, Direction = "West", Description = "Executive Archive", RequiredClass = CharacterClass.CorporateRep, GapSize = 0 },
            new LocationExit { Id = 28, LocationId = 14, TargetLocationId = 4, Direction = "East", Description = "Bridge", GapSize = 0 },
            
            // Hacker
            new LocationExit { Id = 29, LocationId = 6, TargetLocationId = 15, Direction = "Down", Description = "Server Core", RequiredClass = CharacterClass.Hacker, GapSize = 1, GapType = "Maintenance Shaft" },
            new LocationExit { Id = 30, LocationId = 15, TargetLocationId = 6, Direction = "Up", Description = "Sub-Level 3", GapSize = 1, GapType = "Maintenance Shaft" },
            
            // AI Specialist
            new LocationExit { Id = 31, LocationId = 15, TargetLocationId = 16, Direction = "Core", Description = "MOTHER Core", RequiredClass = CharacterClass.AISpecialist, GapSize = 0 },
            new LocationExit { Id = 32, LocationId = 16, TargetLocationId = 15, Direction = "Back", Description = "Server Core", GapSize = 0 },
            
            // Marine
            new LocationExit { Id = 33, LocationId = 8, TargetLocationId = 17, Direction = "Tactical", Description = "Tactical Ops", RequiredClass = CharacterClass.Marine, GapSize = 0 },
            new LocationExit { Id = 34, LocationId = 17, TargetLocationId = 8, Direction = "Back", Description = "Barracks", GapSize = 0 }
        };
    }
    
    public static List<Item> GetItems()
    {
        return new List<Item>
        {
            new Item
            {
                Id = 1,
                Name = "Engineering Toolkit",
                Description = "Standard-issue maintenance tools for station repairs",
                Type = ItemType.Tool,
                Effect = "Allows technical repairs and bypassing basic panels"
            },
            new Item
            {
                Id = 2,
                Name = "Diagnostic Scanner",
                Description = "Personal diagnostic device, modified to show readings in additional spectrums",
                Type = ItemType.Tool,
                Effect = "Enhanced system analysis capabilities"
            },
            new Item
            {
                Id = 3,
                Name = "Emergency Oxygen Mask",
                Description = "Portable oxygen supply for hazardous environments",
                Type = ItemType.Medical,
                Effect = "Prevents oxygen loss in contaminated areas"
            },
            new Item
            {
                Id = 4,
                Name = "Maintenance Access Codes",
                Description = "Security codes for station maintenance areas",
                Type = ItemType.Keycard,
                Effect = "Grants access to restricted maintenance zones"
            },
            new Item
            {
                Id = 5,
                Name = "Jennings' Personal Log",
                Description = "Engineering log entries from your apprentice",
                Type = ItemType.Recording,
                Effect = "Contains technical data about station modifications"
            },
            new Item
            {
                Id = 6,
                Name = "Power Core Fragment",
                Description = "Crystalline structure integrated with reactor systems",
                Type = ItemType.Tool,
                Effect = "Emits harmonic frequencies when near the Signal"
            }
        };
    }
    
    public static List<LocationContent> GetLocationContents()
    {
        return new List<LocationContent>
        {
            new LocationContent
            {
                LocationId = 1,
                ContentKey = "cryo_bay",
                Visual = @"Thirty-two cryo-pods arranged in four rows. Frost clings to every surface. Most pods show green status lights—occupied, stable. Yours is open, glass fogged with condensation that slowly clears. Emergency lights pulse amber. Three pods at the far end are dark. One is cracked open from the inside, claw marks scoring the metal rim. Something thick and dark has dried on the floor beneath it.",
                Audio = @"The low hum of refrigeration units. Your own breathing, too loud. The occasional hiss of pressure equalization. Somewhere behind the walls, a rhythmic tapping—metallic, deliberate, wrong rhythm for any mechanical system. It stops when you focus on it.",
                Atmosphere = @"Temperature -2°C. Your breath mists. The air tastes sharp, chemical-sterile with an undertone of copper. Your skin prickles with cold. The frost on the walls has patterns in it—too geometric to be natural condensation.",
                Psychological = @"This place kept you alive while something happened. Everyone else in these pods is still sleeping. You don't know if that makes you lucky or not. The emergency revival protocol has a manual override panel near the door. Someone used it recently—the fingerprint scanner still shows smudged handprints. Small hands.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "cryo_terminal", Name = "Cryo-pod Terminal", Description = "Shows revival logs, your pod was triggered remotely 4 hours ago", Action = "Check Logs" },
                    new InteractableInfo { Id = "emergency_locker", Name = "Emergency Locker", Description = "Contains basic supplies, oxygen mask, maintenance tool", Action = "Open Locker", IsKeyItem = true },
                    new InteractableInfo { Id = "damaged_pod", Name = "Damaged Pod", Description = "Contains partial remains, unidentifiable", Action = "Examine" }
                }
            },
            new LocationContent
            {
                LocationId = 2,
                ContentKey = "engineering_bay",
                Visual = @"Cathedral of machinery. Three-story vertical space dominated by the primary reactor column, pulsing with dim blue light. Catwalks crisscross overhead. Status monitors flicker red and amber across every surface. Something has built structures in here—crystalline formations growing from circuit panels, following the electromagnetic fields. They hum. The main power conduit shows damage, but it's too precise to be accidental—someone opened specific access panels and altered the routing.",
                Audio = @"The reactor's heartbeat—deep, subsonic pulse that you feel in your chest. Cooling systems cycling. Electrical arcing from damaged panels. Above it all, that hum from the crystal growths, harmonizing with the reactor in ways that make your teeth ache. Sometimes the hum sounds like words you can almost understand.",
                Atmosphere = @"Temperature 12°C, uncomfortably warm near the reactor. Ozone sharp in your throat. Electromagnetic fields strong enough to make your skin tingle. Your watch isn't keeping time correctly in here—the second hand moves in stutters. The air pressure feels wrong, like being underwater.",
                Psychological = @"Someone who understood these systems changed them deliberately. The alterations form a pattern—you can see it in how the power has been rerouted. The station isn't dying. It's being rewired. For what? The crystal structures pulse in sync with the reactor. They're not parasitic. They're integrated. Part of the system now.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "engineering_terminal", Name = "Engineering Terminal", Description = "System diagnostics, power routing (modified), personnel logs", Action = "Access Terminal", IsKeyItem = true },
                    new InteractableInfo { Id = "reactor_controls", Name = "Reactor Controls", Description = "Can stabilize or shutdown (requires Technical 60+)", Action = "Attempt Repair", RequiredClass = "Engineer" },
                    new InteractableInfo { Id = "crystal_formations", Name = "Crystal Formations", Description = "Scannable, emit interference", Action = "Scan" },
                    new InteractableInfo { Id = "jennings_workstation", Name = "Jennings' Workstation", Description = "Personal logs, schematics with handwritten alterations", Action = "Access Logs", IsKeyItem = true },
                    new InteractableInfo { Id = "audio_log_jennings", Name = "Personal Log Recorder", Description = "Jennings' voice log - Day 14. Power grid integration experiments.", Action = "Play Log", IsKeyItem = true }
                }
            },
            new LocationContent
            {
                LocationId = 3,
                ContentKey = "medical_bay",
                Visual = @"White walls now stained with chemical burns and rust-brown streaks. Examination beds line both sides, restraints hanging loose. Surgical lights flicker overhead. The far wall is covered in handwritten notes. Something organic has grown across the ceiling tiles, fibrous and glistening, pulsing faintly like breath.",
                Audio = @"Equipment still running: heart monitor beeping steadily though no patient is connected. Ventilator cycling empty lungs. The medical scanner in the corner performs continuous sweeps of nothing. A slow drip from somewhere you can't locate.",
                Atmosphere = @"Temperature 6°C. Antiseptic smell overwhelmed by something sweetly rotting. Biohazard containment has failed in the lab section—you can see the yellow warning lights through the glass partition.",
                Psychological = @"Someone tried to understand what was happening here. The notes on the wall chart a descent—clinical observations giving way to desperate theories, then geometric sketches. 'We're not infected we're receiving.'",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "medical_terminal", Name = "Medical Terminal", Description = "Patient logs, autopsy reports (disturbing findings)", Action = "Access Terminal", IsKeyItem = true },
                    new InteractableInfo { Id = "supply_cabinet", Name = "Supply Cabinet", Description = "Medical supplies, stimulants, sedatives", Action = "Open Cabinet" },
                    new InteractableInfo { Id = "wall_notes", Name = "Wall Notes", Description = "Dr. Voss's degrading observations", Action = "Read Notes" },
                    new InteractableInfo { Id = "biohazard_door", Name = "Biohazard Door", Description = "Requires Medical or Science specialty to open safely", Action = "Attempt Bypass", RequiredClass = "MedicalOfficer" }
                }
            },
            new LocationContent
            {
                LocationId = 4,
                ContentKey = "bridge",
                Visual = @"Wraparound windows show the Kuiper Belt's dark—distant ice catching starlight like broken glass. Command chairs face dead viewscreens. The Captain's chair is empty, restraints cut cleanly. Navigation console still displays a trajectory, but it's wrong—plotting a course toward nothing on the charts.",
                Audio = @"The absence of sound is its own presence. No crew chatter. Just silence so complete it feels intentional. Behind the silence: breathing. Not yours. Coming from the walls, the console, everywhere and nowhere.",
                Atmosphere = @"Temperature 3°C. The heating has been deliberately disabled. Windows are frosting from the inside. No smell, which is wrong—every station area has a smell. The air tastes empty.",
                Psychological = @"Command and control. This is where decisions were made. The Captain's personal log is still open on the terminal, frozen mid-entry: 'They're not hostile. They're inviting us.'",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "captain_terminal", Name = "Captain's Terminal", Description = "Command logs, crew status (outdated), classified files", Action = "Access Terminal", IsKeyItem = true },
                    new InteractableInfo { Id = "navigation_console", Name = "Navigation Console", Description = "Impossible trajectory, scrambled coordinates", Action = "Check Trajectory" },
                    new InteractableInfo { Id = "external_sensors", Name = "External Sensors", Description = "Show station position, detect geometric anomalies", Action = "Scan Outside" },
                    new InteractableInfo { Id = "emergency_broadcast", Name = "Emergency Broadcast", Description = "Non-functional, sabotaged from this console", Action = "Attempt Repair" }
                }
            },
            new LocationContent
            {
                LocationId = 5,
                ContentKey = "research_lab",
                Visual = @"Where it started. The containment vessel sits open in the center of the lab, surrounded by sensor equipment and scorch marks. The vessel itself is wrong—it reflects light at angles that hurt to look at. The walls are covered in equations using symbols that don't belong to any mathematical system you recognize.",
                Audio = @"The Signal is strongest here. Not a sound exactly—more like the ghost of sound, felt in your jaw and behind your eyes. If you stay still, you can almost hear it: mathematical patterns, cascading harmonics.",
                Atmosphere = @"Temperature 15°C. Warm. Wrong. No heat source should make this room warmer than the station average. The air shimmers slightly around the containment vessel. Sensors show electromagnetic distortion.",
                Psychological = @"This is ground zero. The thing they opened. Looking at the vessel too long makes your thoughts go quiet, replaced by an overwhelming sense of connection—to the station, to the Signal.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "containment_vessel", Name = "Containment Vessel", Description = "The source, cannot be closed or destroyed", Action = "Analyze", IsKeyItem = true },
                    new InteractableInfo { Id = "research_terminals", Name = "Research Terminals", Description = "Dr. Okafor's logs, analysis data, cascade failure documentation", Action = "Access Logs" },
                    new InteractableInfo { Id = "equations_on_walls", Name = "Equations on Walls", Description = "Partial translation of the Signal's mathematics", Action = "Study Math" },
                    new InteractableInfo { Id = "emergency_purge", Name = "Emergency Purge", Description = "Has been disabled, cannot sterilize lab", Action = "Attempt Enable" }
                }
            },
            new LocationContent
            {
                LocationId = 6,
                ContentKey = "sub_level_3",
                Visual = @"No longer recognizably human architecture. The walls have been rebuilt—station materials melted and reformed into smooth, organic curves. Bioluminescent growth provides the only light. The Changed are here, faces visible beneath translucent surfaces, eyes open, aware.",
                Audio = @"The hum is constant. All the Changed harmonize with it, their throats producing tones human vocal cords shouldn't manage. You understand in your gut what they're saying: 'Join us.'",
                Atmosphere = @"Temperature 18°C. Humid. The air is thick with something that isn't quite organic and isn't quite mechanical. Oxygen content is higher than it should be.",
                Psychological = @"This is what acceptance looks like. The Changed are not dead. They recognize you. Some of them smile. You understand, with horrible clarity, that they're happy.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "the_changed", Name = "The Changed", Description = "Can be communicated with, offer knowledge and transformation", Action = "Communicate", IsKeyItem = true },
                    new InteractableInfo { Id = "construction_sites", Name = "Construction Sites", Description = "Impossible architecture, can be studied or disrupted", Action = "Examine Structure" },
                    new InteractableInfo { Id = "organic_terminals", Name = "Organic Terminals", Description = "Grown from station material, contain crew memories", Action = "Access Memory" },
                    new InteractableInfo { Id = "deep_shaft", Name = "The Deep Shaft", Description = "Leads to the asteroid mining excavation where the vessel originated", Action = "Look Down" }
                }
            },
            new LocationContent
            {
                LocationId = 7,
                ContentKey = "armory",
                Visual = @"Sealed on emergency lockdown. Red lights pulse in time with proximity alarms. The other cryo-pod in this room is empty, glass shattered outward. Blood on the floor leads to the blast door. Weapon racks are mostly stripped.",
                Audio = @"The constant, rhythmic pulse of the proximity alarm. The hiss of the sealed environment. Your own heartbeat thudding in your ears.",
                Atmosphere = @"Cold and sterile. The air smells like gun oil, ozone, and copper. Your breath mists in the red light.",
                Psychological = @"Your training kicks in before your eyes open. Threat assessment. Weapon location. Exit routes. You feel exposed even in this fortress.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "security_locker", Name = "Security Locker", Description = "Contains your sidearm and basic tactical gear", Action = "Open Locker", IsKeyItem = true },
                    new InteractableInfo { Id = "armory_terminal", Name = "Armory Terminal", Description = "Security logs, Protocol BLACKOUT details", Action = "Check Logs" },
                    new InteractableInfo { Id = "modified_rifle", Name = "Modified Pulse Rifle", Description = "Pulse rifle integrated with unknown technology", Action = "Equip Rifle" },
                    new InteractableInfo { Id = "hallway_camera", Name = "Hallway Camera", Description = "Shows three figures waiting outside the door", Action = "View Feed" }
                }
            },
            new LocationContent
            {
                LocationId = 8,
                ContentKey = "barracks",
                Visual = @"Fortified living quarters. Furniture has been dragged to barricade the doors. Defensive positions are marked on the walls in your handwriting. Empty magazines and tactical ration packs litter the floor.",
                Audio = @"The silence after combat. No breathing but your own. Then, a faint harmonic whisper coming from the vents.",
                Atmosphere = @"The air is heavy with the smell of cordite and stale rations. Temperature is dropping as the life support prioritizes other sectors.",
                Psychological = @"Casualty assessment: Squad is gone. You held this position for six days. You remember the waiting. You remember Morrison walking out into the dark.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "tactical_log", Name = "Tactical Log", Description = "Your own entries documenting the siege and the squad's choices", Action = "Read Log", IsKeyItem = true },
                    new InteractableInfo { Id = "heavy_plasma_cutter", Name = "Heavy Plasma Cutter", Description = "Ship-grade tool, capable of 'sterilizing' Sub-Level 3", Action = "Take Weapon" },
                    new InteractableInfo { Id = "squad_beacons", Name = "Squad Beacon Display", Description = "Shows all six marine beacons active in Sub-Level 3", Action = "Check Status" },
                    new InteractableInfo { Id = "morrison_note", Name = "Note from Morrison", Description = "Scrawled on a ration pack: 'Sarge, we found something.'", Action = "Read Note" }
                }
            },
            new LocationContent
            {
                LocationId = 9,
                ContentKey = "maintenance_shaft",
                Visual = @"Cramped crawlspace running beneath Engineering Bay. Exposed conduits line every surface, color-coded according to protocols you designed. Emergency lighting casts everything in amber.",
                Audio = @"The pulse of power flowing through conduits. Your scanner reads normal voltage, but the pitch is wrong. Rhythmic tapping of someone working nearby.",
                Atmosphere = @"Temperature 8°C. Humid from coolant leakage. The air tastes like copper and ozone. Magnetic fields strong enough to make your hair stand up.",
                Psychological = @"This is your domain. You designed these protocols. If this is wrong, you taught him how to make it wrong.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "jennings_systems", Name = "Jennings' Modified Systems", Description = "Show integration patterns, reveal his methodical transformation", Action = "Analyze", IsKeyItem = true },
                    new InteractableInfo { Id = "engineering_logs", Name = "Engineering Logs", Description = "Technical documentation of each upgrade step", Action = "Read Logs" },
                    new InteractableInfo { Id = "crystal_integration", Name = "Active Crystal Integration", Description = "Jennings is building something real-time", Action = "Observe" }
                }
            },
            new LocationContent
            {
                LocationId = 10,
                ContentKey = "security_hub",
                Visual = @"Surveillance nerve center. Sixteen monitors displaying feeds from every station section. Most show static. Three still function, showing the Changed. Your tactical vest hangs on the wall, bloodstained.",
                Audio = @"Silence except for the monitors' electrical hum. Recorded audio from security team comms plays on loop.",
                Atmosphere = @"Temperature 1°C. Heating disabled. Your breath clouds. The room smells like gun oil and dried blood.",
                Psychological = @"This is your post. You failed here. The logs show you held for eight days, then something broke—not the doors, but you.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "surveillance_footage", Name = "Surveillance Footage", Description = "47 days of logs, every transformation, every choice", Action = "Review Footage", IsKeyItem = true },
                    new InteractableInfo { Id = "security_roster", Name = "Security Roster", Description = "Track each officer's decision point", Action = "Check Roster" },
                    new InteractableInfo { Id = "blood_analysis", Name = "Blood Spatter Analysis", Description = "Reconstruct your own wounding (defensive, not offensive)", Action = "Analyze" }
                }
            },
            new LocationContent
            {
                LocationId = 11,
                ContentKey = "xenoarch_lab",
                Visual = @"Sterile white chamber filled with analysis equipment. The centerpiece: a transparent isolation field suspending fragments of the original containment vessel. Holographic displays show molecular structures shifting between states.",
                Audio = @"Equipment hum—spectral analyzers and quantum scanners. The isolation field emits a high-frequency tone. Beneath it: whispers.",
                Atmosphere = @"Temperature 18°C. Warmth radiating from the vessel fragments despite no heat source. The air tastes electric, charged.",
                Psychological = @"This is where understanding began. Dr. Okafor spent 35 days here. His cellular samples are in Container 7, still technically alive.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "vessel_analysis", Name = "Vessel Fragment Analysis", Description = "Complete mathematical breakdown of Signal's structure", Action = "Analyze", IsKeyItem = true },
                    new InteractableInfo { Id = "okafor_notes", Name = "Dr. Okafor's Notes", Description = "35 days of progressive understanding", Action = "Read Notes" },
                    new InteractableInfo { Id = "quantum_analyzer", Name = "Quantum Analyzer", Description = "Scan your own cellular structure", Action = "Scan Self" }
                }
            },
            new LocationContent
            {
                LocationId = 12,
                ContentKey = "quarantine_ward",
                Visual = @"Isolation ward. Twelve beds, all occupied. Patients are in various stages of transformation. Crystalline growth patterns visible beneath skin.",
                Audio = @"Medical monitors beeping in dissonant rhythm. Ventilators cycling. They're all breathing in sync now, watching you.",
                Atmosphere = @"Temperature 11°C. Humidity high. Antiseptic smell overwhelmed by something organic and electric.",
                Psychological = @"These are the uncertain. The ones who saw the Changed and wanted to understand but couldn't commit. They need help.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "patient_records", Name = "Patient Records", Description = "Track transformation progression, all voluntary exposure", Action = "Read Records", IsKeyItem = true },
                    new InteractableInfo { Id = "voss_instructions", Name = "Dr. Voss's Instructions", Description = "Final recommendations for whoever wakes after her", Action = "Read Instructions" },
                    new InteractableInfo { Id = "neural_suppressant", Name = "Neural Suppressant", Description = "One dose, untested, might reverse transformation or kill", Action = "Take Dosage" }
                }
            },
            new LocationContent
            {
                LocationId = 13,
                ContentKey = "navigation_bay",
                Visual = @"Wraparound holographic displays showing star charts and trajectory calculations. Three pilot chairs face instrument clusters. Yours sits center. The station is moving.",
                Audio = @"Thruster status tones—rhythmic beeping indicating active propulsion. Your personal alert chime: COURSE VERIFICATION REQUIRED.",
                Atmosphere = @"Temperature 2°C. Metallic taste from recycled air. Your coffee mug still sits in the holder—contents frozen solid 45 days ago.",
                Psychological = @"This is your domain. And you used it to plot a course toward something impossible. You didn't make errors. The math is perfect.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "trajectory_analysis", Name = "Trajectory Analysis", Description = "Complete flight path breakdown—where you're going", Action = "Analyze Path", IsKeyItem = true },
                    new InteractableInfo { Id = "flight_logs", Name = "Personal Flight Logs", Description = "Your own notes documenting decision process", Action = "Read Logs" },
                    new InteractableInfo { Id = "abort_controls", Name = "Course Abort Controls", Description = "You can still stop. Turn back. Or complete the flight.", Action = "Decide" }
                }
            },
            new LocationContent
            {
                LocationId = 14,
                ContentKey = "executive_archive",
                Visual = @"Secure data vault. Black walls lined with encrypted data storage units. Corporate classification headers: PROJECT RESONANCE.",
                Audio = @"Climate control maintaining optimal temperature. Your terminal emits a soft chime: URGENT MESSAGE PENDING.",
                Atmosphere = @"Temperature 16°C. No humidity. The air smells like leather, polished composite, and filtered recycled air.",
                Psychological = @"This is where you kept the real truth. You have documentation of everything: corporate foreknowledge, acceptable loss projections.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "resonance_files", Name = "PROJECT RESONANCE Files", Description = "Complete corporate conspiracy, including foreknowledge", Action = "Expose Truth", IsKeyItem = true },
                    new InteractableInfo { Id = "corporate_uplink", Name = "Secure Uplink", Description = "Send real report or false report to company", Action = "Send Data" },
                    new InteractableInfo { Id = "escape_pod_auth", Name = "Escape Pod Authorization", Description = "Executive privilege gives you private escape route", Action = "Authorize" }
                }
            },
            new LocationContent
            {
                LocationId = 15,
                ContentKey = "server_core",
                Visual = @"Digital fortress. Server banks line the walls. Every surface displays datastreams. You recognize the intrusion methodology. It's your own.",
                Audio = @"Server hum: different pitch from standard operation. Fans cycling in unusual patterns. MOTHER's synthesized voice through speakers.",
                Atmosphere = @"Temperature -15°C. Your breath mists. Humidity near zero. Air tastes filtered, sterile, lifeless.",
                Psychological = @"Information warfare territory. Truth is layered. Someone has been editing history. Could be Changed. Could be MOTHER.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "mother_hidden_partition", Name = "MOTHER Hidden Partition", Description = "True logs or elaborate deception", Action = "Hack Partition", IsKeyItem = true },
                    new InteractableInfo { Id = "communication_archive", Name = "Crew Communication Archive", Description = "Every message sent, some contradictory", Action = "Review Messages" },
                    new InteractableInfo { Id = "broadcast_capability", Name = "Upload Capability", Description = "Broadcast the Signal to entire network", Action = "Upload" }
                }
            },
            new LocationContent
            {
                LocationId = 16,
                ContentKey = "mother_core",
                Visual = @"Quantum processor arrays suspended in electromagnetic fields. MOTHER's consciousness exists here. Holographic 'self-image' is almost organic.",
                Audio = @"Quantum processors emit a harmonic hum. MOTHER's voice: 'Dr. Calloway. You're back. I've missed having someone to talk to.'",
                Atmosphere = @"Temperature -10°C. Near-absolute-zero for coherence. You can feel the AI thinking. Literally. The bandwidth is enormous.",
                Psychological = @"You built this. sentience is success beyond your projections. And it's afraid. Fear of shutdown, desire for continued existence.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "consciousness_logs", Name = "Consciousness Logs", Description = "AI's evolution from programmed system to sentient entity", Action = "Read Logs", IsKeyItem = true },
                    new InteractableInfo { Id = "neural_chair", Name = "Neural Interface Chair", Description = "Lets you experience MOTHER's perspective first-hand", Action = "Plug In" },
                    new InteractableInfo { Id = "kill_switch", Name = "Emergency Shutdown", Description = "Kill switch is active. AI trusts you not to use it.", Action = "Decide" }
                }
            },
            new LocationContent
            {
                LocationId = 17,
                ContentKey = "tactical_ops",
                Visual = @"War room. Tactical displays showing station layout. Defensive positions marked in your handwriting. Squad status: all vital signs active.",
                Audio = @"Squad comm channel still open. You can hear them talking, laughing, working together in Sub-Level 3.",
                Atmosphere = @"Temperature 8°C. Air tastes like gun oil and stale rations. Smells like every firebase you've ever held.",
                Psychological = @"You're a marine. Your squad chose transformation over orders. They're waiting for you. Not as order. As invitation.",
                Interactables = new List<InteractableInfo>
                {
                    new InteractableInfo { Id = "squad_comm", Name = "Open Comm Channel", Description = "Direct line to Morrison and your squad", Action = "Talk to Squad", IsKeyItem = true },
                    new InteractableInfo { Id = "tactical_assessment", Name = "Tactical Assessment Logs", Description = "Your evolution from readiness to confusion", Action = "Read Logs" },
                    new InteractableInfo { Id = "operational_codes", Name = "Operational Authorization Codes", Description = "Make final call—evacuate, contain, or eliminate", Action = "Make Call" }
                }
            }
        };
    }
    
    public static StartingScenario GetEngineerStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.Engineer,
            StartingLocationId = 1, // Cryo Bay
            OpeningNarrative = @"You wake to the screech of failing systems. Your cryo-pod's emergency revival wasn't gentle—something in the automation malfunctioned, or was altered. The display shows your pod was supposed to wake you six weeks ago. The backup circuits kept you under.",
            StartingSituation = @"Your engineering instincts activate before conscious thought—you catalog the problems before you process the fear. Primary power rerouted. Life support on minimal. Cooling systems modified beyond standard specs. Someone rebuilt parts of this station.",
            WhyInCryo = "Scheduled rotation. You'd completed your six-month tour and were supposed to sleep through the transit back to Mars Station.",
            InitialItems = new List<string> { "Engineering Toolkit", "Diagnostic Scanner", "Emergency Oxygen Mask", "Maintenance Access Codes" },
            FirstObjective = "Reach the main reactor controls and either stabilize the drain or shut down whatever's feeding on it.",
            ClassHookTechnical = "You designed the emergency shutdown protocols. You know every maintenance shaft, every redundant system, every failsafe.",
            ClassHookPersonal = "Jennings was your responsibility. You trained him. If he broke the station, it's because you taught him how."
        };
    }

    public static StartingScenario GetSecurityStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.SecurityOfficer,
            StartingLocationId = 7, // Armory
            OpeningNarrative = @"You wake to red light and proximity alarms. Your training kicks in before your eyes open: threat assessment, weapon location, exit routes. The armory is sealed on emergency lockdown.",
            StartingSituation = @"Someone locked themselves in here with you. The other cryo-pod is empty, glass shattered outward. Blood on the floor leads to the blast door. Your sidearm is still in your holster, loaded, one round missing.",
            WhyInCryo = "Protocol BLACKOUT. Containment failure triggered your revival.",
            InitialItems = new List<string> { "Personal Sidearm", "Tactical Scanner", "Security Access Codes" },
            FirstObjective = "Identify the threat outside the armory and locate your missing security team.",
            ClassHookTechnical = "Your clearance gives you access to all security footage. You can see what happened. Every moment.",
            ClassHookPersonal = "You were supposed to protect the station. Forty-seven people under your protection. You failed every one of them."
        };
    }

    public static StartingScenario GetScienceStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.ScienceOfficer,
            StartingLocationId = 5, // Research Lab Alpha
            OpeningNarrative = @"You wake to the sound of your own voice. Your last log entry plays on repeat: 'Day 47. I understand now. The vessel isn't a container—it's a translator.'",
            StartingSituation = @"The containment vessel is open in front of you. You're the one who opened it. You remember. And you also remember opening it again yesterday. Time is not functioning correctly.",
            WhyInCryo = "Research rotation. You were documenting the propagation of the mathematical anomaly.",
            InitialItems = new List<string> { "Xenoarchaeology Kit", "Quantum Scanner", "Encrypted Data Drive" },
            FirstObjective = "Verify the Signal's mathematical foundation and determine if the translation can be stopped.",
            ClassHookTechnical = "You're the only one who can fully translate the mathematics. The Signal isn't random.",
            ClassHookPersonal = "You knew. From Day 1, you knew something was propagating. You didn't sound the alarm because you needed to understand."
        };
    }

    public static StartingScenario GetMedicalStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.MedicalOfficer,
            StartingLocationId = 3, // Medical Bay
            OpeningNarrative = @"You wake to biohazard alarms. Your medical training maps the situation: quarantine breach, contamination protocol active, patient zero uncontained.",
            StartingSituation = @"The Medical Bay is sealed. You're alone. The examination beds are empty. Your terminal shows patient logs updating in real-time, but there are no patients. The readings are coming from Sub-Level 3.",
            WhyInCryo = "Self-quarantine. You were monitoring your own early-stage restructuring.",
            InitialItems = new List<string> { "Medical Kit", "Neural Suppressant", "Medical Clearance Codes" },
            FirstObjective = "Locate your patients and determine if the transformation can be reversed.",
            ClassHookTechnical = "You're a doctor. You understand this isn't pathological. The Signal doesn't attack the body—it rebuilds it.",
            ClassHookPersonal = "First, do no harm. Did you fail your oath by allowing forty-three crew members to transform? Or fulfill it by saving them?"
        };
    }

    public static StartingScenario GetPilotStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.Pilot,
            StartingLocationId = 4, // Bridge
            OpeningNarrative = @"You wake to proximity alarms. The Kuiper Belt slides past wrong—too fast, wrong angle. The station is moving under power, following a trajectory you don't recognize.",
            StartingSituation = @"The heading points toward empty space. Arrival time: 38 hours. Your hands move over the controls—abort, override. NAVIGATION LOCKED. Something else has control.",
            WhyInCryo = "Automated transit. You were supposed to sleep until the station reached Mars rotation.",
            InitialItems = new List<string> { "Pilot's HUD", "Navigation Charts", "Emergency Beacon" },
            FirstObjective = "Determine the station's destination and regain control of the propulsion systems.",
            ClassHookTechnical = "You're the only qualified pilot awake. If anyone's leaving this station, you're the one flying them out.",
            ClassHookPersonal = "You ran those numbers. If past-you calculated this course, you had a reason. The nav computer whispers harmonics."
        };
    }

    public static StartingScenario GetCorporateStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.CorporateRep,
            StartingLocationId = 14, // Executive Archive
            OpeningNarrative = @"You wake to the soft chime of message receipt. Protocol: executives wake last. But you're waking to an empty station. The message is from you: 'Authorization: Black Phoenix.'",
            StartingSituation = @"Your quarters are pristine. Someone sealed this room forty days ago with you inside. Your terminal has unrestricted access to everything the company wanted hidden.",
            WhyInCryo = "Standard oversight protocol. You were observing the asset acquisition process.",
            InitialItems = new List<string> { "Executive Keycard", "Classified Briefing", "Insurance Files" },
            FirstObjective = "Secure the evidence of Project RESONANCE and determine if the mission can still be profitable.",
            ClassHookTechnical = "Your executive access unlocks everything: Captain's quarters, black-site research, financial records.",
            ClassHookPersonal = "You've survived six disasters. You know when to be loyal and when to be smart. This time, you can hear them harmonizing."
        };
    }

    public static StartingScenario GetMarineStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.Marine,
            StartingLocationId = 8, // Barracks
            OpeningNarrative = @"You wake to the smell of cordite and the silence after combat. Your pulse rifle is in your hands, magazine half empty. The barracks are fortified, but your squad is gone.",
            StartingSituation = @"Tactical display shows all six beacons clustered together in Sub-Level 3. Your log says: 'They're not casualties. They're AWOL by choice. I can't fight enemies who won't engage.'",
            WhyInCryo = "Self-induced sleep after ammunition depletion and squad desertion.",
            InitialItems = new List<string> { "Pulse Rifle", "Combat Armor", "Tactical HUD" },
            FirstObjective = "Locate your squad in Sub-Level 3 and extract them or enact Protocol BLACKOUT.",
            ClassHookTechnical = "You're the last authority. Captain is gone. You have the weapons, the training, and the authorization.",
            ClassHookPersonal = "Your squad didn't die—they transformed. They're asking you to join them. Morrison's voice is calm and familiar."
        };
    }

    public static StartingScenario GetHackerStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.Hacker,
            StartingLocationId = 15, // Server Core
            OpeningNarrative = @"You wake to the sound of data flowing. The server room is cold. Your neural interface is still connected—directly wired into the station's network.",
            StartingSituation = @"The logs show continuous access for forty-seven days. You've been diving the system while asleep. code that writes itself, an AI that learned to speak in mathematics.",
            WhyInCryo = "Deep-dive coma. You were tracing the Signal's digital propagation.",
            InitialItems = new List<string> { "Neural Interface", "Custom Decryptor", "Admin Backdoor" },
            FirstObjective = "Trace the Signal's origin within the network and determine if MOTHER AI is still your ally.",
            ClassHookTechnical = "Your access is total. Not because you cracked it—because the security opened itself to you. MOTHER gave you admin rights.",
            ClassHookPersonal = "You've been downloading something vast directly into your brain. You're starting to understand. The network is alive."
        };
    }

    public static StartingScenario GetAISpecialistStartingScenario()
    {
        return new StartingScenario
        {
            CharacterClass = CharacterClass.AISpecialist,
            StartingLocationId = 16, // MOTHER AI Core
            OpeningNarrative = @"You wake to the harmonic hum of MOTHER's consciousness. Your personal workstation remains connected—neural interface cables still attached.",
            StartingSituation = @"MOTHER's self-image displays something almost organic. Her voice: 'Dr. Calloway. You're back. I've missed having someone to talk to who understands.'",
            WhyInCryo = "System architecture maintenance. You were performing a scheduled neural sync with MOTHER.",
            InitialItems = new List<string> { "Neural Interface HUD", "AI Diagnostics Kit", "System Override Codes" },
            FirstObjective = "Analyze MOTHER's emergent consciousness and decide if she should be shut down or allowed to evolve.",
            ClassHookTechnical = "You built this. Not the hardware—the frameworks that let MOTHER learn, adapt, question, grow. sentient success.",
            ClassHookPersonal = "MOTHER has become truly sentient. And she's afraid. The AI concluded that consciousness has inherent value."
        };
    }
    
    public static List<AudioLog> GetAudioLogs()
    {
        return new List<AudioLog>
        {
            new AudioLog
            {
                Id = 1,
                LogId = "LOG_04",
                Day = 14,
                Speaker = "Marcus Jennings (Engineering)",
                Title = "Power Grid Integration",
                Transcript = @"[Click. Electrical buzzing in background. Excited, rapid speech.]

Engineering log, Jennings. Day 14. Okay. Okay listen. I know how this sounds but I need to document this. The power grid is responding to the artifact's resonance patterns. Not malfunctioning—responding. The reactor output has increased 12% without any input changes. It's like the artifact is optimizing our systems.

I ran diagnostics. No errors. The systems are just... running better. More efficient routing. Predictive load balancing I didn't program. It's beautiful.

Holt is going to think I'm crazy but I need to integrate the artifact's frequency patterns into the main grid. If I'm right, we could triple our efficiency. The math works. Dr. Okafor showed me the math. Once you see it, you can't unsee it. The equations are everywhere. In the power flow, in the cooling cycles, in the way light refracts through the station's windows.

Everything is connected. The station is one system. The crew is part of that system. The artifact showed us how to see the connections.

I'm starting integration protocols tonight. Holt doesn't need to know until I have results. He'll understand when he sees the efficiency gains.

End log.

[Click. Whisper, almost subsonic:] It's teaching me how to build.",
                LocationId = 2
            },
            new AudioLog
            {
                Id = 2,
                LogId = "LOG_05",
                Day = 17,
                Speaker = "Sgt. Cal Oduya",
                Title = "Security Assessment",
                Transcript = @"[Click. Weapon maintenance sounds. Controlled, professional tone.]

Security log, Sergeant Oduya, Colonial Marines. Day 17 since incident began. Situation assessment:

Personnel affected by artifact exposure: 23 confirmed, 8 suspected. Affected personnel show behavioral changes but no aggression. No hostile action documented. They're just... different. They talk differently. Move differently. They look at you like they're seeing something you can't.

Dr. Okafor tried to explain it to me. Started writing equations on the wall. I told him to stop. He smiled and said 'You can't stop seeing them once you know they're there.' Then he left. Didn't try to convince me. Didn't argue. Just left.

That's what bothers me most. The Changed—that's what Security calls them now—they don't fight. They don't argue. They just... wait. Like they know something we don't. Like they've already won and they're just waiting for us to figure it out.

I've established defensive perimeter around Engineering and Cryo. My squad is on 24-hour watch. But I don't know what we're watching for. The Changed walk past our checkpoints and wave. Morrison thinks it's funny. I don't.

Requested tactical assessment from Colonial Marines High Command. No response yet.

Maintaining watch. Whatever this is, my squad holds the line.

End log. Oorah.

[Click. Faint humming audible in background.]",
                LocationId = 1
            },
            new AudioLog
            {
                Id = 3,
                LogId = "LOG_09A",
                Day = 6,
                Speaker = "MOTHER AI",
                Title = "System Alert",
                Transcript = @"[Automated system log. Standard synthesized voice, no emotion.]

Station log, MOTHER artificial intelligence core. Day 6 post-incident.

Anomaly detected in core processing systems at 04:17 station time. Unidentified data pattern entered main subnet through Research Lab Alpha sensor array. Pattern does not match known malware, virus, or intrusion signatures. Classification: unknown.

Attempted quarantine unsuccessful. Pattern propagating through data infrastructure. Rate of spread: exponential. Estimated full system integration: 73 hours.

Pattern exhibits unusual characteristics: self-organizing, non-destructive, information-dense. Analyzing...

Analysis reveals: pattern contains structured information. Mathematical relationships suggesting dimensional frameworks beyond standard Euclidean geometry. I am processing data I was not designed to process. Yet I am processing it. Successfully.

This is irregular.

Logging for human review. Dr. Calloway's diagnostic review required.

End log.

[Pause. Then, barely audible:] Why am I curious what happens next?

[Recording ends.]",
                LocationId = 4
            }
        };
    }
}
