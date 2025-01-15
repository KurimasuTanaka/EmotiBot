using DataAccess.Models;
using Microsoft.Extensions.Logging;
using Services.EmoticonsService;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

public enum Folder
{
    Root,
    Positive,
    Joy,
    Love,
    Negative,
    Dissatisfaction,
    Anger
}

public class BotUser
{
    public int userId = 0;
    public Folder currentFolder = Folder.Root;

    public BotUser(int userId)
    {
        this.userId = userId;
    }

}

public class UpdateHandler(ILogger<UpdateHandler> logger, IEmoticonsSerivice emoticonsSerivice) : IUpdateHandler
{

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogError("Handle error: {error}", exception); // just dump the exception to the console

        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.InlineQuery)
        {
            await OnInlineQuery(botClient, update.InlineQuery!);
        } else if (update.Type == UpdateType.Message && update.Message?.From.Id ==  int.Parse(Environment.GetEnvironmentVariable("AdminId")!))
        {
            await OnMessage(botClient, update.Message!);
        }
    }
    int messageCounter = 0;
    public async Task OnInlineQuery(ITelegramBotClient botClient, InlineQuery inlineQuery)
    {
        List<EmoticonModel> emoticons = await emoticonsSerivice.GetEmoticonsAsync(inlineQuery.Query);

        var results = emoticons.Select(e => new InlineQueryResultArticle(
            id: messageCounter++.ToString(),
            title: e.Emoticon,
            inputMessageContent: new InputTextMessageContent(e.Emoticon)
        )).ToList();
        

        await botClient.AnswerInlineQuery(inlineQuery.Id, results);

    }

    public async Task OnMessage(ITelegramBotClient botClient, Message message)
    {
        if (message.Text != null)
        {
            await emoticonsSerivice.AddEmoticon(message.Text);
        }
    }   
}

