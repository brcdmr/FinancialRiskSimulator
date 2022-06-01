using System.ComponentModel;
using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Models.Enums;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.WebApi.Tests.Mocks;

public class FakeCoordinator :ILogicCoordinator
{
    private bool isSccess = false;
    private string tskId = "";
    private List<AssesResult> assrtResult;
    
    public FakeCoordinator()
    {
        
    }

    public FakeCoordinator(bool isSuccess, string taskId)
    {
        this.isSccess = isSuccess;
        this.tskId = taskId;
    }
    public FakeCoordinator(bool isSuccess, string taskId,List<AssesResult> assertResult)
    {
        this.isSccess = isSuccess;
        this.tskId = taskId;
        this.assrtResult = assertResult;
    }
    public async Task<BaseOperation> StartJob(SimulationRequest data)
    {
        return await Task.FromResult(new BaseOperation() {IsSuccess = isSccess, OperationResult = tskId});
    }

    public async Task<SimulationState> QueryState(string key)
    {
        return await Task.FromResult(new SimulationState() {IsSuccess = isSccess, State = StateType.Started, OperationResult = tskId});
    }

    public async Task<SimulationResult> FindResult(string key)
    {
        return await Task.FromResult(new SimulationResult() {IsSuccess = isSccess, FinalResults = assrtResult, OperationResult = tskId});

    }
    
    
}