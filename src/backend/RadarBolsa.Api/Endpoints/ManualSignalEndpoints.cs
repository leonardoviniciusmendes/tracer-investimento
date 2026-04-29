using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RadarBolsa.Api.Contracts;
using RadarBolsa.Api.Mappings;
using RadarBolsa.Application.Signals;

namespace RadarBolsa.Api.Endpoints;

public static class ManualSignalEndpoints
{
    public static IEndpointRouteBuilder MapManualSignalEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/signals");

        group.MapGet("/", GetManualSignals)
            .WithName("GetManualSignals");

        group.MapPost("/", CreateManualSignal)
            .WithName("CreateManualSignal");

        return app;
    }

    private static async Task<Ok<ManualSignalResponse[]>> GetManualSignals(
        GetManualSignalsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var signals = await useCase.ExecuteAsync(cancellationToken);

        return TypedResults.Ok(
            signals
                .Select(item => item.ToResponse())
                .ToArray());
    }

    private static async Task<Results<Created<ManualSignalResponse>, ValidationProblem, ProblemHttpResult>> CreateManualSignal(
        CreateManualSignalRequest request,
        CreateManualSignalUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(request.ToInput(), cancellationToken);

        if (result.Status == CreateManualSignalStatus.ValidationError)
        {
            return TypedResults.ValidationProblem(result.Errors);
        }

        if (result.Status == CreateManualSignalStatus.NotFound)
        {
            return TypedResults.Problem(
                title: "Tracked asset not found",
                detail: result.NotFoundMessage,
                statusCode: StatusCodes.Status404NotFound);
        }

        var response = result.ManualSignal!.ToResponse();

        return TypedResults.Created($"/api/signals/{response.Id}", response);
    }
}
