using Watchdog.Bot.Listeners;

namespace Watchdog.Bot.Events;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventManagers(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IEventManager, MessageListener>()
            .AddSingleton<IEventManager, StartupEventManager>()
            .AddSingleton<IEventManager, BasicGuildEventManager>();
    }
}