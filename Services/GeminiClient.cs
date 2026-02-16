using Google.GenAI;
using Google.GenAI.Types;
using TheSignal.Services.Interfaces;

namespace TheSignal.Services;

public class GeminiClient : IGeminiClient
{
    private readonly Client _client;
    private readonly string _modelName;
    private readonly int _maxOutputTokens;
    private readonly float _defaultTemperature;

    public GeminiClient(IConfiguration configuration)
    {
        var apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Gemini:ApiKey not configured");
        _modelName = configuration["Gemini:ModelName"] ?? "gemini-2.5-flash";
        _maxOutputTokens = configuration.GetValue<int>("Gemini:MaxOutputTokens", 4096);
        _defaultTemperature = configuration.GetValue<float>("Gemini:Temperature", 0.7f);

        _client = new Client(apiKey: apiKey);
    }

    public async Task<string> GenerateNarrativeAsync(string prompt, CancellationToken cancellationToken = default)
    {
        return await GenerateNarrativeAsync(prompt, _defaultTemperature, cancellationToken);
    }

    public async Task<string> GenerateNarrativeAsync(string prompt, float temperature, CancellationToken cancellationToken = default)
    {
        var config = new GenerateContentConfig
        {
            Temperature = temperature,
            MaxOutputTokens = _maxOutputTokens
        };

        var response = await _client.Models.GenerateContentAsync(
            model: _modelName,
            contents: prompt,
            config: config,
            cancellationToken: cancellationToken);

        if (response?.Candidates?[0]?.Content?.Parts?[0]?.Text is not string text)
        {
            throw new InvalidOperationException("No content generated");
        }

        return text;
    }
}
