using Microsoft.Extensions.Logging;
using StudyBot.Abstract;
using Telegram.Bot;
using Telegram.Bot.Polling;

public class ReceiverService : IReceiverService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<ReceiverService> _logger;

    public ReceiverService(ITelegramBotClient telegramBotClient, 
                                IUpdateHandler updateHandler, 
                                ILogger<ReceiverService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    public async Task ReceiveAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = [],
            DropPendingUpdates = true,
        };

        var me = await _telegramBotClient.GetMe(cancellationToken);
        _logger.LogInformation("Start receiving updates for {BotName}", me.Username);

        await _telegramBotClient.ReceiveAsync(
            updateHandler: _updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken);
    }
}