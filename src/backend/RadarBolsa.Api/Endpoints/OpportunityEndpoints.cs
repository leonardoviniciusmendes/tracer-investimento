using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

    private static async Task<Results<Ok<OpportunityResponse[]>, ValidationProblem>> GetOpportunities(
        [AsParameters] GetOpportunitiesRequest request,
        GetOpportunitiesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var validationResult = request.ValidateAndMap();

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.Errors);
        }

        var opportunities = await useCase.ExecuteAsync(
            validationResult.Filters!,
            cancellationToken);

        return TypedResults.Ok(
            opportunities
                .Select(item => item.ToResponse())
                .ToArray());
    }
}
