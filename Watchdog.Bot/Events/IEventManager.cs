using DSharpPlus;

namespace Watchdog.Bot.Events;

public interface IEventManager
{
    void RegisterEvents(DiscordClient client);
}