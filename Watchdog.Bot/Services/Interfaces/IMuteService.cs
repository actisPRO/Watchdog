using DSharpPlus.Entities;

namespace Watchdog.Bot.Services.Interfaces;

public interface IMuteService
{
    Task MuteMemberAsync(DiscordMember member, DiscordUser moderator, TimeSpan duration, string reason);
    
    Task UnmuteMemberAsync(DiscordMember member, DiscordUser moderator, string reason);
}