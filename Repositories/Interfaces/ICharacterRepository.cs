using TheSignal.Models.Entities;

namespace TheSignal.Repositories.Interfaces;

public interface ICharacterRepository
{
    Task<Character?> GetByIdAsync(int id);
    Task<Character?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Character>> GetAllAsync();
    Task<IEnumerable<Character>> GetByLocationAsync(int locationId);
    Task<Character> AddAsync(Character character);
    Task UpdateAsync(Character character);
    Task UpdateLocationAsync(int characterId, int newLocationId);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
