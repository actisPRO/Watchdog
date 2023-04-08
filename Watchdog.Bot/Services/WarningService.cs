using AutoMapper;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Strings;

namespace Watchdog.Bot.Services;

public sealed class WarningService : IWarningService
{
    private readonly IMapper _mapper;
    private readonly IWarningRepository _warningRepository;
    private readonly ILoggingService _loggingService;

    public WarningService(IMapper mapper, IWarningRepository warningRepository, ILoggingService loggingService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _warningRepository = warningRepository ?? throw new ArgumentNullException(nameof(warningRepository));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
    }

    public async Task<int> WarnMemberAsync(WarningData warningData)
    {
        var dbEntry = await CreateDatabaseEntryAsync(warningData);
        var warningCount = await GetWarningCountAsync(warningData.Guild.Id, warningData.User.Id);
        await CreateLogEntryAsync(true, warningData.Guild, warningData.Moderator, warningData.User, warningData.Reason,
            warningCount, dbEntry.CreatedAt, dbEntry.Id);
        await SendWarningNotificationAsync(warningData, warningCount, dbEntry.Id);
        return warningCount;
    }

    public async Task<(bool foundWarning, int count)> RemoveWarningAsync(DiscordMember user, string warningId, DiscordGuild guild,
        DiscordMember moderator)
    {
        var removedWarning = await DeleteDatabaseEntryAsync(guild.Id, user.Id, warningId);
        if (removedWarning == null)
            return (false, 0);

        var warningCount = await GetWarningCountAsync(guild.Id, moderator.Id);
        await CreateLogEntryAsync(false, guild, moderator, user, "", warningCount,
            DateTimeOffset.UtcNow, removedWarning.Id);
        
        await SendWarningDeletionNotificationAsync(user, moderator, guild, warningCount, removedWarning.Id);
        
        return (true, warningCount);
    }

    private async Task<int> GetWarningCountAsync(ulong guildId, ulong userId)
    {
        return await _warningRepository.GetCountAsync(warning => warning.GuildId == guildId && warning.UserId == userId);
    }

    private async Task<Warning> CreateDatabaseEntryAsync(WarningData warningData)
    {
        var warning = _mapper.Map<Warning>(warningData);
        return await _warningRepository.AddAsync(warning);
    }

    private async Task<Warning?> DeleteDatabaseEntryAsync(ulong guildId, ulong memberId, string warningId)
    {
        var warning = await _warningRepository.GetByIdAsync(warningId, guildId);
        if (warning == null)
            return null;

        if (memberId != warning.UserId)
            return null;

        await _warningRepository.DeleteAsync(warningId, guildId);
        return warning;
    }

    private async Task SendWarningNotificationAsync(WarningData warningData, int warningCount, string id)
    {
        try
        {
            var message = string.Format(Phrases.Notification_Warning, warningData.Moderator.FullName(), warningData.Guild.Name,
                warningData.Reason, warningCount, id);
            await warningData.User.SendMessageAsync(message);
        }
        catch (UnauthorizedException)
        {
            // user blocked the bot/not a server member
        }
    }

    private async Task SendWarningDeletionNotificationAsync(DiscordMember user, DiscordUser moderator, DiscordGuild guild, int warningCount, string id)
    {
        try
        {
            var message = string.Format(Phrases.Notification_WarningDeletion, moderator.FullName(), id, guild.Name,
                warningCount);
            await user.SendMessageAsync(message);
        } catch (UnauthorizedException)
        {
            // user blocked the bot/not a server member
        }
    }

    private async Task CreateLogEntryAsync(bool warnAdded, DiscordGuild guild, DiscordUser moderator, DiscordUser member, string reason,
        int warningCount, DateTimeOffset timestamp, string id)
    {
        LogEntry logEntry;
        if (warnAdded) 
            logEntry = LogEntry.CreateForWarning(guild, id, moderator, member, reason, timestamp, warningCount);
        else 
            logEntry = LogEntry.CreateForWarningDeletion(guild, id, moderator, member, timestamp, warningCount);
        
        await _loggingService.LogAsync(logEntry);
    }
}