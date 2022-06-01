using RiskSimulator.Infrastructure.Models;

namespace RiskSimulator.Infrastructure.Services;

public interface IJobWorker
{
    public Task<bool> RegisterJob(SimulationRequest data);
}