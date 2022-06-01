using Microsoft.Extensions.Logging;
using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Models.Enums;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.Application;

public class JobBag
{
    public IDataService DataService { get; set; }
    public ICalculator CalculatorService { get; set; }
    public SimulationRequest Data { get; set; }
    public ILogger<JobWorker> LoggerService { get; set; }
}

public class JobWorker : IJobWorker
{
    private readonly IDataService _dataService;
    private readonly ICalculator _calculatorService;
    private readonly ILogger<JobWorker> _logger;

    public JobWorker(IDataService dataService, ICalculator calculator, ILogger<JobWorker> logger)
    {
        _dataService = dataService;
        _calculatorService = calculator;
        _logger = logger;
    }

    public async Task<bool> RegisterJob(SimulationRequest data)
    {
        var bag = new JobBag()
        {
            DataService = _dataService,
            CalculatorService = _calculatorService,
            LoggerService = _logger,
            Data = data
        };

        ThreadPool.QueueUserWorkItem(BackgroundTask, bag);
        return await Task.FromResult(true);
    }

    private void BackgroundTask(object? state)
    {
        var bag = state as JobBag;
        if (bag != null)
        {
            var tempTask = BackgroundTaskAsync(bag);
            Task.WhenAll(tempTask);
        }
    }

    private async Task<bool> BackgroundTaskAsync(JobBag bag)
    {
        try
        {
            var calculationResult = await bag.CalculatorService.Calculate(bag.Data);

            var state = await bag.DataService.Get<StateBag>(bag.Data.Key);

            state.EndTime = DateTime.UtcNow;
            state.Result = calculationResult;
            state.State = StateType.Finished;

            return await bag.DataService.Save(bag.Data.Key, state);
        }
        catch (Exception exception)
        {
            bag.LoggerService.LogError(exception.Message);
        }

        return false;
    }
}