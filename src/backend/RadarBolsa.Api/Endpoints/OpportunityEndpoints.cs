using Microsoft.AspNetCore.Http.HttpResults;
using RadarBolsa.Api.Contracts;
using RadarBolsa.Api.Mappings;
using RadarBolsa.Application.Opportunities;

namespace RadarBolsa.Api.Endpoints;

public static class OpportunityEndpoints
{
    public static IEndpointRouteBuilder MapOpportunityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/opportunities", GetOpportunities)
            .WithName("GetOpportunities");

        return app;
    }

    private static async Task<Ok<OpportunityResponse[]>> GetOpportunities(
        int? minScore,
        string? sector,
        GetOpportunitiesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var opportunities = await useCase.ExecuteAsync(
            minScore,
            sector,
            cancellationToken);

        return TypedResults.Ok(
            opportunities
                .Select(item => item.ToResponse())
                .ToArray());
    }
}
