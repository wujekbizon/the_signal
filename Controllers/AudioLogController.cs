using Microsoft.AspNetCore.Mvc;
using TheSignal.Data;
using TheSignal.Models.GameData;

namespace TheSignal.Controllers;

public class AudioLogController : Controller
{
    private readonly ApplicationDbContext _context;

    public AudioLogController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(int characterId)
    {
        // Get all logs discovered by this character
        var discoveredLogIds = _context.Characters
            .Where(c => c.Id == characterId)
            .SelectMany(c => c.DiscoveredLogs)
            .Select(dl => dl.Id)
            .ToList();

        var allLogs = SeedData.GetAudioLogs();
        
        var viewModel = new AudioLogListViewModel
        {
            CharacterId = characterId,
            DiscoveredLogs = allLogs
                .Where(l => discoveredLogIds.Contains(l.Id))
                .Select(l => new AudioLogSummaryViewModel
                {
                    LogId = l.LogId,
                    Day = l.Day,
                    Speaker = l.Speaker,
                    Title = l.Title,
                    IsDiscovered = true,
                    DiscoveredAt = DateTime.UtcNow // In production, get from DiscoveredLog entity
                }).ToList(),
            TotalLogs = allLogs.Count
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Play(int characterId, int logId)
    {
        var log = SeedData.GetAudioLogs().FirstOrDefault(l => l.Id == logId);
        if (log == null) return NotFound();

        // Mark as discovered (in real implementation, save to DB)
        var viewModel = new AudioLogPlayViewModel
        {
            CharacterId = characterId,
            LogId = log.LogId,
            Day = log.Day,
            Speaker = log.Speaker,
            Title = log.Title,
            Transcript = log.Transcript,
            // Audio properties for future implementation:
            AudioUrl = null, // Will be set when audio files are added
            Duration = EstimateDuration(log.Transcript),
            HasAudio = false // Set to true when audio files are ready
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Discover(int characterId, int logId)
    {
        // In production: Save to Character.DiscoveredLogs
        // For now, just redirect to play view
        return RedirectToAction(nameof(Play), new { characterId, logId });
    }

    private TimeSpan EstimateDuration(string transcript)
    {
        // Rough estimate: average speaking rate ~130 words per minute
        var wordCount = transcript.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        var minutes = wordCount / 130.0;
        return TimeSpan.FromMinutes(minutes);
    }
}

// ViewModels for Audio Logs
public class AudioLogListViewModel
{
    public int CharacterId { get; set; }
    public List<AudioLogSummaryViewModel> DiscoveredLogs { get; set; } = new();
    public int TotalLogs { get; set; }
}

public class AudioLogSummaryViewModel
{
    public string LogId { get; set; } = string.Empty;
    public int Day { get; set; }
    public string Speaker { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsDiscovered { get; set; }
    public DateTime DiscoveredAt { get; set; }
}

public class AudioLogPlayViewModel
{
    public int CharacterId { get; set; }
    public string LogId { get; set; } = string.Empty;
    public int Day { get; set; }
    public string Speaker { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Transcript { get; set; } = string.Empty;
    
    // Audio properties (for future implementation)
    public string? AudioUrl { get; set; }
    public TimeSpan Duration { get; set; }
    public bool HasAudio { get; set; }
}
