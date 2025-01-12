using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

public class UpdateHandler(ILogger<UpdateHandler> logger) : IUpdateHandler
{

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogError("Handle error: {error}", exception); // just dump the exception to the console

        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(update.Type == UpdateType.InlineQuery)
        {
            await OnInlineQuery(botClient, update.InlineQuery!);   
        }
    }

    public async Task OnInlineQuery(ITelegramBotClient botClient, InlineQuery inlineQuery)
    {
        var results = new InlineQueryResult[]
        {
            new InlineQueryResultArticle(
                id: "1",
                title: "Hello",
                inputMessageContent: new InputTextMessageContent("Hello World!")
            )
        };

        await botClient.AnswerInlineQuery(inlineQuery.Id, results, cancellationToken: default);
    }
}

