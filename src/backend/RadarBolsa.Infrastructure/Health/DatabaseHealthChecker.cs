using MySqlConnector;
using RadarBolsa.Application.Abstractions.Health;
using RadarBolsa.Application.Health;
using RadarBolsa.Infrastructure.Persistence;

namespace RadarBolsa.Infrastructure.Health;

internal sealed class DatabaseHealthChecker(
    IRadarBolsaDbConnectionFactory connectionFactory) : IDatabaseHealthChecker
{
    public async Task<DatabaseHealthStatus> CheckAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = new MySqlCommand("SELECT 1;", connection);
            await command.ExecuteScalarAsync(cancellationToken);

            return DatabaseHealthStatus.Healthy(DateTimeOffset.UtcNow);
        }
        catch (Exception exception)
        {
            return DatabaseHealthStatus.Unhealthy(
                exception.Message,
                DateTimeOffset.UtcNow);
        }
    }
}
