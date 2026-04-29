using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Domain.TrackedAssets;

namespace RadarBolsa.Application.TrackedAssets;

public sealed class CreateTrackedAssetUseCase(
    ITrackedAssetRepository trackedAssetRepository)
{
    public Task<TrackedAsset> ExecuteAsync(
        CreateTrackedAssetInput input,
        CancellationToken cancellationToken)
    {
        var normalizedInput = new CreateTrackedAssetInput(
            input.Ticker.Trim().ToUpperInvariant(),
            input.CompanyName.Trim(),
            input.Sector.Trim());

        return trackedAssetRepository.AddAsync(normalizedInput, cancellationToken);
    }
}
