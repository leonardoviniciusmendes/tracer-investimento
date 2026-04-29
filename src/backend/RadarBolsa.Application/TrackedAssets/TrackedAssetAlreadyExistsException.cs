namespace RadarBolsa.Application.TrackedAssets;

public sealed class TrackedAssetAlreadyExistsException(string ticker)
    : Exception($"Tracked asset '{ticker}' already exists.")
{
    public string Ticker { get; } = ticker;
}
