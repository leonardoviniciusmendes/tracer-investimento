using Microsoft.AspNetCore.Http.HttpResults;
using RadarBolsa.Api.Contracts;
using RadarBolsa.Application.Health;

namespace RadarBolsa.Api.Endpoints;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", GetHealth)
            .WithName("GetHealth");

        return app;
    }

    private static async Task<Results<Ok<HealthResponse>, ProblemHttpResult>> GetHealth(
        GetHealthStatusUseCase useCase,
        CancellationToken cancellationToken)
    {
        var healthStatus = await useCase.ExecuteAsync(cancellationToken);

        if (!healthStatus.IsHealthy)
        {
            return TypedResults.Problem(
                title: "Database connectivity failed",
                detail: healthStatus.ErrorMessage,
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        return TypedResults.Ok(
            new HealthResponse(
                "ok",
                "radarbolsa-api",
                healthStatus.Database,
                healthStatus.CheckedAt));
    }
}
