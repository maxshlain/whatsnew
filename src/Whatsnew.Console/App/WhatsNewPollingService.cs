using JsonFlatFileDataStore;
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
    private readonly IMonitoredItemsRepository _repository;

    public WhatsNewPollingService(
        ILogger<WhatsNewPollingService> logger,
        WhatsNewPollingServiceSettings settings,
        GithubInfoProvider provider,
        IMonitoredItemsRepository repository)
    {
        _logger = logger;
        _settings = settings;
        _provider = provider;
        _repository = repository;
    }

    public async Task PollAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
        {
            return;
        }

        await PollGitHub(stoppingToken);
    }

    private async Task PollGitHub(CancellationToken stoppingToken)
    {
        var issues = _repository.GetAll("gitHubIssues");

        foreach (var issue in issues)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            var (changed, item) = await _provider.TryGetInfoAsync(issue);
            _logger.LogInformation("Changed: {Changed}", changed);
        }
    }

    public Task Delay(CancellationToken stoppingToken)
    {
        return Task.Delay(
            _settings.PollingDelayInMilliSeconds,
            stoppingToken
        );
    }
}
