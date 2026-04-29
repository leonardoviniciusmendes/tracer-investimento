using RadarBolsa.Application.Health;

namespace RadarBolsa.Application.Abstractions.Health;

public interface IDatabaseHealthChecker
{
    Task<DatabaseHealthStatus> CheckAsync(CancellationToken cancellationToken);
}
