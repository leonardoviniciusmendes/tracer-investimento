using RadarBolsa.Application.Abstractions.Persistence;

namespace RadarBolsa.Application.Signals;

public sealed class CreateManualSignalUseCase(
    ITrackedAssetRepository trackedAssetRepository,
    ISignalRepository signalRepository)
{
    public async Task<CreateManualSignalResult> ExecuteAsync(
        CreateManualSignalInput input,
        CancellationToken cancellationToken)
    {
        var normalizedInput = new CreateManualSignalInput(
            input.TrackedAssetId,
            NormalizeTicker(input.Ticker),
            NormalizeText(input.SignalType).ToLowerInvariant(),
            input.Confidence,
            NormalizeText(input.Note),
            input.CapturedAt == default ? DateTimeOffset.UtcNow : input.CapturedAt);

        var errors = Validate(normalizedInput);

        if (errors.Count > 0)
        {
            return CreateManualSignalResult.ValidationError(errors);
        }

        var trackedAsset = await trackedAssetRepository.FindByTickerAsync(
            normalizedInput.Ticker,
            cancellationToken);

        if (trackedAsset is null)
        {
            return CreateManualSignalResult.NotFound(
                $"Tracked asset '{normalizedInput.Ticker}' was not found.");
        }

        var inputWithTrackedAsset = normalizedInput with
        {
            TrackedAssetId = trackedAsset.Id
        };

        var manualSignal = await signalRepository.AddAsync(
            inputWithTrackedAsset,
            cancellationToken);

        return CreateManualSignalResult.Success(manualSignal);
    }

    private static string NormalizeTicker(string? ticker) =>
        (ticker ?? string.Empty).Trim().ToUpperInvariant();

    private static string NormalizeText(string? value) =>
        (value ?? string.Empty).Trim();

    private static Dictionary<string, string[]> Validate(CreateManualSignalInput input)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(input.Ticker))
        {
            errors["ticker"] = ["Ticker is required."];
        }
        else
        {
            var tickerErrors = new List<string>();

            if (input.Ticker.Length is < 4 or > 12)
            {
                tickerErrors.Add("Ticker must have between 4 and 12 characters.");
            }

            if (!input.Ticker.All(char.IsLetterOrDigit))
            {
                tickerErrors.Add("Ticker must contain only letters and numbers.");
            }

            if (tickerErrors.Count > 0)
            {
                errors["ticker"] = tickerErrors.ToArray();
            }
        }

        if (string.IsNullOrWhiteSpace(input.SignalType))
        {
            errors["signalType"] = ["Signal type is required."];
        }
        else if (input.SignalType is not "buy" and not "sell" and not "watch")
        {
            errors["signalType"] = ["Signal type must be buy, sell or watch."];
        }

        if (input.Confidence is < 0 or > 100)
        {
            errors["confidence"] = ["Confidence must be between 0 and 100."];
        }

        if (string.IsNullOrWhiteSpace(input.Note))
        {
            errors["note"] = ["Note is required."];
        }
        else if (input.Note.Length > 500)
        {
            errors["note"] = ["Note must have at most 500 characters."];
        }

        return errors;
    }
}
