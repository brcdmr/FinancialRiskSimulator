namespace RiskSimulator.Infrastructure.Models;

public class AssesResult
{
    public string Name { get; set; }
   // public List<double> SimulatedValues { get; set; } = new List<double>();
    public List<double> OverOnePercent { get; set; }
    public List<double> OverFivePercent { get; set; }
}

public class SimulationResult : BaseOperation
{
    public List<AssesResult> FinalResults { get; set; } = new List<AssesResult>();
    
}