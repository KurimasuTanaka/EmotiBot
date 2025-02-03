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


IHost builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, config) =>
{
    config.AddEnvironmentVariables();
    config.AddUserSecrets<Program>();

}).ConfigureServices((context, services) =>
{
    string DbConnectionString = "";

    services.AddHttpClient("telegram_bot_client").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
        DbConnectionString = configuration["BotSettings:DbConnectionString"]!;

        TelegramBotClientOptions options = new(configuration["BotSettings:BotToken"]!);
        return new TelegramBotClient(options, httpClient);
    });

    services.AddScoped<IUpdateHandler, UpdateHandler>();
    services.AddScoped<IReceiverService, ReceiverService>();
    services.AddScoped<IEmoticonsSerivice, EmoticonsSerivice>();

   services.AddHostedService<PollingService>();

    var serverVersion = new MySqlServerVersion(new Version(8, 0, 41));
    services.AddDbContext<EmoticonsDbContext>(options =>
    options.UseMySQL(DbConnectionString));

    services.AddScoped<IDataAccess, DataAccess.DataAccess.DataAccess>();

}).Build();

await builder.RunAsync();