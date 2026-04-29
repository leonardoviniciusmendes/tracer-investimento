namespace RadarBolsa.Application.Signals;

public sealed record CreateManualSignalInput(
    long TrackedAssetId,
    string Ticker,
    string SignalType,
    int Confidence,
    string Note,
    DateTimeOffset CapturedAt);
