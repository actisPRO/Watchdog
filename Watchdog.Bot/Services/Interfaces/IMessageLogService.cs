using DSharpPlus.Entities;

namespace Watchdog.Bot.Services.Interfaces;

public interface IMessageLogService
{
    Task LogDeletedMessage(DiscordGuild guild, DiscordMessage message, ulong messageLogsChannelId);
    Task LogUpdatedMessage(DiscordGuild guild, DiscordMessage messageBefore, DiscordMessage message, ulong messageLogsChannelId);
    Task LogBulkDeletedMessages(DiscordGuild guild, IReadOnlyList<DiscordMessage> messages, ulong messageLogsChannelId);
}