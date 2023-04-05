using AutoMapper;
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
        var warningCount = await GetWarningCountAsync(warningData);
        await CreateWarningLogEntryAsync(warningData, warningCount, dbEntry.CreatedAt, dbEntry.Id);
        await SendWarningNotificationAsync(warningData, warningCount);
        return warningCount;
    }

    private async Task<int> GetWarningCountAsync(WarningData warningData)
    {
        return await _warningRepository.GetCountAsync(warning => warning.GuildId == warningData.Guild.Id && warning.UserId == warningData.User.Id);
    }

    private async Task<Warning> CreateDatabaseEntryAsync(WarningData warningData)
    {
        var warning = _mapper.Map<Warning>(warningData);
        return await _warningRepository.AddAsync(warning);
    }

    private async Task SendWarningNotificationAsync(WarningData warningData, int warningCount)
    {
        try
        {
            var message = string.Format(Phrases.Notification_Warning, warningData.Moderator.ToNiceString(), warningData.Guild.ToNiceString(),
                warningData.Reason, warningCount);
            await warningData.User.SendMessageAsync(message);
        }
        catch (UnauthorizedException)
        {
            // user blocked the bot/not server member
        }
    }

    private async Task CreateWarningLogEntryAsync(WarningData warningData, int warningCount, DateTimeOffset timestamp, string id)
    {
        var logEntry = LogEntry.CreateForWarning(warningData.Guild, id, warningData.Moderator, warningData.User, warningData.Reason, timestamp,
            warningCount);
        await _loggingService.LogAsync(logEntry);
    }
}