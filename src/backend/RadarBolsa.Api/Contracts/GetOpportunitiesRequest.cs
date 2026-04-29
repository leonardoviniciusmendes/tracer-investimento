namespace RadarBolsa.Api.Contracts;

internal sealed class GetOpportunitiesRequest
{
    public string? MinScore { get; init; }

    public string? Sector { get; init; }

    public string? MinUpside { get; init; }

    public string? MaxUpside { get; init; }

    public string? SortBy { get; init; }

    public string? SortDirection { get; init; }
}
