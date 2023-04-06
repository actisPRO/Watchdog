using DSharpPlus;
using DSharpPlus.Entities;
using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Services.Interfaces;

public interface IWarningService
{
    Task<int> WarnMemberAsync(WarningData warningData);

    Task<(bool foundWarning, int count)> RemoveWarningAsync(DiscordMember user, string warningId, DiscordGuild guild, DiscordMember moderator);
}