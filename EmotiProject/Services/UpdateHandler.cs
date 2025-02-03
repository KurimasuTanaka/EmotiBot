using DataAccess.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.EmoticonsService;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

public class UpdateHandler(ILogger<UpdateHandler> logger, IEmoticonsSerivice emoticonsSerivice, IConfiguration configuration) : IUpdateHandler
{


    //Error handler for the bot
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogError("Handle error: {error}", exception); // just dump the exception to the console

        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //Regular inline query with emoticon tag or without it 
        if (update.Type == UpdateType.InlineQuery)
        {
            await OnInlineQuery(botClient, update.InlineQuery!);
        }
        //Message from the admin account with new emoticons
        else if (update.Type == UpdateType.Message && update.Message is not null)
        {
            if(update.Message.From!.Id == configuration.GetValue<int>("BotSettings:AdminId")) await OnMessage(botClient, update.Message!);
            else await botClient.SendMessage(update.Message.Chat.Id, "You are not allowed to add emoticons");
        }
    }
    int messageCounter = 0; // Counter fro uniqe message id

    //Recive inline query from the user and send appropriate emoticon list
    public async Task OnInlineQuery(ITelegramBotClient botClient, InlineQuery inlineQuery)
    {
        List<EmoticonModel> emoticons = await emoticonsSerivice.GetEmoticonsAsync(inlineQuery.Query);

        var results = emoticons.Select(e => new InlineQueryResultArticle(
            id: messageCounter++.ToString(),
            title: e.Emoticon,
            inputMessageContent: new InputTextMessageContent(e.Emoticon)
        )).ToList();

        //Sending the list of emoticons to the user
        try
        {
            await botClient.AnswerInlineQuery(inlineQuery.Id, results);
        }
        catch (Exception e)
        {
            logger.LogError("Error sending inline query results: {error}", e);
        }

    }

    //Recive messages from the admin account with new emoticons 
    public async Task OnMessage(ITelegramBotClient botClient, Message message)
    {
        if (message.Text != null)
        {
            try
            {
                await emoticonsSerivice.AddEmoticon(message.Text);
            }
            catch (Exception e)
            {
                logger.LogError("Error adding emoticon: {error}", e);
            }
        }
    }
}

