using Flurl.Http;
using Whatsnew.Console.Github;

namespace Whatsnew.Console;

public interface IGithubProxy
{
    /// <summary>
    /// Gets github issue data json
    /// </summary>
    /// <param name="issueUrl"></param>
    /// <returns></returns>
    Task<MonitoredItem> GetIssueData(string issueUrl);
}

public class GithubProxy : IGithubProxy
{
    const string issuesUrl = "https://api.github.com/repos/maxshlain/whatsnew/issues";
  
    private readonly string _pat;
    private readonly string _userName;

    public GithubProxy(string userName, string pat)
    {
        _userName = userName;
        _pat = pat;
    }
    public async Task<MonitoredItem> GetIssueData(string issueUrl)
    {
        var gitHubIssue = await issuesUrl
            .WithHeader("User-Agent", "Flurl")
            .WithBasicAuth(_userName, _pat)
            .GetJsonAsync<GithubIssueModel[]>();

        var firstItem = gitHubIssue?.FirstOrDefault(i => i.url == issueUrl)
        ?? throw new Exception("No issues found");

        var item = new MonitoredItem
        (
            Url: firstItem.url,
            Type: "github",
            Status: firstItem.state
        );
        
        return item;
    }
}
