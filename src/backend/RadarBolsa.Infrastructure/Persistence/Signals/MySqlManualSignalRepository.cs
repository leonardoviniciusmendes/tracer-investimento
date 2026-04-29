using MySqlConnector;
using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Application.Signals;
using RadarBolsa.Domain.Signals;

namespace RadarBolsa.Infrastructure.Persistence.Signals;

internal sealed class MySqlManualSignalRepository(
    IRadarBolsaDbConnectionFactory connectionFactory) : ISignalRepository
{
    public async Task<ManualSignal> AddAsync(
        CreateManualSignalInput input,
        CancellationToken cancellationToken)
    {
        const string insertSql = """
            INSERT INTO manual_signals (
                tracked_asset_id,
                signal_type,
                confidence,
                note,
                captured_at)
            VALUES (
                @trackedAssetId,
                @signalType,
                @confidence,
                @note,
                @capturedAt);
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var insertCommand = new MySqlCommand(insertSql, connection);
        insertCommand.Parameters.AddWithValue("@trackedAssetId", input.TrackedAssetId);
        insertCommand.Parameters.AddWithValue("@signalType", input.SignalType);
        insertCommand.Parameters.AddWithValue("@confidence", input.Confidence);
        insertCommand.Parameters.AddWithValue("@note", input.Note);
        insertCommand.Parameters.AddWithValue("@capturedAt", input.CapturedAt.UtcDateTime);

        await insertCommand.ExecuteNonQueryAsync(cancellationToken);

        return await GetByIdAsync(
            insertCommand.LastInsertedId,
            connection,
            cancellationToken)
            ?? throw new InvalidOperationException(
                "Manual signal was inserted but could not be retrieved.");
    }

    public async Task<IReadOnlyList<ManualSignal>> ListAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                ms.id,
                ta.ticker,
                ta.company_name,
                ms.signal_type,
                ms.confidence,
                ms.note,
                ms.captured_at
            FROM manual_signals ms
            INNER JOIN tracked_assets ta ON ta.id = ms.tracked_asset_id
            ORDER BY ms.captured_at DESC, ms.id DESC;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var signals = new List<ManualSignal>();

        while (await reader.ReadAsync(cancellationToken))
        {
            signals.Add(MapManualSignal(reader));
        }

        return signals;
    }

    private static ManualSignal MapManualSignal(MySqlDataReader reader)
    {
        var capturedAt = DateTime.SpecifyKind(
            reader.GetDateTime("captured_at"),
            DateTimeKind.Utc);

        return new ManualSignal(
            reader.GetInt64("id"),
            reader.GetString("ticker"),
            reader.GetString("company_name"),
            reader.GetString("signal_type"),
            reader.GetInt32("confidence"),
            reader.GetString("note"),
            new DateTimeOffset(capturedAt));
    }

    private static async Task<ManualSignal?> GetByIdAsync(
        long id,
        MySqlConnection connection,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                ms.id,
                ta.ticker,
                ta.company_name,
                ms.signal_type,
                ms.confidence,
                ms.note,
                ms.captured_at
            FROM manual_signals ms
            INNER JOIN tracked_assets ta ON ta.id = ms.tracked_asset_id
            WHERE ms.id = @id
            LIMIT 1;
            """;

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapManualSignal(reader);
    }
}
