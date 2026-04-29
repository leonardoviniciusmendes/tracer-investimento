namespace RadarBolsa.Domain.TrackedAssets;

public sealed record TrackedAsset(
    long Id,
    string Ticker,
    string CompanyName,
    string Sector,
    bool IsActive,
    DateTimeOffset CreatedAt);
