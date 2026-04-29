using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RadarBolsa.Api.Contracts;
using RadarBolsa.Api.Mappings;
using RadarBolsa.Application.TrackedAssets;

namespace RadarBolsa.Api.Endpoints;

public static class TrackedAssetEndpoints
{
    public static IEndpointRouteBuilder MapTrackedAssetEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tracked-assets");

        group.MapGet("/", GetTrackedAssets)
            .WithName("GetTrackedAssets");

        group.MapGet("/{ticker}", GetTrackedAssetByTicker)
            .WithName("GetTrackedAssetByTicker");

        group.MapPost("/", CreateTrackedAsset)
            .WithName("CreateTrackedAsset");

        return app;
    }

    private static async Task<Ok<TrackedAssetResponse[]>> GetTrackedAssets(
        GetTrackedAssetsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var trackedAssets = await useCase.ExecuteAsync(cancellationToken);

        return TypedResults.Ok(
            trackedAssets
                .Select(item => item.ToResponse())
                .ToArray());
    }

    private static async Task<Results<Ok<TrackedAssetResponse>, NotFound>> GetTrackedAssetByTicker(
        string ticker,
        GetTrackedAssetByTickerUseCase useCase,
        CancellationToken cancellationToken)
    {
        var trackedAsset = await useCase.ExecuteAsync(ticker, cancellationToken);

        if (trackedAsset is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(trackedAsset.ToResponse());
    }

    private static async Task<Results<Created<TrackedAssetResponse>, ValidationProblem, ProblemHttpResult>> CreateTrackedAsset(
        CreateTrackedAssetRequest request,
        CreateTrackedAssetUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(
            request.ToInput(),
            cancellationToken);

        if (result.Status == CreateTrackedAssetStatus.ValidationError)
        {
            return TypedResults.ValidationProblem(result.Errors);
        }

        if (result.Status == CreateTrackedAssetStatus.Conflict)
        {
            return TypedResults.Problem(
                title: "Tracked asset already exists",
                detail: result.ConflictMessage,
                statusCode: StatusCodes.Status409Conflict);
        }

        var response = result.TrackedAsset!.ToResponse();

        return TypedResults.Created(
            $"/api/tracked-assets/{response.Ticker}",
            response);
    }
}
