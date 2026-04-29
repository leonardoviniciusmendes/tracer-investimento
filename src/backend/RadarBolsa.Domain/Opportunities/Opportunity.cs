namespace RadarBolsa.Domain.Opportunities;

public sealed record Opportunity(
    string Ticker,
    string CompanyName,
    string Sector,
    decimal CurrentPrice,
    decimal TargetPrice,
    int Score,
    string Thesis,
    DateTimeOffset CapturedAt)
{
    public decimal UpsidePercent =>
        CurrentPrice == 0
            ? 0
            : Math.Round(((TargetPrice - CurrentPrice) / CurrentPrice) * 100, 2);
}
