using AutoMapper;
using DSharpPlus;
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
    private readonly ILogger<WarningService> _logger;
    private readonly IMapper _mapper;
    private readonly IWarningRepository _warningRepository;

    public WarningService(ILogger<WarningService> logger, IMapper mapper, IWarningRepository warningRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _warningRepository = warningRepository ?? throw new ArgumentNullException(nameof(warningRepository));
    }

    public async Task WarnMemberAsync(WarningData warningData)
    {
        await CreateDatabaseEntryAsync(warningData);
        var warningCount = await GetWarningCountAsync(warningData);
        await CreateWarningLogEntryAsync(warningData, warningCount);
        await SendWarningNotificationAsync(warningData, warningCount);
    }

    private async Task<int> GetWarningCountAsync(WarningData warningData)
    {
        return await _warningRepository.GetCountAsync(warning => warning.GuildId == warningData.Guild.Id && warning.UserId == warningData.User.Id);
    }

    private async Task CreateDatabaseEntryAsync(WarningData warningData)
    {
        var warning = _mapper.Map<Warning>(warningData);
        await _warningRepository.AddAsync(warning);
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

    private async Task CreateWarningLogEntryAsync(WarningData warningData, int warningCount)
    {
        throw new NotImplementedException();
    }
}