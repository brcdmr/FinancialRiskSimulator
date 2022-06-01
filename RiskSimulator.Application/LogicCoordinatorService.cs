using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Models.Enums;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.Application;

public class StateBag
{
    public string Key { get; set; }
    public SimulationResult Result { get; set; }
    public StateType State { get; set; } = StateType.Started;
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public DateTime EndTime { get; set; }
}

public class LogicCoordinatorService : ILogicCoordinator
{
    private readonly IDataService _dataService;
    private readonly IJobWorker _jobWorker;

    public LogicCoordinatorService(IDataService dataService, IJobWorker worker)
    {
        _dataService = dataService;
        _jobWorker = worker;
    }

    public async Task<BaseOperation> StartJob(SimulationRequest data)
    {
        // validation eklenecek

        string key = Guid.NewGuid().ToString();
        data.Key = key;

        bool isStarted = await _jobWorker.RegisterJob(data);
        if (isStarted)
        {
            var saveResult = await _dataService.Save(key, new StateBag() {Key = key});
            return new BaseOperation() {IsSuccess = true, OperationResult = key};
        }

        return new BaseOperation() {IsSuccess = false, OperationResult = "Error ! Job is not started"};
    }

    public async Task<SimulationState> QueryState(string key)
    {
        var stateResult = await _dataService.Get<StateBag>(key);

        return stateResult == null
            ? new SimulationState() {IsSuccess = false, OperationResult = "no such key found ..."}
            : new SimulationState() {IsSuccess = true, State = stateResult.State};
    }

    public async Task<SimulationResult> FindResult(string key)
    {
        var stateResult = await _dataService.Get<StateBag>(key);

        if (stateResult == null) 
            return  new SimulationResult() {IsSuccess = false, OperationResult = "no such key found ..."};
             
        if (stateResult.State != StateType.Finished) 
            return  new SimulationResult() {IsSuccess = false, OperationResult = "task not finished ..."};

        // It can use for drop unneccessary data after calculation 
        //await _dataService.Drop(key);
        return new SimulationResult() {IsSuccess = true, FinalResults = stateResult.Result.FinalResults};
    }
}