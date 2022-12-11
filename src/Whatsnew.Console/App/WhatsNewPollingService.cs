using Microsoft.Extensions.Logging;
using Whatsnew.Console;

internal record WhatsNewPollingServiceSettings(
    int PollingDelayInMilliSeconds
);

internal class WhatsNewPollingService
{
    private readonly ILogger<WhatsNewPollingService> _logger;
    private readonly GithubInfoProvider _provider;
    private readonly WhatsNewPollingServiceSettings _settings;

    public WhatsNewPollingService(
        ILogger<WhatsNewPollingService> logger, 
        GithubInfoProvider provider,
        WhatsNewPollingServiceSettings settings)
    {
        _logger = logger;
        _provider = provider;
        _settings = settings;
    }

    public async Task PollAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
        {
            return;
        }

        var issue = new MonitoredItem(
            Url: "https://api.github.com/repos/maxshlain/whatsnew/issues/1",
            Type: "Github.Issue",
            Status: ""
        );

        var (changed, item) = await _provider.TryGetInfoAsync(issue);
        _logger.LogInformation("Changed: {Changed}", changed);
    }

    public Task Delay(CancellationToken stoppingToken)
    {
        return Task.Delay(
            _settings.PollingDelayInMilliSeconds, 
            stoppingToken
        );
    }
}
