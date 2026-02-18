using TheSignal.Models.Entities;
using TheSignal.Models.GameData;

namespace TheSignal.Services.Interfaces;

public interface IExplorationService
{
    Task<ExplorationState?> GetExplorationStateAsync(int characterId, CancellationToken cancellationToken = default);
    
    Task<(ProceduralRoom Room, bool IsNew)> EnterProceduralGapAsync(
        int characterId, 
        int sourceLocationId, 
        int targetLocationId,
        int gapSize,
        string gapType,
        CancellationToken cancellationToken = default);
    
    Task<(ProceduralRoom? Room, bool ReachedTarget, ExplorationState State)> MoveInGapAsync(
        int characterId,
        bool moveForward,
        CancellationToken cancellationToken = default);
    
    Task ResetExplorationAsync(int characterId, int targetLocationId, CancellationToken cancellationToken = default);
    
    Task<bool> IsInProceduralGapAsync(int characterId, CancellationToken cancellationToken = default);
    
    Task ConsumePlayerAsync(int characterId, CancellationToken cancellationToken = default);
}
