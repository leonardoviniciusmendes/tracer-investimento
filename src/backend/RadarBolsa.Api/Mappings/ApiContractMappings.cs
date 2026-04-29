using RadarBolsa.Api.Contracts;
using RadarBolsa.Application.Opportunities;
using RadarBolsa.Domain.Opportunities;
using RadarBolsa.Domain.TrackedAssets;
using RadarBolsa.Application.TrackedAssets;

namespace RadarBolsa.Api.Mappings;

internal static class ApiContractMappings
{
    public static OpportunityFilters ToFilters(
        this GetOpportunitiesRequest request) =>
        new(
            request.MinScore,
            string.IsNullOrWhiteSpace(request.Sector)
                ? null
                : request.Sector.Trim(),
            request.MinUpside,
            request.MaxUpside,
            ParseSortBy(request.SortBy),
            ParseSortDirection(request.SortDirection));

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

    private static OpportunitySortBy ParseSortBy(string? sortBy) =>
        sortBy?.Trim().ToLowerInvariant() switch
        {
            "upside" => OpportunitySortBy.Upside,
            _ => OpportunitySortBy.Score
        };

    private static SortDirection ParseSortDirection(string? sortDirection) =>
        sortDirection?.Trim().ToLowerInvariant() switch
        {
            "asc" => SortDirection.Asc,
            _ => SortDirection.Desc
        };
}
