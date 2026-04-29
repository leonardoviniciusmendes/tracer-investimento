namespace RadarBolsa.Api.Contracts;

internal sealed record CreateManualSignalRequest(
    string Ticker,
    string SignalType,
    int Confidence,
    string Note,
    DateTimeOffset? CapturedAt);
