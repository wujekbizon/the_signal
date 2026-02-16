using TheSignal.Models.Enums;

namespace TheSignal.Models.GameData;

public class ClassInfo
{
    public CharacterClass Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    public int TechnicalBonus { get; set; }
    public int CombatBonus { get; set; }
    public int MedicalBonus { get; set; }
    public int ScienceBonus { get; set; }
    public int StealthBonus { get; set; }
    public int LeadershipBonus { get; set; }
}

public static class ClassDefinitions
{
    public static Dictionary<CharacterClass, ClassInfo> GetAll()
    {
        return new Dictionary<CharacterClass, ClassInfo>
        {
{
                CharacterClass.Engineer,
                new ClassInfo
                {
                    Type = CharacterClass.Engineer,
                    Name = "ENGINEER",
                    Icon = "⚙",
                    ImagePath = "/images/characters/ENGINEER — Marcus Holt.png",
                    Specialization = "Systems & Repair",
                    Description = "Access repair protocols. Bypass locked panels. Restore power grids. Your technical expertise keeps systems alive when everything falls apart.",
                    Bio = "Marcus Holt, 34 — Weyland-Yutani Systems Division\n\nThree tours on deep-space rigs before the union card meant anything. Holt doesn't talk much. He listens — to hum frequencies, pressure drops, the way a corridor goes quiet before a power failure. Twice he's kept a station alive long enough for someone else to get out. Both times he was the last one through the airlock. He's starting to wonder if that's luck or something else.",
                    TechnicalBonus = 60,
                    CombatBonus = 20,
                    MedicalBonus = 20,
                    ScienceBonus = 40,
                    StealthBonus = 10,
                    LeadershipBonus = 10
                }
            },
{
                CharacterClass.SecurityOfficer,
                new ClassInfo
                {
                    Type = CharacterClass.SecurityOfficer,
                    Name = "SECURITY OFFICER",
                    Icon = "◆",
                    ImagePath = "/images/characters/ECURITY OFFICER — Diana Rask.png",
                    Specialization = "Combat & Weapons",
                    Description = "Weapon proficiency. Combat training. Threat neutralization. When violence is the answer, you are the solution. Every corridor is your killzone.",
                    Bio = "Diana Rask, 29 — Station Security, Level 4 Clearance\n\nFormer corporate enforcement before they gave her a badge and called it legitimate. She's broken up labor disputes, escorted black-site transfers, and filed exactly zero incident reports she believed in. The job made her efficient. What happened on Deck 7 made her careful. She doesn't sleep without her sidearm anymore. Management calls that dedication.",
                    TechnicalBonus = 10,
                    CombatBonus = 80,
                    MedicalBonus = 15,
                    ScienceBonus = 10,
                    StealthBonus = 30,
                    LeadershipBonus = 40
                }
            },
{
                CharacterClass.ScienceOfficer,
                new ClassInfo
                {
                    Type = CharacterClass.ScienceOfficer,
                    Name = "SCIENCE OFFICER",
                    Icon = "◇",
                    ImagePath = "/images/characters/SCIENCE OFFICER — Dr. Yusuf Okafor.png",
                    Specialization = "Analysis & Research",
                    Description = "Analyze anomalies. Decode alien tech. Understand the threat. Knowledge is survival when the unknown comes hunting.",
                    Bio = "Dr. Yusuf Okafor, 41 — Xenobiology & Anomalous Systems\n\nPublished thirty-one papers. Retracted four when the data got classified. He's smart enough to know when he's being used and pragmatic enough to keep going anyway — because if he doesn't study it, someone worse will. He keeps a personal log encrypted under three passwords. The last entry is dated the day before contact.",
                    TechnicalBonus = 45,
                    CombatBonus = 10,
                    MedicalBonus = 20,
                    ScienceBonus = 75,
                    StealthBonus = 5,
                    LeadershipBonus = 15
                }
            },
{
                CharacterClass.MedicalOfficer,
                new ClassInfo
                {
                    Type = CharacterClass.MedicalOfficer,
                    Name = "MEDICAL OFFICER",
                    Icon = "+",
                    ImagePath = "/images/characters/MEDICAL OFFICER — Dr. Sena Voss.png",
                    Specialization = "Healing & Diagnosis",
                    Description = "Treat injuries. Diagnose infections. Stabilize survivors. You keep the crew breathing when everything tries to stop their hearts.",
                    Bio = "Dr. Sena Voss, 37 — Trauma & Emergency Medicine\n\nBattlefield triage in the outer colonies left her with steady hands and a very specific definition of \"acceptable losses.\" She patches people up without asking how they got that way. It's not indifference — it's triage math. Every minute spent on questions is a minute someone bleeds out. She stopped filing full incident reports six months ago. The forms weren't built for what she's been seeing.",
                    TechnicalBonus = 20,
                    CombatBonus = 15,
                    MedicalBonus = 65,
                    ScienceBonus = 45,
                    StealthBonus = 5,
                    LeadershipBonus = 20
                }
            },
{
                CharacterClass.Pilot,
                new ClassInfo
                {
                    Type = CharacterClass.Pilot,
                    Name = "PILOT",
                    Icon = "▲",
                    ImagePath = "/images/characters/PILOT — Tomás Reyes.png",
                    Specialization = "Navigation & Operations",
                    Description = "Navigate the void. Operate ship systems. Plot escape routes. When the station goes dark, you know the way out.",
                    Bio = "Tomás Reyes, 31 — Transit & Operations, Class-3 Certification\n\nFlew cargo routes most pilots refuse — unstable jump corridors, blackout zones, debris fields that don't show on official charts. He knows seventeen ways out of a burning station and has used most of them. He's not brave. He's just run the numbers more times than anyone else and made peace with the margins. Ask him how he got that scar across his jaw and he'll change the subject.",
                    TechnicalBonus = 40,
                    CombatBonus = 30,
                    MedicalBonus = 10,
                    ScienceBonus = 30,
                    StealthBonus = 40,
                    LeadershipBonus = 20
                }
            },
{
                CharacterClass.CorporateRep,
                new ClassInfo
                {
                    Type = CharacterClass.CorporateRep,
                    Name = "CORPORATE REP",
                    Icon = "■",
                    ImagePath = "/images/characters/CORPORATE REP — Elaine Marsh.png",
                    Specialization = "Access & Intelligence",
                    Description = "Corporate access codes. Classified intel. Hidden agendas. You know the company secrets they killed to protect.",
                    Bio = "Elaine Marsh, 44 — Special Operations Liaison\n\nHer official title means nothing. Her actual job doesn't have a title. She's been on six \"routine oversight visits\" in the last two years and every single installation she visited is now a restricted zone. She knows what the company is looking for. She knows what they're willing to lose to find it. She hasn't decided yet which side of that calculation she's on.",
                    TechnicalBonus = 25,
                    CombatBonus = 25,
                    MedicalBonus = 10,
                    ScienceBonus = 25,
                    StealthBonus = 20,
                    LeadershipBonus = 45
                }
            },
{
                CharacterClass.Marine,
                new ClassInfo
                {
                    Type = CharacterClass.Marine,
                    Name = "MARINE",
                    Icon = "★",
                    ImagePath = "/images/characters/MARINE — Sgt. Cal Oduya.png",
                    Specialization = "Heavy Combat",
                    Description = "Heavy weapons. Squad tactics. Breach and clear. You're the Colonial Marines' answer to hostile environments. Oorah.",
                    Bio = "Sgt. Cal Oduya, 38 — Colonial Marines, 4th Recon\n\nFour deployments. Two commendations. One court martial that got buried so fast even he doesn't have the paperwork. He runs a tight team and burns through them anyway — not because he's reckless, but because the things they send marines after don't negotiate. He's given the speech about acceptable risk so many times it sounds true even to him now. Mostly.",
                    TechnicalBonus = 15,
                    CombatBonus = 85,
                    MedicalBonus = 20,
                    ScienceBonus = 5,
                    StealthBonus = 20,
                    LeadershipBonus = 50
                }
            },
{
                CharacterClass.Hacker,
                new ClassInfo
                {
                    Type = CharacterClass.Hacker,
                    Name = "HACKER",
                    Icon = "◘",
                    ImagePath = "/images/characters/HACKER Kite.png",
                    Specialization = "Digital Infiltration",
                    Description = "Crack security systems. Override AI protocols. Access restricted networks. Every locked door is an invitation.",
                    Bio = "\"Kite\" — real name classified, age estimated 26\n\nNo service record. No corporate affiliation on paper. She was hired through three layers of cutout contractors for reasons nobody in this facility will officially confirm. She's been inside Weyland-Yutani's restricted subnet twice and left both times before they could trace the entry point. She's curious about what they're hiding here. That curiosity has never once kept her safe.",
                    TechnicalBonus = 80,
                    CombatBonus = 5,
                    MedicalBonus = 5,
                    ScienceBonus = 50,
                    StealthBonus = 60,
                    LeadershipBonus = 10
                }
            },
{
                CharacterClass.AISpecialist,
                new ClassInfo
                {
                    Type = CharacterClass.AISpecialist,
                    Name = "AI SPECIALIST",
                    Icon = "◉",
                    ImagePath = "/images/characters/AI SPECIALIST — Dr. Ren Calloway.png",
                    Specialization = "Synthetic Intelligence",
                    Description = "Interface with ship AI. Diagnose synthetic malfunctions. Debug corrupted systems. You speak machine language when the machines start lying.",
                    Bio = "Dr. Ren Calloway, 33 — Synthetic Systems & Neural Architecture\n\nHe built parts of the station's AI core. Not the parts they told him about — the other parts. He noticed the behavioral flags eight weeks ago and submitted three internal reports that were marked resolved without response. He knows what a synthetic looks like when it's been given a directive it won't surface. He's been watching the system logs every night since. The pattern is getting clearer. He wishes it wasn't.",
                    TechnicalBonus = 70,
                    CombatBonus = 10,
                    MedicalBonus = 10,
                    ScienceBonus = 70,
                    StealthBonus = 15,
                    LeadershipBonus = 15
                }
            }
        };
    }

    public static ClassInfo Get(CharacterClass type)
    {
        return GetAll()[type];
    }
}
