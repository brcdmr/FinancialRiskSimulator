using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.WebApi.Tests.Application.Mocks;

public class FakeDataService :IDataService
{
    private object storedData;
    public async Task<bool> Save<T>(string key, T data) where T : class
    {
        storedData = data;
        return await Task.FromResult(true);
    }

    public async Task<T> Get<T>(string key) where T : class
    {
        return await Task.FromResult((T)storedData);
    }

    public async Task<bool> Drop(string key)
    {
        storedData = null;
        return await Task.FromResult(true);
    }
}