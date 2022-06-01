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
        string guid = Guid.NewGuid().ToString();
        var request = new SimulationRequest(){Key = guid};
        var fakeDataService = new FakeDataService();
        var fakeWorker = new FakeJobWorker();
        
        var coordinatorService = new LogicCoordinatorService(fakeDataService, fakeWorker);

        // Act
        var result = await coordinatorService.StartJob(request);
        
        // Assert
        Assert.NotNull(result.OperationResult);

    }
}