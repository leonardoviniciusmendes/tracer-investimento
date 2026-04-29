using RadarBolsa.Api.Contracts;
using RadarBolsa.Domain.Opportunities;

namespace RadarBolsa.Api.Mappings;

internal static class ApiContractMappings
{
    public static OpportunityResponse ToResponse(this Opportunity opportunity) =>
        new(
            opportunity.Ticker,
            opportunity.CompanyName,
            opportunity.Sector,
            opportunity.CurrentPrice,
            opportunity.TargetPrice,
            opportunity.Score,
            opportunity.Thesis,
            opportunity.CapturedAt);
}
