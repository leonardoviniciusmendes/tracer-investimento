namespace RadarBolsa.Application.Opportunities;

public sealed record OpportunityFilters(
    int? MinScore,
    string? Sector,
    decimal? MinUpside,
    decimal? MaxUpside,
    OpportunitySortBy SortBy,
    SortDirection SortDirection);
