namespace TheSignal.Services.Interfaces;

public interface IGeminiClient
{
    Task<string> GenerateNarrativeAsync(string prompt, CancellationToken cancellationToken = default);
    Task<string> GenerateNarrativeAsync(string prompt, float temperature, CancellationToken cancellationToken = default);
}
