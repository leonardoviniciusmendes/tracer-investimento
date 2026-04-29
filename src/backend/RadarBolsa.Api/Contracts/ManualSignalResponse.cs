namespace RadarBolsa.Api.Contracts;

internal sealed record ManualSignalResponse(
    long Id,
    string Ticker,
    string CompanyName,
    string SignalType,
    int Confidence,
    string Note,
    DateTimeOffset CapturedAt);
