using Microsoft.EntityFrameworkCore;
using ChroniclesOfTheVoid.Data;
using ChroniclesOfTheVoid.Models.Entities;
using ChroniclesOfTheVoid.Repositories.Interfaces;

namespace ChroniclesOfTheVoid.Repositories.Implementations;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;

    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Location?> GetByIdAsync(int id)
    {
        return await _context.Locations.FindAsync(id);
    }

    public async Task<Location?> GetByIdWithExitsAsync(int id)
    {
        return await _context.Locations
            .Include(l => l.Exits)
            .ThenInclude(e => e.Conditions)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _context.Locations.ToListAsync();
    }

    public async Task<IEnumerable<Location>> GetByBiomeAsync(string biome)
    {
        return await _context.Locations
            .Where(l => l.Biome.ToString() == biome)
            .ToListAsync();
    }

    public async Task<Location> AddAsync(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task UpdateAsync(Location location)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Locations.AnyAsync(l => l.Id == id);
    }
}
