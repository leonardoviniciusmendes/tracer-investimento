using RadarBolsa.Application.TrackedAssets;
using RadarBolsa.Domain.TrackedAssets;

namespace RadarBolsa.Application.Abstractions.Persistence;

public interface ITrackedAssetRepository
{
    Task<TrackedAsset> AddAsync(
        CreateTrackedAssetInput input,
        CancellationToken cancellationToken);

    Task<TrackedAsset?> FindByTickerAsync(
        string ticker,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<TrackedAsset>> ListAsync(CancellationToken cancellationToken);
}
