
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudyBot.Abstract;

public class PollingService : BackgroundService
{
    private IReceiverService _receiverService;
    private ILogger _logger;

    public PollingService(IReceiverService receiverService, ILogger<PollingService> logger)
    {
        _receiverService = receiverService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting polling service");
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Attempting to receive updates");
                await _receiverService.ReceiveAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Polling failed with exception: {e}", e);
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}