using DSharpPlus;

namespace Watchdog.Bot.Services.Interfaces;

public interface IBotStatus
{
    Task UpdateBotStatusAsync(DiscordClient client, bool updateCache = false);
}