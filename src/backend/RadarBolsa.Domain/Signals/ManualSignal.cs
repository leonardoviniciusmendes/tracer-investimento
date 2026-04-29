namespace RadarBolsa.Domain.Signals;

public sealed record ManualSignal(
    long Id,
    string Ticker,
    string CompanyName,
    string SignalType,
    int Confidence,
    string Note,
    DateTimeOffset CapturedAt);
