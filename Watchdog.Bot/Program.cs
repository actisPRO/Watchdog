using Microsoft.EntityFrameworkCore;
using Watchdog.Bot;
using Watchdog.Bot.Events;
using Watchdog.Bot.Mapping;
using Watchdog.Bot.Options;
using Watchdog.Bot.Repositories;
using Watchdog.Bot.Services;

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
            .AddAutoMapper(typeof(AutoMapperProfile))
            .AddSingleton<DiscordBotClient>()
            .AddSingleton<ParameterInitializer>()
            .AddEventManagers()
            .AddDbContext<DbContext, DatabaseContext>()
            .AddRepositories()
            .AddServices();
    })
    .Build();

host.Run();