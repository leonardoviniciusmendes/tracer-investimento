using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Domain.Opportunities;

namespace RadarBolsa.Application.Opportunities;

public sealed class GetOpportunitiesUseCase(
    IOpportunityReadRepository opportunityReadRepository)
{
    public Task<IReadOnlyList<Opportunity>> ExecuteAsync(
        OpportunityFilters filters,
        CancellationToken cancellationToken) =>
        opportunityReadRepository.ListAsync(filters, cancellationToken);
}
