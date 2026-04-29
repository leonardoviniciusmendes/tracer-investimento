using MySqlConnector;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<DatabaseConnectionProbe>();

var app = builder.Build();

app.UseCors("frontend");

var capturedAt = DateTimeOffset.UtcNow;
var opportunities = new[]
{
    new OpportunityResponse(
        "PETR4",
        "Petrobras PN",
        "Energia",
        31.42m,
        37.80m,
        84,
        "Geracao de caixa consistente, dividendos relevantes e desconto frente aos pares.",
        capturedAt),
    new OpportunityResponse(
        "ITUB4",
        "Itau Unibanco PN",
        "Financeiro",
        34.15m,
        39.60m,
        81,
        "Rentabilidade elevada, qualidade operacional e resiliencia em ciclos distintos.",
        capturedAt),
    new OpportunityResponse(
        "WEGE3",
        "WEG ON",
        "Industria",
        48.90m,
        55.00m,
        78,
        "Execucao recorrente e exposicao a tendencias de eletrificacao e eficiencia.",
        capturedAt),
    new OpportunityResponse(
        "TAEE11",
        "Taesa Unit",
        "Utilidade Publica",
        34.70m,
        38.10m,
        74,
        "Receita previsivel, foco em transmissao e perfil defensivo para carteira.",
        capturedAt)
};

app.MapGet("/health", GetHealth);

app.MapGet("/api/opportunities", (int? minScore, string? sector) =>
{
    IEnumerable<OpportunityResponse> query = opportunities;

    if (minScore.HasValue)
    {
        query = query.Where(item => item.Score >= minScore.Value);
    }

    if (!string.IsNullOrWhiteSpace(sector))
    {
        query = query.Where(item =>
            item.Sector.Equals(sector, StringComparison.OrdinalIgnoreCase));
    }

    return TypedResults.Ok(query.OrderByDescending(item => item.Score));
})
.WithName("GetOpportunities");

static async Task<Microsoft.AspNetCore.Http.HttpResults.Results<Ok<HealthResponse>, ProblemHttpResult>> GetHealth(
    DatabaseConnectionProbe databaseConnectionProbe,
    CancellationToken cancellationToken)
{
    var databaseCheck = await databaseConnectionProbe.CheckAsync(cancellationToken);

    if (!databaseCheck.IsHealthy)
    {
        return TypedResults.Problem(
            title: "Database connectivity failed",
            detail: databaseCheck.ErrorMessage,
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }

    return TypedResults.Ok(
        new HealthResponse(
            "ok",
            "radarbolsa-api",
            "ok",
            DateTimeOffset.UtcNow));
}

app.Run();

internal sealed record OpportunityResponse(
    string Ticker,
    string CompanyName,
    string Sector,
    decimal CurrentPrice,
    decimal TargetPrice,
    int Score,
    string Thesis,
    DateTimeOffset CapturedAt)
{
    public decimal UpsidePercent =>
        CurrentPrice == 0
            ? 0
            : Math.Round(((TargetPrice - CurrentPrice) / CurrentPrice) * 100, 2);
}

internal sealed record HealthResponse(
    string Status,
    string Service,
    string Database,
    DateTimeOffset Timestamp);

internal sealed class DatabaseConnectionProbe(IConfiguration configuration)
{
    private readonly string _connectionString =
        configuration.GetConnectionString("RadarBolsa")
        ?? throw new InvalidOperationException(
            "Connection string 'RadarBolsa' was not configured.");

    public async Task<DatabaseHealthResult> CheckAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new MySqlCommand("SELECT 1;", connection);
            await command.ExecuteScalarAsync(cancellationToken);

            return DatabaseHealthResult.Healthy();
        }
        catch (Exception exception)
        {
            return DatabaseHealthResult.Unhealthy(exception.Message);
        }
    }
}

internal sealed record DatabaseHealthResult(bool IsHealthy, string? ErrorMessage)
{
    public static DatabaseHealthResult Healthy() => new(true, null);

    public static DatabaseHealthResult Unhealthy(string errorMessage) =>
        new(false, errorMessage);
}
