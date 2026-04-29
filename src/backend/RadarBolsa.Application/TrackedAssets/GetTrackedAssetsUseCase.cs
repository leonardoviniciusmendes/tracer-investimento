using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Domain.TrackedAssets;

namespace RadarBolsa.Application.TrackedAssets;

public sealed class GetTrackedAssetsUseCase(
    ITrackedAssetRepository trackedAssetRepository)
{
    public Task<IReadOnlyList<TrackedAsset>> ExecuteAsync(
        CancellationToken cancellationToken) =>
        trackedAssetRepository.ListAsync(cancellationToken);
}
