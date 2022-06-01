using Microsoft.Extensions.DependencyInjection;

namespace RiskSimulator.Data.InMemory;

public static class DependencyInjection
{
    public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return services;
    }
}