namespace RadarBolsa.Api.Contracts;

internal sealed class GetOpportunitiesRequest
{
    public int? MinScore { get; init; }

    public string? Sector { get; init; }

    public decimal? MinUpside { get; init; }

    public decimal? MaxUpside { get; init; }

    public string? SortBy { get; init; }

    public string? SortDirection { get; init; }
}
