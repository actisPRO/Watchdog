namespace Watchdog.Bot.Events;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventManagers(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IEventManager, MessageEventManager>()
            .AddSingleton<IEventManager, StartupEventManager>()
            .AddSingleton<IEventManager, BasicGuildEventManager>()
            .AddSingleton<IEventManager, CommandEventManager>();
    }
}