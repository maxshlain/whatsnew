using JsonFlatFileDataStore;
using Whatsnew.Console;

public interface IMonitoredItemsRepository
{
    List<MonitoredItem> GetAll(string collectionName);
}

public class MonitoredItemsRepository : IMonitoredItemsRepository
{
    private readonly DataStore _store;

    public MonitoredItemsRepository()
    {
        _store = new DataStore("data.json");
    }

    public List<MonitoredItem> GetAll(string collectionName)
    {
        return _store
        .GetCollection<MonitoredItem>(collectionName)
        .AsQueryable()
        .ToList();
    }
}
