using RadarBolsa.Application.Abstractions.Health;

namespace RadarBolsa.Application.Health;

public sealed class GetHealthStatusUseCase(IDatabaseHealthChecker databaseHealthChecker)
{
    public Task<DatabaseHealthStatus> ExecuteAsync(CancellationToken cancellationToken) =>
        databaseHealthChecker.CheckAsync(cancellationToken);
}
