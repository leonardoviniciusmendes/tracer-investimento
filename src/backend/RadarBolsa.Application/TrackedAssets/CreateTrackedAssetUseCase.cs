using RadarBolsa.Application.Abstractions.Persistence;

namespace RadarBolsa.Application.TrackedAssets;

public sealed class CreateTrackedAssetUseCase(
    ITrackedAssetRepository trackedAssetRepository)
{
    public async Task<CreateTrackedAssetResult> ExecuteAsync(
        CreateTrackedAssetInput input,
        CancellationToken cancellationToken)
    {
        var normalizedInput = new CreateTrackedAssetInput(
            NormalizeTicker(input.Ticker),
            NormalizeText(input.CompanyName),
            NormalizeText(input.Sector));

        var errors = Validate(normalizedInput);

        if (errors.Count > 0)
        {
            return CreateTrackedAssetResult.ValidationError(errors);
        }

        var existingTrackedAsset = await trackedAssetRepository.FindByTickerAsync(
            normalizedInput.Ticker,
            cancellationToken);

        if (existingTrackedAsset is not null)
        {
            return CreateTrackedAssetResult.Conflict(
                $"Tracked asset '{normalizedInput.Ticker}' is already registered.");
        }

        try
        {
            var trackedAsset = await trackedAssetRepository.AddAsync(
                normalizedInput,
                cancellationToken);

            return CreateTrackedAssetResult.Success(trackedAsset);
        }
        catch (TrackedAssetAlreadyExistsException)
        {
            return CreateTrackedAssetResult.Conflict(
                $"Tracked asset '{normalizedInput.Ticker}' is already registered.");
        }
    }

    private static string NormalizeTicker(string? ticker) =>
        (ticker ?? string.Empty).Trim().ToUpperInvariant();

    private static string NormalizeText(string? value) =>
        (value ?? string.Empty).Trim();

    private static Dictionary<string, string[]> Validate(CreateTrackedAssetInput input)
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

        ValidateRequiredText(
            errors,
            "companyName",
            input.CompanyName,
            120,
            "Company name");

        ValidateRequiredText(
            errors,
            "sector",
            input.Sector,
            80,
            "Sector");

        return errors;
    }

    private static void ValidateRequiredText(
        IDictionary<string, string[]> errors,
        string field,
        string value,
        int maxLength,
        string label)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors[field] = [$"{label} is required."];
            return;
        }

        if (value.Length > maxLength)
        {
            errors[field] = [$"{label} must have at most {maxLength} characters."];
        }
    }
}
