namespace RadarBolsa.Api.Contracts;

internal sealed record HealthResponse(
    string Status,
    string Service,
    string Database,
    DateTimeOffset Timestamp);
