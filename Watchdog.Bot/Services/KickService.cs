using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Strings;

namespace Watchdog.Bot.Services;

public sealed class KickService : IKickService
{
    private readonly ILoggingService _loggingService;

    public KickService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task KickMemberAsync(DiscordMember member, DiscordUser moderator, string reason)
    {
        await NotifyMemberAsync(member, moderator, reason);
        await member.RemoveAsync(reason);
        await CreateLogEntryAsync(member, moderator, reason);
    }
    
    private async Task NotifyMemberAsync(DiscordMember member, DiscordUser moderator, string reason)
    {
        try
        {
            var message = string.Format(Phrases.Notification_Kick, member.Guild.Name, moderator.FullName(), reason);
            await member.SendMessageAsync(message);
        }
        catch (UnauthorizedException)
        {
            
        }
    }
    
    private async Task CreateLogEntryAsync(DiscordMember member, DiscordUser moderator, string reason)
    {
        var logEntry = LogEntry.CreateForKick(member.Guild, moderator, member, reason, DateTimeOffset.UtcNow);
        await _loggingService.LogAsync(logEntry);
    }
}