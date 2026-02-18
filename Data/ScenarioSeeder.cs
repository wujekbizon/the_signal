using TheSignal.Models.Entities;
using TheSignal.Models.GameData;
using Microsoft.EntityFrameworkCore;

namespace TheSignal.Data;

public class ScenarioSeeder
{
    private readonly ApplicationDbContext _context;

    public ScenarioSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        Console.WriteLine("Updating scenario data...");

        // Clear existing data to ensure full refresh with new schema/ids
        _context.LocationExits.RemoveRange(_context.LocationExits);
        _context.Locations.RemoveRange(_context.Locations);
        _context.Items.RemoveRange(_context.Items);
        _context.Set<AudioLog>().RemoveRange(_context.Set<AudioLog>());
        await _context.SaveChangesAsync();

        // Seed Locations
        var locations = SeedData.GetLocations();
        await _context.Locations.AddRangeAsync(locations);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Seeded {locations.Count} locations");

        // Seed Location Exits
        var exits = SeedData.GetLocationExits();
        await _context.LocationExits.AddRangeAsync(exits);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Seeded {exits.Count} location exits");

        // Seed Items
        var items = SeedData.GetItems();
        await _context.Items.AddRangeAsync(items);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Seeded {items.Count} items");

        // Seed Audio Logs
        var logs = SeedData.GetAudioLogs();
        await _context.Set<AudioLog>().AddRangeAsync(logs);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Seeded {logs.Count} audio logs");

        Console.WriteLine("Scenario seeding complete!");
    }
}
