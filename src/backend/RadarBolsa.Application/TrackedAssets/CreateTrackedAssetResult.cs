using RadarBolsa.Domain.TrackedAssets;

namespace RadarBolsa.Application.TrackedAssets;

public enum CreateTrackedAssetStatus
{
    Success,
    ValidationError,
    Conflict
}

public sealed record CreateTrackedAssetResult(
    CreateTrackedAssetStatus Status,
    TrackedAsset? TrackedAsset,
    IReadOnlyDictionary<string, string[]> Errors,
    string? ConflictMessage)
{
    public static CreateTrackedAssetResult Success(TrackedAsset trackedAsset) =>
        new(CreateTrackedAssetStatus.Success, trackedAsset, EmptyErrors, null);

    public static CreateTrackedAssetResult ValidationError(
        IReadOnlyDictionary<string, string[]> errors) =>
        new(CreateTrackedAssetStatus.ValidationError, null, errors, null);

    public static CreateTrackedAssetResult Conflict(string message) =>
        new(CreateTrackedAssetStatus.Conflict, null, EmptyErrors, message);

    private static IReadOnlyDictionary<string, string[]> EmptyErrors { get; } =
        new Dictionary<string, string[]>();
}
