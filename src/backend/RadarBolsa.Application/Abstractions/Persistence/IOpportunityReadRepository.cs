using RadarBolsa.Application.Opportunities;
using RadarBolsa.Domain.Opportunities;

namespace RadarBolsa.Application.Abstractions.Persistence;

public interface IOpportunityReadRepository
{
    Task<IReadOnlyList<Opportunity>> ListAsync(
        OpportunityFilters filters,
        CancellationToken cancellationToken);
}
