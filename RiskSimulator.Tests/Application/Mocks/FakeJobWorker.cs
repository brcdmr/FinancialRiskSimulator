using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.WebApi.Tests.Application.Mocks;

public class FakeJobWorker:IJobWorker
{
    private bool isSccess = false;
    public FakeJobWorker()
    {
        
    }

    public FakeJobWorker(bool isSuccess)
    {
        this.isSccess = isSuccess;
    }
    public Task<bool> RegisterJob(SimulationRequest data)
    {
        return Task.FromResult(this.isSccess);
    }
}