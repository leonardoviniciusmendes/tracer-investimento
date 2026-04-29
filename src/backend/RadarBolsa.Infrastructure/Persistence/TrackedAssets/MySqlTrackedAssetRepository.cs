using MySqlConnector;
using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Application.TrackedAssets;
using RadarBolsa.Domain.TrackedAssets;

namespace RadarBolsa.Infrastructure.Persistence.TrackedAssets;

internal sealed class MySqlTrackedAssetRepository(
    IRadarBolsaDbConnectionFactory connectionFactory) : ITrackedAssetRepository
{
    public async Task<TrackedAsset> AddAsync(
        CreateTrackedAssetInput input,
        CancellationToken cancellationToken)
    {
        const string insertSql = """
            INSERT INTO tracked_assets (
                ticker,
                company_name,
                sector,
                is_active)
            VALUES (
                @ticker,
                @companyName,
                @sector,
                b'1');
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var insertCommand = new MySqlCommand(insertSql, connection);
        insertCommand.Parameters.AddWithValue("@ticker", input.Ticker);
        insertCommand.Parameters.AddWithValue("@companyName", input.CompanyName);
        insertCommand.Parameters.AddWithValue("@sector", input.Sector);

        await insertCommand.ExecuteNonQueryAsync(cancellationToken);

        return await GetByIdAsync(
            insertCommand.LastInsertedId,
            connection,
            cancellationToken)
            ?? throw new InvalidOperationException(
                "Tracked asset was inserted but could not be retrieved.");
    }

    public async Task<TrackedAsset?> FindByTickerAsync(
        string ticker,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                id,
                ticker,
                company_name,
                sector,
                is_active,
                created_at
            FROM tracked_assets
            WHERE ticker = @ticker
            LIMIT 1;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ticker", ticker.Trim().ToUpperInvariant());

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapTrackedAsset(reader);
    }

    public async Task<IReadOnlyList<TrackedAsset>> ListAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                id,
                ticker,
                company_name,
                sector,
                is_active,
                created_at
            FROM tracked_assets
            ORDER BY ticker;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var trackedAssets = new List<TrackedAsset>();

        while (await reader.ReadAsync(cancellationToken))
        {
            trackedAssets.Add(MapTrackedAsset(reader));
        }

        return trackedAssets;
    }

    private static TrackedAsset MapTrackedAsset(MySqlDataReader reader)
    {
        var createdAt = DateTime.SpecifyKind(
            reader.GetDateTime("created_at"),
            DateTimeKind.Utc);

        return new TrackedAsset(
            reader.GetInt64("id"),
            reader.GetString("ticker"),
            reader.GetString("company_name"),
            reader.GetString("sector"),
            ReadBoolean(reader, "is_active"),
            new DateTimeOffset(createdAt));
    }

    private static bool ReadBoolean(MySqlDataReader reader, string columnName)
    {
        var value = reader.GetValue(reader.GetOrdinal(columnName));

        return value switch
        {
            bool booleanValue => booleanValue,
            ulong integerValue => integerValue == 1,
            long integerValue => integerValue == 1,
            byte[] bytes => bytes.Length > 0 && bytes[0] == 1,
            _ => Convert.ToBoolean(value)
        };
    }

    private static async Task<TrackedAsset?> GetByIdAsync(
        long id,
        MySqlConnection connection,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                id,
                ticker,
                company_name,
                sector,
                is_active,
                created_at
            FROM tracked_assets
            WHERE id = @id
            LIMIT 1;
            """;

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapTrackedAsset(reader);
    }
}
