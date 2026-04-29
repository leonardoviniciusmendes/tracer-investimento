using System.Text;
using MySqlConnector;
using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Application.Opportunities;
using RadarBolsa.Domain.Opportunities;

namespace RadarBolsa.Infrastructure.Persistence.Opportunities;

internal sealed class MySqlOpportunityReadRepository(
    IRadarBolsaDbConnectionFactory connectionFactory) : IOpportunityReadRepository
{
    public async Task<IReadOnlyList<Opportunity>> ListAsync(
        OpportunityFilters filters,
        CancellationToken cancellationToken)
    {
        const string upsideExpression =
            """
            CASE
                WHEN o.current_price = 0 THEN 0
                ELSE ((o.target_price - o.current_price) / o.current_price) * 100
            END
            """;

        var sql = new StringBuilder(
            """
            SELECT
                ta.ticker,
                ta.company_name,
                ta.sector,
                o.current_price,
                o.target_price,
                o.score,
                o.thesis,
                o.captured_at
            FROM opportunities o
            INNER JOIN tracked_assets ta ON ta.id = o.tracked_asset_id
            WHERE ta.is_active = b'1'
            """);

        if (filters.MinScore.HasValue)
        {
            sql.AppendLine(" AND o.score >= @minScore");
        }

        if (!string.IsNullOrWhiteSpace(filters.Sector))
        {
            sql.AppendLine(" AND LOWER(ta.sector) = LOWER(@sector)");
        }

        if (filters.MinUpside.HasValue)
        {
            sql.AppendLine($" AND {upsideExpression} >= @minUpside");
        }

        if (filters.MaxUpside.HasValue)
        {
            sql.AppendLine($" AND {upsideExpression} <= @maxUpside");
        }

        var sortColumn = filters.SortBy == OpportunitySortBy.Upside
            ? upsideExpression
            : "o.score";

        var sortDirection = filters.SortDirection == SortDirection.Asc
            ? "ASC"
            : "DESC";

        sql.AppendLine(
            $" ORDER BY {sortColumn} {sortDirection}, o.score DESC, o.captured_at DESC;");

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql.ToString(), connection);

        if (filters.MinScore.HasValue)
        {
            command.Parameters.AddWithValue("@minScore", filters.MinScore.Value);
        }

        if (!string.IsNullOrWhiteSpace(filters.Sector))
        {
            command.Parameters.AddWithValue("@sector", filters.Sector);
        }

        if (filters.MinUpside.HasValue)
        {
            command.Parameters.AddWithValue("@minUpside", filters.MinUpside.Value);
        }

        if (filters.MaxUpside.HasValue)
        {
            command.Parameters.AddWithValue("@maxUpside", filters.MaxUpside.Value);
        }

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var results = new List<Opportunity>();

        var tickerOrdinal = reader.GetOrdinal("ticker");
        var companyNameOrdinal = reader.GetOrdinal("company_name");
        var sectorOrdinal = reader.GetOrdinal("sector");
        var currentPriceOrdinal = reader.GetOrdinal("current_price");
        var targetPriceOrdinal = reader.GetOrdinal("target_price");
        var scoreOrdinal = reader.GetOrdinal("score");
        var thesisOrdinal = reader.GetOrdinal("thesis");
        var capturedAtOrdinal = reader.GetOrdinal("captured_at");

        while (await reader.ReadAsync(cancellationToken))
        {
            var capturedAt = DateTime.SpecifyKind(
                reader.GetDateTime(capturedAtOrdinal),
                DateTimeKind.Utc);

            results.Add(
                new Opportunity(
                    reader.GetString(tickerOrdinal),
                    reader.GetString(companyNameOrdinal),
                    reader.GetString(sectorOrdinal),
                    reader.GetDecimal(currentPriceOrdinal),
                    reader.GetDecimal(targetPriceOrdinal),
                    reader.GetInt32(scoreOrdinal),
                    reader.GetString(thesisOrdinal),
                    new DateTimeOffset(capturedAt)));
        }

        return results;
    }
}
