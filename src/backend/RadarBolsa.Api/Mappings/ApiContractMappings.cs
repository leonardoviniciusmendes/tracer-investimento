using RadarBolsa.Api.Contracts;
using RadarBolsa.Domain.Opportunities;
using RadarBolsa.Domain.TrackedAssets;
using RadarBolsa.Application.TrackedAssets;

namespace RadarBolsa.Api.Mappings;

internal static class ApiContractMappings
{
    public static OpportunityResponse ToResponse(this Opportunity opportunity) =>
        new(
            opportunity.Ticker,
            opportunity.CompanyName,
            opportunity.Sector,
            opportunity.CurrentPrice,
            opportunity.TargetPrice,
            opportunity.Score,
            opportunity.Thesis,
            opportunity.CapturedAt);

    public static CreateTrackedAssetInput ToInput(
        this CreateTrackedAssetRequest request) =>
        new(
            request.Ticker,
            request.CompanyName,
            request.Sector);

    public static TrackedAssetResponse ToResponse(this TrackedAsset trackedAsset) =>
        new(
            trackedAsset.Id,
            trackedAsset.Ticker,
            trackedAsset.CompanyName,
            trackedAsset.Sector,
            trackedAsset.IsActive,
            trackedAsset.CreatedAt);
}
