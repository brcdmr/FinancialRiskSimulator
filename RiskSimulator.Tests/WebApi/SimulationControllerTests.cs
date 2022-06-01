using Microsoft.Extensions.Logging;
using Moq;
using RiskSimulator.Api.Controllers;
using RiskSimulator.Infrastructure.Models;
using RiskSimulator.Infrastructure.Models.Enums;
using RiskSimulator.WebApi.Tests.Mocks;
using Xunit;


namespace RiskSimulator.WebApi.Tests;

public class SimulationControllerTests
{
    [Fact]
    public async Task Submit_IsSucceesTrue_TaskIdShouldBePresent()
    {
        // Arrange
        string guid = Guid.NewGuid().ToString();
        var fakeCoordinator = new FakeCoordinator(true, guid);
        var request = new SimulationRequest();
        var mock = new Mock<ILogger<SimulationController>>();
        ILogger<SimulationController> logger = mock.Object;

        logger = Mock.Of<ILogger<SimulationController>>();
        
        var controller = new SimulationController(logger, fakeCoordinator);

        // Act
        var result = await controller.Submit(request);
        
        // Assert
        Assert.Null(result.Message);
        Assert.NotNull(result.TaskId);
        Assert.Equal( guid, result.TaskId);

    }
    
    [Fact]
    public async Task Submit_IsSucceesFalse_MessageShouldBePresent()
    {
        // Arrange
        string guid = Guid.NewGuid().ToString();
        var fakeCoordinator = new FakeCoordinator(false, guid);
        var request = new SimulationRequest();
        var mock = new Mock<ILogger<SimulationController>>();
        ILogger<SimulationController> logger = mock.Object;

        logger = Mock.Of<ILogger<SimulationController>>();
        
        var controller = new SimulationController(logger, fakeCoordinator);

        // Act
        var result = await controller.Submit(request);
        
        // Assert
        Assert.NotNull(result.Message);
        Assert.Null(result.TaskId);
    }
    
    [Fact]
    public async Task Query_IsSucceded_StateShouldBeShown()
    {
        // Arrange
        string message = StateType.Started.ToString();
        var fakeCoordinator = new FakeCoordinator(true, message);
        var request = new SimulationQuery();
        var mock = new Mock<ILogger<SimulationController>>();
        ILogger<SimulationController> logger = mock.Object;

        logger = Mock.Of<ILogger<SimulationController>>();
        
        var controller = new SimulationController(logger, fakeCoordinator);

        // Act
        var result = await controller.Query(request);
        
        // Assert
        Assert.Null(result.Message);
        Assert.NotNull(result.TaskState);
        Assert.Equal(message, result.TaskState);
    }
    
    [Fact]
    public async Task Query_Failed_NoStateToDisplay()
    {
        // Arrange
        string message = StateType.Started.ToString();
        var fakeCoordinator = new FakeCoordinator(false, message);
        var request = new SimulationQuery();
        var mock = new Mock<ILogger<SimulationController>>();
        ILogger<SimulationController> logger = mock.Object;

        logger = Mock.Of<ILogger<SimulationController>>();
        
        var controller = new SimulationController(logger, fakeCoordinator);

        // Act
        var result = await controller.Query(request);
        
        // Assert
        Assert.NotNull(result.Message);
        Assert.Null(result.TaskState);
       
    }
    [Fact]
    public async Task FinalResult_IsSuccessFlagTrue_RiskSimulatorResultShouldBePresent()
    {
        // Arrange
        string message = StateType.Finished.ToString();
        var fakeAssertResult = GetFakeRiskSimulatorResult();
        var fakeCoordinator = new FakeCoordinator(true, message, fakeAssertResult);
        var request = new SimulationQuery();
        
        var mock = new Mock<ILogger<SimulationController>>();
        ILogger<SimulationController> logger = mock.Object;

        logger = Mock.Of<ILogger<SimulationController>>();
        
        var controller = new SimulationController(logger, fakeCoordinator);

        // Act
        var result = await controller.FinalResult(request);
        
        // Assert
        Assert.NotNull(result.RiskSimulatorResult);
        Assert.Null(result.Message);
        Assert.Equal(fakeAssertResult.Count(),result.RiskSimulatorResult.Count());
   
    }

    [Fact]
    public async Task FinalResult_IsSuccessFlagFalse_NoDataPresent()
    {
        // Arrange
        string message = StateType.Finished.ToString();
        var fakeAssertResult = GetFakeRiskSimulatorResult();
        var fakeCoordinator = new FakeCoordinator(false, message, fakeAssertResult);
        var request = new SimulationQuery();
        
        var mock = new Mock<ILogger<SimulationController>>();
        ILogger<SimulationController> logger = mock.Object;

        logger = Mock.Of<ILogger<SimulationController>>();
        
        var controller = new SimulationController(logger, fakeCoordinator);

        // Act
        var result = await controller.FinalResult(request);
        
        // Assert
        Assert.Null(result.RiskSimulatorResult);
        Assert.NotNull(result.Message);

    }
    public List<AssesResult> GetFakeRiskSimulatorResult()
    {
        var result = new List<AssesResult>();
        result.Add(new AssesResult(){Name="asset1", Quantiles = new List<Quantiles>(){new Quantiles(){Percentile = 1.1,Value = 5.1}}});
        result.Add(new AssesResult(){Name="asset2", Quantiles = new List<Quantiles>(){new Quantiles(){Percentile = 2.1,Value = 8.1}}});
        
        return result;
    }
}