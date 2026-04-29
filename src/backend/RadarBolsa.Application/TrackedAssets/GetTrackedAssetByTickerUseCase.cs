using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Domain.TrackedAssets;

namespace RadarBolsa.Application.TrackedAssets;

public sealed class GetTrackedAssetByTickerUseCase(
    ITrackedAssetRepository trackedAssetRepository)
{
    public Task<TrackedAsset?> ExecuteAsync(
        string ticker,
        CancellationToken cancellationToken) =>
        trackedAssetRepository.FindByTickerAsync(ticker, cancellationToken);
}
