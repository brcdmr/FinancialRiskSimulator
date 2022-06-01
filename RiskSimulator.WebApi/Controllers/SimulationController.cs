using Microsoft.AspNetCore.Mvc;
using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Models.Response;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.Api.Controllers;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("[controller]/[action]")]
public class SimulationController : ControllerBase
{
    private readonly ILogger<SimulationController> _logger;
    private readonly ILogicCoordinator _coordinator;

    public SimulationController(ILogger<SimulationController> logger, ILogicCoordinator coordinator)
    {
        _logger = logger;
        _coordinator = coordinator;
    }
    
    [HttpPost]
    public async Task<SubmitResponse> Submit([FromBody] SimulationRequest request)
    {
     
        var result =  await _coordinator.StartJob(request);
        return result.IsSuccess ? 
            new SubmitResponse() {TaskId = result.OperationResult}
            : new SubmitResponse() {Message = result.OperationResult};

    }
    
    [HttpPost]
    public async Task<QueryResponse> Query([FromBody] SimulationQuery query)
    {
        var result =  await _coordinator.QueryState(query.TaskId);
        if (!result.IsSuccess) return new QueryResponse() {Message = result.OperationResult};

        return new QueryResponse() {TaskState = result.State.ToString()};
    }
    
    [HttpPost]
    public async Task<FinalResponse> FinalResult([FromBody] SimulationQuery query)
    {
        var result =  await _coordinator.FindResult(query.TaskId);
        if (!result.IsSuccess) return new FinalResponse() {Message = result.OperationResult};

        return new FinalResponse() { RiskSimulatorResult = result.FinalResults}; 
    }

}