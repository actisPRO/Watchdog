using DSharpPlus.Entities;

namespace Watchdog.Bot.Services.Interfaces;

public interface IMessageLogService
{
    Task LogDeletedMessageAsync(DiscordGuild guild, DiscordMessage message, ulong messageLogsChannelId);
    Task LogUpdatedMessageAsync(DiscordGuild guild, DiscordMessage messageBefore, DiscordMessage message, ulong messageLogsChannelId);
    Task LogBulkDeletedMessagesAsync(DiscordGuild guild, IReadOnlyList<DiscordMessage> messages, ulong messageLogsChannelId);
}