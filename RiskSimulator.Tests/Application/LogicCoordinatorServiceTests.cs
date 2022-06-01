using RiskSimulator.Application;
using RiskSimulator.Infrastructure.Models;
using RiskSimulator.WebApi.Tests.Application.Mocks;
using Xunit;

namespace RiskSimulator.WebApi.Tests.Application;

public class LogicCoordinatorServiceTests
{
    [Fact]
    public async Task StartJob_IsSuccessTrue_OperationResultShouldBePresent()
    {
        // Arrange
        var request = new SimulationRequest(){};
        var fakeDataService = new FakeDataService();
        var fakeWorker = new FakeJobWorker(true);
        
        var coordinatorService = new LogicCoordinatorService(fakeDataService, fakeWorker);

        // Act
        var result = await coordinatorService.StartJob(request);
        
        // Assert
        Assert.NotNull(result.OperationResult);
        Assert.DoesNotContain("Error", result.OperationResult);
    }
    
    [Fact]
    public async Task StartJob_IsSuccessFalse_OperationResultShouldContainsError()
    {
        // Arrange
        var request = new SimulationRequest(){};
        var fakeDataService = new FakeDataService();
        var fakeWorker = new FakeJobWorker(false);
        
        var coordinatorService = new LogicCoordinatorService(fakeDataService, fakeWorker);

        // Act
        var result = await coordinatorService.StartJob(request);
        
        // Assert
        Assert.NotNull(result.OperationResult);
        Assert.Contains("Error", result.OperationResult);

    }
    
    [Fact]
    public async Task QueryState_IsSuccessFalse_StateShouldBeShown()
    {
        // Arrange
        var fakeDataService = new FakeDataService();
        var fakeWorker = new FakeJobWorker(true);
        string key = Guid.NewGuid().ToString();
        var expectedData = new StateBag();

        await fakeDataService.Save(key, expectedData);
        var coordinatorService = new LogicCoordinatorService(fakeDataService, fakeWorker);

        // Act
        var result = await coordinatorService.QueryState(key);
        
        // Assert
        Assert.NotNull(result.State.ToString());
        Assert.Null(result.OperationResult);

    }
    
    [Fact]
    public async Task QueryState_IsSuccessFalse_OperationResultShouldBeShown()
    {
        // Arrange
        var fakeDataService = new FakeDataService();
        var fakeWorker = new FakeJobWorker();
        string key = Guid.NewGuid().ToString();

        var coordinatorService = new LogicCoordinatorService(fakeDataService, fakeWorker);

        // Act
        var result = await coordinatorService.QueryState(key);
        
        // Assert
        Assert.NotNull(result.OperationResult);
        Assert.Contains("Error", result.OperationResult);

    }

    [Fact]
    public async Task FindResult_StateNotFinished_FinalResultShouldBeShown()
    {
        // Arrange
        var fakeDataService = new FakeDataService();
        var fakeWorker = new FakeJobWorker(true);
        string key = Guid.NewGuid().ToString();
        var expectedData = new StateBag();

        await fakeDataService.Save(key, expectedData);
        var coordinatorService = new LogicCoordinatorService(fakeDataService, fakeWorker);

        // Act
        var result = await coordinatorService.FindResult(key);
        
        // Assert
        Assert.Empty(result.FinalResults);
        Assert.NotNull(result.OperationResult);
        Assert.Contains("Simulation", result.OperationResult);
        

    }
    
    
}