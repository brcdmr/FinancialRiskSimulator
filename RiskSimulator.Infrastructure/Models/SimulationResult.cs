namespace RiskSimulator.Infrastructure.Models;

public class AssesResult
{
    public string Name { get; set; }
    public List<Quantiles> Quantiles { get; set; } = new List<Quantiles>();
}

public class Quantiles
{
    public double Percentile { get; set; }
    public double Value { get; set; }
}

public class SimulationResult : BaseOperation
{
    public List<AssesResult> FinalResults { get; set; } = new List<AssesResult>();
    
}