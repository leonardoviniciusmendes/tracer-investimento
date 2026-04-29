using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace RadarBolsa.Infrastructure.Persistence;

internal sealed class RadarBolsaDbConnectionFactory(IConfiguration configuration)
    : IRadarBolsaDbConnectionFactory
{
    private readonly string _connectionString =
        configuration.GetConnectionString("RadarBolsa")
        ?? throw new InvalidOperationException(
            "Connection string 'RadarBolsa' was not configured.");

    public MySqlConnection CreateConnection() => new(_connectionString);
}
