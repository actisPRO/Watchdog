using DSharpPlus.Entities;

namespace Watchdog.Bot.Services.Interfaces;

public interface IGuildService
{
    Task CreateOrUpdateGuildAsync(DiscordGuild guild);
}