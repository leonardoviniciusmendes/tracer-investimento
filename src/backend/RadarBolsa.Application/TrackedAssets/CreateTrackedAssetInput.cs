namespace RadarBolsa.Application.TrackedAssets;

public sealed record CreateTrackedAssetInput(
    string Ticker,
    string CompanyName,
    string Sector);
