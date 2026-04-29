namespace RadarBolsa.Api.Contracts;

internal sealed class GetOpportunitiesRequest
{
    public int? MinScore { get; init; }

    public string? Sector { get; init; }
}
