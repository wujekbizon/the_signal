using System.Text.RegularExpressions;

namespace TheSignal.Services;

public class ParsedNarrative
{
    public string Html { get; set; } = string.Empty;
    public List<DiscoveryMarker> DiscoveryMarkers { get; set; } = new();
}

public class DiscoveryMarker
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

public interface INarrativeParserService
{
    ParsedNarrative ParseNarrative(string rawNarrative);
    string ConvertToDiscoveryHtml(string rawNarrative);
}

public class NarrativeParserService : INarrativeParserService
{
    // Pattern to match [DISCOVER:id]text[/DISCOVER]
    private static readonly Regex DiscoverPattern = new(
        @"\[DISCOVER:(?<id>[^\]]+)\](?<text>[^\[]+)\[/DISCOVER\]",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public ParsedNarrative ParseNarrative(string rawNarrative)
    {
        if (string.IsNullOrWhiteSpace(rawNarrative))
        {
            return new ParsedNarrative { Html = string.Empty };
        }
        
        var markers = new List<DiscoveryMarker>();
        
        // Extract all discovery markers
        var matches = DiscoverPattern.Matches(rawNarrative);
        foreach (Match match in matches)
        {
            markers.Add(new DiscoveryMarker
            {
                Id = match.Groups["id"].Value.Trim(),
                Text = match.Groups["text"].Value.Trim()
            });
        }
        
        // Convert to HTML
        var html = ConvertToDiscoveryHtml(rawNarrative);
        
        return new ParsedNarrative
        {
            Html = html,
            DiscoveryMarkers = markers
        };
    }
    
    public string ConvertToDiscoveryHtml(string rawNarrative)
    {
        if (string.IsNullOrWhiteSpace(rawNarrative))
        {
            return string.Empty;
        }
        
        // Replace discovery markers with HTML links
        var html = DiscoverPattern.Replace(rawNarrative, match =>
        {
            var id = match.Groups["id"].Value.Trim();
            var text = match.Groups["text"].Value.Trim();
            
            return $"<a class=\"discovery-hud\" data-id=\"{id}\" href=\"javascript:void(0);\" aria-label=\"Inspect {text}\">{text}</a>";
        });
        
        // Convert newlines and double newlines to proper HTML paragraphs
        var paragraphs = html.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (paragraphs.Length == 1)
        {
            // Single paragraph - just replace single newlines
            return $"<p>{html.Replace("\n", "<br/>")}</p>";
        }
        
        // Multiple paragraphs
        return string.Join("", paragraphs.Select(p => $"<p>{p.Trim().Replace("\n", "<br/>")}</p>"));
    }
}
