using Microsoft.Extensions.DependencyInjection;
using RadarBolsa.Application.Health;
using RadarBolsa.Application.Opportunities;
using RadarBolsa.Application.TrackedAssets;

namespace RadarBolsa.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetHealthStatusUseCase>();
        services.AddScoped<GetOpportunitiesUseCase>();
        services.AddScoped<CreateTrackedAssetUseCase>();
        services.AddScoped<GetTrackedAssetsUseCase>();
        services.AddScoped<GetTrackedAssetByTickerUseCase>();

        return services;
    }
}
