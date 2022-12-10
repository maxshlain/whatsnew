namespace Whatsnew.Console;

public interface IInfoProvider
{
    Task<(bool, MonitoredItem)> TryGetInfoAsync(MonitoredItem item);
}

public class GithubInfoProvider: IInfoProvider
{
    private readonly IGithubProxy _proxy;

    public GithubInfoProvider(IGithubProxy proxy)
    {
        _proxy = proxy;
    }
    
    public async Task<(bool, MonitoredItem)> TryGetInfoAsync(MonitoredItem item)
    {
        var fromSource = await _proxy.GetIssueData(item.Url);
        if (fromSource.Status != item.Status)
        {
            return (true, fromSource);
        }

        return (false, item);
    }
}
