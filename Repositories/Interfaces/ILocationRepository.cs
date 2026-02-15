using ChroniclesOfTheVoid.Models.Entities;

namespace ChroniclesOfTheVoid.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int id);
    Task<Location?> GetByIdWithExitsAsync(int id);
    Task<IEnumerable<Location>> GetAllAsync();
    Task<IEnumerable<Location>> GetByBiomeAsync(string biome);
    Task<Location> AddAsync(Location location);
    Task UpdateAsync(Location location);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
