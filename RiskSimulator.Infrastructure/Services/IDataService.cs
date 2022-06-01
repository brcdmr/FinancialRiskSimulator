namespace RiskSimulator.Infrastructure.Services;

public interface IDataService
{
    public Task<bool> Save<T>(string key, T data) where T:class; //upsert
    public Task<T> Get<T>(string key) where T:class;
    public Task<bool> Drop(string key);
}