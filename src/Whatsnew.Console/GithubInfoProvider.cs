using Flurl.Http;

namespace Whatsnew.Console;

public class GithubProxy : IGithubProxy
{
    private readonly string _pat;

    public GithubProxy(string pat)
    {
        _pat = pat;
    }
    public async Task<string> GetIssueData(string issueUrl)
    {
        var issuesUrl = "https://api.github.com/repos/maxshlain/whatsnew/issues";

        var person = await issuesUrl
            .WithHeader("User-Agent", "Flurl")
            .WithBasicAuth("maxshlain@gmail.com", _pat)
            .GetStringAsync();

        return person;
    }
}

public interface IGithubProxy
{
    /// <summary>
    /// Gets github issue data json
    /// </summary>
    /// <param name="issueUrl"></param>
    /// <returns></returns>
    Task<string> GetIssueData(string issueUrl);
}

public class GithubInfoProvider: IInfoProvider
{
    private readonly IGithubProxy _proxy;

    public GithubInfoProvider(IGithubProxy proxy)
    {
        _proxy = proxy;
    }
    
    public bool TryGetInfo(MonitoredItem item, out MonitoredItem updated)
    {
        throw new NotImplementedException();
    }
}

public interface IInfoProvider
{
    bool TryGetInfo(MonitoredItem item, out MonitoredItem updated);
}