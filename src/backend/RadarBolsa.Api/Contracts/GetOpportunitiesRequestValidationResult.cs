using RadarBolsa.Application.Opportunities;

namespace RadarBolsa.Api.Contracts;

internal sealed record GetOpportunitiesRequestValidationResult(
    bool IsValid,
    OpportunityFilters? Filters,
    IReadOnlyDictionary<string, string[]> Errors)
{
    public static GetOpportunitiesRequestValidationResult Success(
        OpportunityFilters filters) =>
        new(true, filters, EmptyErrors);

    public static GetOpportunitiesRequestValidationResult Failure(
        IReadOnlyDictionary<string, string[]> errors) =>
        new(false, null, errors);

    private static IReadOnlyDictionary<string, string[]> EmptyErrors { get; } =
        new Dictionary<string, string[]>();
}
