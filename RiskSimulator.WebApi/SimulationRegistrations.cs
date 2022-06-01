using RiskSimulator.Application;
using RiskSimulator.Application.Logging;
using RiskSimulator.Data.InMemory;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.Api;

public static class SimulationRegistrations
{
    public static IServiceCollection AddSimulationServices(
        this IServiceCollection services)
    {
        services.AddInMemoryCache();
        services.AddSingleton<IDataService, InMemoryDataService>();
        services.AddSingleton<IJobWorker, JobWorker>();
        services.AddSingleton<ICalculator, MonteCarloCalculatorService>();
        services.AddSingleton<ILogicCoordinator, LogicCoordinatorService>();
        
        return services;
    }

    public static IApplicationBuilder UseSimulation(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<LoggingMiddleWare>();
        return app;
    }
    
}