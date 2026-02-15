using Microsoft.EntityFrameworkCore;
using ChroniclesOfTheVoid.Data;
using ChroniclesOfTheVoid.Models.Entities;
using ChroniclesOfTheVoid.Repositories.Interfaces;

namespace ChroniclesOfTheVoid.Repositories.Implementations;

public class CharacterRepository : ICharacterRepository
{
    private readonly ApplicationDbContext _context;

    public CharacterRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Character?> GetByIdAsync(int id)
    {
        return await _context.Characters.FindAsync(id);
    }

    public async Task<Character?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Characters
            .Include(c => c.CurrentLocation)
            .Include(c => c.Inventory)
            .Include(c => c.ActiveEffects)
            .Include(c => c.Quests)
            .ThenInclude(cq => cq.Quest)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Character>> GetAllAsync()
    {
        return await _context.Characters.ToListAsync();
    }

    public async Task<IEnumerable<Character>> GetByLocationAsync(int locationId)
    {
        return await _context.Characters
            .Where(c => c.CurrentLocationId == locationId)
            .ToListAsync();
    }

    public async Task<Character> AddAsync(Character character)
    {
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        return character;
    }

    public async Task UpdateAsync(Character character)
    {
        character.LastPlayedAt = DateTime.UtcNow;
        _context.Characters.Update(character);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLocationAsync(int characterId, int newLocationId)
    {
        var character = await _context.Characters.FindAsync(characterId);
        if (character != null)
        {
            character.CurrentLocationId = newLocationId;
            character.LastPlayedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character != null)
        {
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Characters.AnyAsync(c => c.Id == id);
    }
}
