using Microsoft.Extensions.DependencyInjection;
using RadarBolsa.Application.Health;
using RadarBolsa.Application.Opportunities;

namespace RadarBolsa.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetHealthStatusUseCase>();
        services.AddScoped<GetOpportunitiesUseCase>();

        return services;
    }
}
