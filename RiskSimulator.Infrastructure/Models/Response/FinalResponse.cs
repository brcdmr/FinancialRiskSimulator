namespace RiskSimulator.Infrastructure.Models.Response;

public class FinalResponse :BaseResponse
{
    public List<AssesResult> RiskSimulatorResult { get; set; }
}