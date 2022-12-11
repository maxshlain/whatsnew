using Flurl.Http;
using Whatsnew.Console.Github;

namespace Whatsnew.Console;

public interface IGithubProxy
{
    Task<MonitoredItem> GetIssueData(string issueUrl);
}

public record GithubProxySettings(string UserName, string Pat);

public class GithubProxy : IGithubProxy
{
    const string issuesUrl = "https://api.github.com/repos/maxshlain/whatsnew/issues";
    private readonly GithubProxySettings _settings;

    public GithubProxy(GithubProxySettings settings)
    {
        _settings = settings;
    }

    public async Task<MonitoredItem> GetIssueData(string issueUrl)
    {
        var gitHubIssue = await issuesUrl
            .WithHeader("User-Agent", "Flurl")
            .WithBasicAuth(_settings.UserName, _settings.Pat)
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
