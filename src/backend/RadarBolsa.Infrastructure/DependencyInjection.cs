using Microsoft.Extensions.DependencyInjection;
using RadarBolsa.Application.Abstractions.Health;
using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Infrastructure.Health;
using RadarBolsa.Infrastructure.Persistence;
using RadarBolsa.Infrastructure.Persistence.Opportunities;

namespace RadarBolsa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IRadarBolsaDbConnectionFactory, RadarBolsaDbConnectionFactory>();
        services.AddSingleton<IDatabaseHealthChecker, DatabaseHealthChecker>();
        services.AddScoped<IOpportunityReadRepository, MySqlOpportunityReadRepository>();

        return services;
    }
}
