using DataAccess.Context;
using DataAccess.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Services.EmoticonsService;
using StudyBot.Abstract;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


IHost builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(  (context, config)=>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddEnvironmentVariables();
}).ConfigureServices( (context, services) =>
{
    services.Configure<BotSettings>(context.Configuration.GetSection("BotSettings"));

    services.AddHttpClient("telegram_bot_client").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        BotSettings? botSettings = sp.GetService<IOptions<BotSettings>>()?.Value;

        Environment.SetEnvironmentVariable("AdminId", botSettings!.AdminId.ToString());

        if (botSettings is null)
        {
            throw new InvalidOperationException("Bot settings not found");
        } else if (botSettings.BotToken is null)
        {
            throw new InvalidOperationException("Bot token not found");
        }

        TelegramBotClientOptions options = new(botSettings.BotToken);
        return new TelegramBotClient(options, httpClient);
    });

    services.AddScoped<IUpdateHandler, UpdateHandler>();
    services.AddScoped<IReceiverService, ReceiverService>();
    services.AddScoped<IEmoticonsSerivice, EmoticonsSerivice>();
    services.AddHostedService<PollingService>();

    services.AddDbContext<EmoticonsDbContext>();
    services.AddScoped<IDataAccess, DataAccess.DataAccess.DataAccess>();

}).Build();


await builder.RunAsync();