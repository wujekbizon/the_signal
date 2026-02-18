using TheSignal.Models.Enums;
using TheSignal.Models.GameData;

namespace TheSignal.Services.Interfaces;

public interface INarrativeService
{
    Task<string> GenerateNarrativeAsync(
        int characterId,
        int locationId,
        TimeOfDay timeOfDay,
        AtmosphereType atmosphere,
        ThreatLevel threatLevel,
        List<string>? worldEvents = null,
        CancellationToken cancellationToken = default);

    Task<string> GenerateSynthesizedNarrativeAsync(
        int characterId,
        int locationId,
        LocationContent locationContent,
        CancellationToken cancellationToken = default);
}
