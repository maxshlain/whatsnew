using Microsoft.Extensions.Logging;

namespace Whatsnew.Console;

public interface IInfoProvider
{
    Task<(bool, MonitoredItem)> TryGetInfoAsync(MonitoredItem item);
}

public class GithubInfoProvider: IInfoProvider
{
    private readonly ILogger<GithubInfoProvider> _logger;
    private readonly IGithubProxy _proxy;

    public GithubInfoProvider(ILogger<GithubInfoProvider> logger, IGithubProxy proxy)
    {
        _logger = logger;
        _proxy = proxy;
    }
    
    public async Task<(bool, MonitoredItem)> TryGetInfoAsync(MonitoredItem item)
    {
        var fromSource = await _proxy.GetIssueData(item.Url);
        _logger.LogInformation("{FromSource}", fromSource);
        if (fromSource.Status != item.Status)
        {
            return (true, fromSource);
        }

        return (false, item);
    }
}
