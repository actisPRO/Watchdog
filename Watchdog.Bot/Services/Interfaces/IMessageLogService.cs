using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Watchdog.Bot.Services.Interfaces;

public interface IMessageLogService
{
    Task ClientOnMessageDeleteReceived(DiscordClient client, DiscordGuild guild, DiscordMessage message, ulong messageLogsChannelId);
    Task ClientOnMessageUpdateReceived(DiscordClient client, DiscordGuild guild, DiscordMessage messageBefore, DiscordMessage message, ulong messageLogsChannelId);
    Task ClientOnMessagesBulkDeleteReceived(DiscordClient client, DiscordGuild guild, IReadOnlyList<DiscordMessage> messages, ulong messageLogsChannelId);
}