using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Whatsnew.Console;

internal class WhatsNewHostedService : BackgroundService
{
    private readonly ILogger<WhatsNewHostedService>  _logger;
    private readonly GithubInfoProvider _provider;

    public WhatsNewHostedService(ILogger<WhatsNewHostedService> logger, GithubInfoProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var issue = new MonitoredItem(
            Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
            Type: "Github.Issue",
            Status: ""
        );

        var (changed, item) = await _provider.TryGetInfoAsync(issue);
        _logger.LogInformation("Changed: {Changed}", changed);
    }
}
