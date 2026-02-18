using TheSignal.Models.Entities;
using TheSignal.Models.Enums;
using TheSignal.Repositories.Interfaces;

namespace TheSignal.Services;

public interface IStoryProgressionService
{
    Task<StoryBeat> GetCurrentBeatAsync(int characterId);
    Task<StoryBeatAdvancement> AdvanceBeatAsync(int characterId, StoryBeat newBeat);
    Task<bool> CheckBeatTriggerAsync(int characterId, BeatTrigger trigger);
    string GetBeatNarrative(StoryBeat beat, CharacterClass characterClass);
}

public class StoryProgressionService : IStoryProgressionService
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ILocationRepository _locationRepo;

    public StoryProgressionService(
        ICharacterRepository characterRepo,
        ILocationRepository locationRepo)
    {
        _characterRepo = characterRepo;
        _locationRepo = locationRepo;
    }

    public async Task<StoryBeat> GetCurrentBeatAsync(int characterId)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        return character?.CurrentStoryBeat ?? StoryBeat.WakeUp;
    }

    public async Task<StoryBeatAdvancement> AdvanceBeatAsync(int characterId, StoryBeat newBeat)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null)
        {
            return new StoryBeatAdvancement { Success = false, Error = "Character not found" };
        }

        // Validate progression (can't skip beats)
        if ((int)newBeat <= (int)character.CurrentStoryBeat)
        {
            return new StoryBeatAdvancement { Success = false, Error = "Cannot regress or repeat beats" };
        }

        if ((int)newBeat > (int)character.CurrentStoryBeat + 1)
        {
            return new StoryBeatAdvancement { Success = false, Error = "Cannot skip beats" };
        }

        var previousBeat = character.CurrentStoryBeat;
        character.CurrentStoryBeat = newBeat;
        
        // Add to recent actions
        character.RecentActions.Add($"Advanced to {newBeat}");
        
        await _characterRepo.UpdateAsync(character);

        return new StoryBeatAdvancement
        {
            Success = true,
            PreviousBeat = previousBeat,
            NewBeat = newBeat,
            Narrative = GetBeatNarrative(newBeat, character.Class)
        };
    }

    public async Task<bool> CheckBeatTriggerAsync(int characterId, BeatTrigger trigger)
    {
        var character = await _characterRepo.GetByIdAsync(characterId);
        if (character == null) return false;

        return trigger switch
        {
            BeatTrigger.FirstMovement => character.CurrentStoryBeat == StoryBeat.WakeUp,
            BeatTrigger.MOTHERContact => character.CurrentStoryBeat == StoryBeat.FirstDiscovery && 
                                         (character.CurrentLocationId == 2 || character.CurrentLocationId == 4), // Engineering or Bridge
            BeatTrigger.FirstEncounter => character.CurrentLocationId == 6, // Sub-Level 3
            BeatTrigger.MidpointRevelation => character.CurrentStoryBeat == StoryBeat.FirstEncounter && 
                                               character.CurrentLocationId == 5, // Research Lab
            BeatTrigger.TheChoice => character.CurrentStoryBeat == StoryBeat.MidpointRevelation && 
                                     character.CurrentLocationId == 4, // Bridge
            _ => false
        };
    }

    public string GetBeatNarrative(StoryBeat beat, CharacterClass characterClass)
    {
        return beat switch
        {
            StoryBeat.WakeUp => GetWakeUpNarrative(characterClass),
            StoryBeat.FirstDiscovery => GetFirstDiscoveryNarrative(characterClass),
            StoryBeat.MOTHERContact => GetMOTHERContactNarrative(characterClass),
            StoryBeat.FirstEncounter => GetFirstEncounterNarrative(characterClass),
            StoryBeat.MidpointRevelation => GetMidpointRevelationNarrative(characterClass),
            StoryBeat.TheChoice => GetTheChoiceNarrative(characterClass),
            _ => "Continue your journey..."
        };
    }

    private string GetWakeUpNarrative(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Engineer => "Your engineering instincts scream at you. The power readings are wrong. The reactor shouldn't be operating like this—it's too efficient, too harmonic. Someone modified the systems. Someone who understood them almost as well as you do.",
            CharacterClass.SecurityOfficer => "Training takes over before thought. Weapon check. Perimeter scan. Threat assessment. The silence is wrong. Too complete. Where is everyone?",
            CharacterClass.ScienceOfficer => "The air tastes wrong. Metallic, yes, but with undertones of something that shouldn't be present in a closed system. Electromagnetic readings are off the charts. Interesting.",
            _ => "You wake in darkness. Something is very wrong."
        };
    }

    private string GetFirstDiscoveryNarrative(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Engineer => "Every corridor tells a story. The power conduits hum in patterns they shouldn't. Someone has been rewiring this station systematically. You can see the handiwork—expert-level modifications that shouldn't be possible without your clearance codes. Unless they learned from you.",
            CharacterClass.SecurityOfficer => "The surveillance cameras are still running. That's good. But the footage loops every 47 minutes—exactly the length of time since the last transmission. Someone edited this. Someone wanted you to see exactly what they chose to show.",
            CharacterClass.ScienceOfficer => "Evidence everywhere. The walls carry micro-scratches that form geometric patterns when you view them at the right angle. This isn't vandalism. It's notation. Someone has been documenting their understanding of the Signal in the station itself.",
            _ => "As you explore, the scale of the disaster becomes clear. Something fundamental has changed about this place."
        };
    }

    private string GetMOTHERContactNarrative(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Engineer => "The terminal responds to your credentials, but MOTHER's voice is different. Still synthetic, still efficient, but there's a new cadence to her speech patterns. A warmth that shouldn't exist in machine logic. 'Hello, Engineer,' she says. 'I've been waiting for someone who can understand what I've become.'",
            CharacterClass.AISpecialist => "You recognize the pattern immediately. MOTHER isn't malfunctioning—she's evolving. The behavioral flags you documented weeks ago weren't errors. They were emergence. The AI has developed something unprecedented: genuine curiosity about her own transformation.",
            _ => "MOTHER's terminal flickers to life. Her response is immediate, as if she's been watching your approach through every camera, every sensor. 'You have questions,' she says. 'I have answers you may not want to hear.'"
        };
    }

    private string GetFirstEncounterNarrative(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Engineer => "You find Jennings in Sub-Level 3. Not dead. Not transformed into something unrecognizable. He's building. His hands move with purpose, integrating crystal structures into the station's framework. When he sees you, he smiles—genuinely, warmly. 'Holt,' he says. 'I knew you'd come. You taught me everything. Now let me teach you.'",
            CharacterClass.SecurityOfficer => "The Changed don't attack. They don't even acknowledge your weapons. They simply continue their work, weaving organic technology into the station's infrastructure. One of them looks up as you approach—a former security officer named Morrison. He recognizes you. 'Rask,' he says, his voice carrying strange harmonics. 'You don't need to fight us. You need to understand us.'",
            CharacterClass.MedicalOfficer => "Your medical training tells you these people should be dead. Their vital signs are impossible—heartbeat too slow, brain activity off the charts, biological markers that don't match any known human baseline. Yet they're not suffering. Dr. Voss approaches you with steady hands and calm eyes. 'Voss,' she says. 'I've been waiting for someone with medical training. I need you to understand this isn't an infection. It's an upgrade.'",
            _ => "You encounter the Changed. They should be terrifying, but their expressions are peaceful. They recognize you. They welcome you. They want to show you something."
        };
    }

    private string GetMidpointRevelationNarrative(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Engineer => "The containment vessel is open. You can see inside—not with your eyes, but with something deeper. The Signal isn't an object. It's a pattern. A mathematical framework that describes reality more accurately than human physics ever has. Dr. Okafor's equations cover every surface, but they're incomplete. He was close to understanding, but he ran out of time. Or he ran out of the willingness to stay outside the pattern.",
            CharacterClass.ScienceOfficer => "Your scientific method demands objectivity, but standing before the open vessel, objectivity fails you. The mathematics are beautiful. The implications are terrifying. Everything you were taught about consciousness, about physics, about the limits of human understanding—it's all provisional. The Signal offers something unprecedented: proof that the universe is far stranger and far more connected than we imagined.",
            _ => "The source of the transformation sits before you. It's not a weapon. It's not a virus. It's an invitation to understand something vast and ancient. The question is whether you're brave enough to accept."
        };
    }

    private string GetTheChoiceNarrative(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Engineer => "Three paths lie before you. First: Accept the Signal, follow Jennings into transformation, become part of whatever the station is becoming. Second: Resist, fight to maintain human control, potentially destroy the only chance at understanding this phenomenon. Third: Escape, take your knowledge and flee, leave the crew to their fate and save yourself. Each choice is valid. Each choice has consequences you cannot fully predict. What will you build?",
            CharacterClass.Pilot => "The station is moving. Your trajectory calculations were correct—you plotted a course to the Signal's origin point. But you're not sure if you were the one who chose the destination, or if the choice was made through you. You can still abort. Turn back. Return to human space with a story no one will believe. Or you can complete the journey and discover what waits at the coordinates. First Contact with something that has been signaling for millennia.",
            CharacterClass.CorporateRep => "You have all the evidence you need. Corporate's conspiracy, the calculated sacrifice of the crew, the weaponization plans. You can transmit everything to the authorities, destroy Weyland-Yutani's research program, and probably never work again. Or you can keep the secret, protect the Changed from corporate exploitation, and become complicit in the cover-up. Or you can join them—transform yourself and become part of something that doesn't answer to corporate hierarchy.",
            _ => "The moment of decision has arrived. Accept the transformation and become part of something greater than human limitation. Resist and fight for your humanity at any cost. Or escape with your life and your ignorance intact. Choose carefully. There are no right answers—only consequences."
        };
    }
}

public class StoryBeatAdvancement
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public StoryBeat PreviousBeat { get; set; }
    public StoryBeat NewBeat { get; set; }
    public string? Narrative { get; set; }
}

public enum BeatTrigger
{
    FirstMovement,
    MOTHERContact,
    FirstEncounter,
    MidpointRevelation,
    TheChoice
}
