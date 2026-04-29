using RadarBolsa.Domain.Signals;

namespace RadarBolsa.Application.Signals;

public enum CreateManualSignalStatus
{
    Success,
    ValidationError,
    NotFound
}

public sealed record CreateManualSignalResult(
    CreateManualSignalStatus Status,
    ManualSignal? ManualSignal,
    IReadOnlyDictionary<string, string[]> Errors,
    string? NotFoundMessage)
{
    public static CreateManualSignalResult Success(ManualSignal manualSignal) =>
        new(CreateManualSignalStatus.Success, manualSignal, EmptyErrors, null);

    public static CreateManualSignalResult ValidationError(
        IReadOnlyDictionary<string, string[]> errors) =>
        new(CreateManualSignalStatus.ValidationError, null, errors, null);

    public static CreateManualSignalResult NotFound(string message) =>
        new(CreateManualSignalStatus.NotFound, null, EmptyErrors, message);

    private static IReadOnlyDictionary<string, string[]> EmptyErrors { get; } =
        new Dictionary<string, string[]>();
}
