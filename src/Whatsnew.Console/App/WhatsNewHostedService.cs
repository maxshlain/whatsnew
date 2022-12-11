using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class WhatsNewHostedService : BackgroundService
{
    private WhatsNewPollingService _service;

    public WhatsNewHostedService(
        ILogger<WhatsNewHostedService> logger, 
        WhatsNewPollingService service)
    {
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(true)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            await _service.PollAsync();

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            await _service.Delay();
        }
    }
}
