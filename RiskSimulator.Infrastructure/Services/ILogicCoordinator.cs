using RiskSimulator.Infrastructure.Models;

namespace RiskSimulator.Infrastructure.Services;

public interface ILogicCoordinator
{
    public Task<BaseOperation> StartJob(SimulationRequest data);
    public Task<SimulationState> QueryState(string key);
    public Task<SimulationResult> FindResult(string key);
    
}