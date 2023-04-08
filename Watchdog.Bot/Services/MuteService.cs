using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Strings;
using Watchdog.Bot.Utils;

namespace Watchdog.Bot.Services;

public class MuteService : IMuteService

{
    private readonly ILoggingService _loggingService;

    public MuteService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task MuteMemberAsync(DiscordMember member, DiscordUser moderator, string duration, string reason)
    {
        var timeSpan = TimeParserUtils.ParseTimeSpan(duration);
        var until = DateTimeOffset.UtcNow.Add(timeSpan);
        var formattedUntil = Formatter.Timestamp(until, TimestampFormat.RelativeTime);
        
        var message = string.Format(Phrases.Notification_Mute, member.Guild.Name, moderator.FullName(), reason, formattedUntil);

        await NotifyMemberAsync(member, message);
        await member.TimeoutAsync(until, reason);

        var logEntry = LogEntry.CreateForMute(member.Guild, moderator, member, reason, DateTimeOffset.UtcNow, timeSpan);
        await _loggingService.LogAsync(logEntry);
    }

    public async Task UnmuteMemberAsync(DiscordMember member, DiscordUser moderator, string reason)
    {
        var message = string.Format(Phrases.Notification_Unmute, member.Guild.Name, moderator.FullName(), reason);
        
        await NotifyMemberAsync(member, message);
        await member.TimeoutAsync(null, reason);
        
        var logEntry = LogEntry.CreateForUnmute(member.Guild, moderator, member, reason, DateTimeOffset.UtcNow);
        await _loggingService.LogAsync(logEntry);
    }

    private async Task NotifyMemberAsync(
        DiscordMember member, string message)
    {
        try
        {
            await member.SendMessageAsync(message);
        }
        catch (UnauthorizedException)
        {
        }
    }
}