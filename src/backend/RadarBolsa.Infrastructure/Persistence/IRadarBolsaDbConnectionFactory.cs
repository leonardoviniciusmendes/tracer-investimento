using MySqlConnector;

namespace RadarBolsa.Infrastructure.Persistence;

internal interface IRadarBolsaDbConnectionFactory
{
    MySqlConnection CreateConnection();
}
