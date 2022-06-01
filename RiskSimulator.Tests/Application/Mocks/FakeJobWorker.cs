using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.WebApi.Tests.Application.Mocks;

public class FakeJobWorker:IJobWorker
{
    public Task<bool> RegisterJob(SimulationRequest data)
    {
        return Task.FromResult(true);
    }
}