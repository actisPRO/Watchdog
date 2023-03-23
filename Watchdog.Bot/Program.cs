using Watchdog.Bot;
using Watchdog.Bot.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddOptions<DatabaseOptions>().BindConfiguration(DatabaseOptions.SectionName);
        services.AddHostedService<Worker>()
            .AddDbContext<DatabaseContext>();
    })
    .Build();

host.Run();