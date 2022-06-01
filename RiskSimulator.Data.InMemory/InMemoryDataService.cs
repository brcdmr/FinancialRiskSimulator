using Microsoft.Extensions.Caching.Memory;
using RiskSimulator.Infrastructure.Services;

namespace RiskSimulator.Data.InMemory;

public class InMemoryDataService : IDataService
{
    private readonly IMemoryCache _memoryCache;

    public InMemoryDataService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public Task<bool> Save<T>(string key, T data) where T : class
    {
        _memoryCache.Set(key, data);
        return Task.FromResult(true);
    }

    public Task<T> Get<T>(string key) where T : class
    {
        _memoryCache.TryGetValue(key, out T cachedResponse);
        return Task.FromResult(cachedResponse as T);
    }

    public Task<bool> Drop(string key)
    {
        _memoryCache.Remove(key);
        return Task.FromResult(true);
    }
}