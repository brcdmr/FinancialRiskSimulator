namespace RiskSimulator.Infrastructure.Models;

public class AssesResult
{
    public string Name { get; set; }
    public double OverOnePercent { get; set; }
    public double OverFivePercent { get; set; }
}

public class SimulationResult : BaseOperation
{
    public List<AssesResult> FinalResults { get; set; } = new List<AssesResult>();
    
}