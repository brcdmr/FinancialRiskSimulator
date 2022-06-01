using RiskSimulator.Infrastructure.Models;

namespace RiskSimulator.Infrastructure.Services;

public interface ICalculator
{
    public Task<SimulationResult> Calculate(SimulationRequest data);
}