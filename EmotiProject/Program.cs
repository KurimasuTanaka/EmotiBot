using DataAccess.Context;
using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Services.EmoticonsService;
using StudyBot.Abstract;
using Telegram.Bot;
using Telegram.Bot.Polling;


IHost builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(  (context, config)=>
{
    config.AddEnvironmentVariables();
    config.AddUserSecrets<Program>();

}).ConfigureServices( (context, services) =>
{
    services.AddHttpClient("telegram_bot_client").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
        TelegramBotClientOptions options = new(configuration["BotSettings:BotToken"]!);
        return new TelegramBotClient(options, httpClient);
    });

    services.AddScoped<IUpdateHandler, UpdateHandler>();
    services.AddScoped<IReceiverService, ReceiverService>();
    services.AddScoped<IEmoticonsSerivice, EmoticonsSerivice>();
    services.AddHostedService<PollingService>();

    services.AddDbContext<EmoticonsDbContext>(optionBuilder =>
        {
            string connectionString = context.Configuration["BotSettings:DbConnectionString"]!;
            optionBuilder.UseSqlite(connectionString);
        });
    services.AddScoped<IDataAccess, DataAccess.DataAccess.DataAccess>();

}).Build();

await builder.RunAsync();