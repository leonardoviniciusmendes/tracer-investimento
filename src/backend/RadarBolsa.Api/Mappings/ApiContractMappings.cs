using System.Globalization;
using RadarBolsa.Api.Contracts;
using RadarBolsa.Application.Opportunities;
using RadarBolsa.Domain.Opportunities;
using RadarBolsa.Domain.TrackedAssets;
using RadarBolsa.Application.TrackedAssets;

namespace RadarBolsa.Api.Mappings;

internal static class ApiContractMappings
{
    public static GetOpportunitiesRequestValidationResult ValidateAndMap(
        this GetOpportunitiesRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        var minScore = ParseNullableInt(
            request.MinScore,
            "minScore",
            0,
            100,
            errors);

        var minUpside = ParseNullableDecimal(request.MinUpside, "minUpside", errors);
        var maxUpside = ParseNullableDecimal(request.MaxUpside, "maxUpside", errors);

        if (minUpside.HasValue && maxUpside.HasValue && minUpside > maxUpside)
        {
            errors["upsideRange"] =
            [
                "minUpside must be less than or equal to maxUpside."
            ];
        }

        var sector = NormalizeSector(request.Sector, errors);
        var sortBy = ParseSortBy(request.SortBy, errors);
        var sortDirection = ParseSortDirection(request.SortDirection, errors);

        if (errors.Count > 0)
        {
            return GetOpportunitiesRequestValidationResult.Failure(errors);
        }

        return GetOpportunitiesRequestValidationResult.Success(
            new OpportunityFilters(
                minScore,
                sector,
                minUpside,
                maxUpside,
                sortBy,
                sortDirection));
    }

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

    private static int? ParseNullableInt(
        string? value,
        string field,
        int min,
        int max,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!int.TryParse(
                value.Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var parsedValue))
        {
            errors[field] = [$"{field} must be a valid integer."];
            return null;
        }

        if (parsedValue < min || parsedValue > max)
        {
            errors[field] = [$"{field} must be between {min} and {max}."];
            return null;
        }

        return parsedValue;
    }

    private static decimal? ParseNullableDecimal(
        string? value,
        string field,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!decimal.TryParse(
                value.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var parsedValue))
        {
            errors[field] = [$"{field} must be a valid decimal number."];
            return null;
        }

        return parsedValue;
    }

    private static string? NormalizeSector(
        string? sector,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(sector))
        {
            return null;
        }

        var normalizedSector = sector.Trim();

        if (normalizedSector.Length > 80)
        {
            errors["sector"] = ["sector must have at most 80 characters."];
            return null;
        }

        return normalizedSector;
    }

    private static OpportunitySortBy ParseSortBy(
        string? sortBy,
        IDictionary<string, string[]> errors) =>
        sortBy?.Trim().ToLowerInvariant() switch
        {
            "upside" => OpportunitySortBy.Upside,
            null or "" => OpportunitySortBy.Score,
            "score" => OpportunitySortBy.Score,
            _ => AddSortByError(errors)
        };

    private static SortDirection ParseSortDirection(
        string? sortDirection,
        IDictionary<string, string[]> errors) =>
        sortDirection?.Trim().ToLowerInvariant() switch
        {
            "asc" => SortDirection.Asc,
            null or "" => SortDirection.Desc,
            "desc" => SortDirection.Desc,
            _ => AddSortDirectionError(errors)
        };

    private static OpportunitySortBy AddSortByError(
        IDictionary<string, string[]> errors)
    {
        errors["sortBy"] = ["sortBy must be 'score' or 'upside'."];
        return OpportunitySortBy.Score;
    }

    private static SortDirection AddSortDirectionError(
        IDictionary<string, string[]> errors)
    {
        errors["sortDirection"] = ["sortDirection must be 'asc' or 'desc'."];
        return SortDirection.Desc;
    }
}
