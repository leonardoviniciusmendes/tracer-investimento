namespace RadarBolsa.Api.Contracts;

internal sealed record TrackedAssetResponse(
    long Id,
    string Ticker,
    string CompanyName,
    string Sector,
    bool IsActive,
    DateTimeOffset CreatedAt);
