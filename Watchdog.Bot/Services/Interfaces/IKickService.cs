using DSharpPlus.Entities;

namespace Watchdog.Bot.Services.Interfaces;

public interface IKickService
{
    Task KickMemberAsync(DiscordMember member, DiscordUser moderator, string reason);
}