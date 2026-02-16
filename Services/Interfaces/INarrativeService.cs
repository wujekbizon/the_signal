using TheSignal.Models.Enums;

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
}
