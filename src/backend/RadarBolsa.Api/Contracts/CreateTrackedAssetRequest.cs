namespace RadarBolsa.Api.Contracts;

internal sealed record CreateTrackedAssetRequest(
    string Ticker,
    string CompanyName,
    string Sector);
