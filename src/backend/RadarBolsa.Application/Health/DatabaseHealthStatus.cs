namespace RadarBolsa.Application.Health;

public sealed record DatabaseHealthStatus(
    bool IsHealthy,
    string Database,
    string? ErrorMessage,
    DateTimeOffset CheckedAt)
{
    public static DatabaseHealthStatus Healthy(DateTimeOffset checkedAt) =>
        new(true, "ok", null, checkedAt);

    public static DatabaseHealthStatus Unhealthy(
        string errorMessage,
        DateTimeOffset checkedAt) =>
        new(false, "unavailable", errorMessage, checkedAt);
}
