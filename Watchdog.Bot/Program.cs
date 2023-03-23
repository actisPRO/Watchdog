using Watchdog.Bot;
using Watchdog.Bot.Events;
using Watchdog.Bot.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.local.json", optional: true);
    })
    .ConfigureServices(services =>
    {
        services.AddOptions<DatabaseOptions>().BindConfiguration(DatabaseOptions.SectionName);
        services.AddOptions<DiscordOptions>().BindConfiguration(DiscordOptions.SectionName);

        services.AddHostedService<Worker>()
            .AddSingleton<DiscordBotClient>()
            .AddEventManagers()
            .AddDbContext<DatabaseContext>();
    })
    .Build();

host.Run();