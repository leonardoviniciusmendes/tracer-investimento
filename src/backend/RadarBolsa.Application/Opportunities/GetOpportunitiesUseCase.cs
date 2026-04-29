using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Domain.Opportunities;

namespace RadarBolsa.Application.Opportunities;

public sealed class GetOpportunitiesUseCase(
    IOpportunityReadRepository opportunityReadRepository)
{
    public Task<IReadOnlyList<Opportunity>> ExecuteAsync(
        int? minScore,
        string? sector,
        CancellationToken cancellationToken)
    {
        var filters = new OpportunityFilters(
            minScore,
            string.IsNullOrWhiteSpace(sector) ? null : sector.Trim());

        return opportunityReadRepository.ListAsync(filters, cancellationToken);
    }
}
