using System.Text;
using AutoMapper;
using DSharpPlus;
using DSharpPlus.Entities;
using Watchdog.Bot.Constants;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Exceptions;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Strings;

namespace Watchdog.Bot.Services;

public sealed class LoggingService : ILoggingService
{
    private readonly ILogger<GuildService> _logger;
    private readonly IMapper _mapper;
    private readonly ILogRepository _logRepository;
    private readonly IParameterService _parameterService;

    public LoggingService(ILogger<GuildService> logger, IMapper mapper, ILogRepository logRepository,
        IParameterService parameterService)
    {
        _logger = logger;
        _mapper = mapper;
        _logRepository = logRepository;
        _parameterService = parameterService;
    }

    public async Task LogAsync(LogEntry entry)
    {
        try
        {
            await LogInDbAsync(entry);

            var channel = await GetLogChannelAsync(entry.Guild);
            if (channel != null)
                await LogInChannelAsync(channel, entry);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create a log entry");
        }
    }

    private async Task LogInDbAsync(LogEntry logEntry)
    {
        var dbEntity = _mapper.Map<ModerationLogEntry>(logEntry);
        await _logRepository.AddAsync(dbEntity);
    }

    private async Task<DiscordChannel?> GetLogChannelAsync(DiscordGuild guild)
    {
        try
        {
            var channelId = 
                await _parameterService.GetGuildParameterValueAsync<ulong>(ParameterNames.ModerationLogChannelId, guild.Id);
            var channel = guild.GetChannel(channelId.Value);
            return channel;
        }
        catch (ObjectNotFoundException e)
        {
            _logger.LogError(e, "Failed to get log channel parameter value. Have you initialized the parameter?");
            return null;
        }
    }

    private async Task LogInChannelAsync(DiscordChannel channel, LogEntry entry)
    {
        var message = CreateLogMessage(entry);
        await channel.SendMessageAsync(message);
    }

    private DiscordMessageBuilder CreateLogMessage(LogEntry entry)
    {
        return new DiscordMessageBuilder().WithContent(CreateLogMessageText(entry));
    }

    private string CreateLogMessageText(LogEntry entry)
    {
        var builder = new StringBuilder()
            .AppendLine($"**{GetMessageTitle(entry)}**")
            .AppendLine()
            .AppendLine($"**{Phrases.Moderator}:** {entry.Executor.ToNiceString()}")
            .AppendLine($"**{Phrases.User}:** {entry.Target.ToNiceString()}")
            .AppendLine($"**{Phrases.Reason}:** {entry.Reason}");

        if (entry.ValidUntil != null)
            builder = builder.AppendLine($"**{Phrases.Until}:** {Formatter.Timestamp(entry.ValidUntil.Value, TimestampFormat.ShortDateTime)}");

        builder = builder.AppendLine($"**{Phrases.Timestamp}:** {Formatter.Timestamp(entry.Timestamp, TimestampFormat.ShortDateTime)}");

        return builder.ToString();
    }

    private string GetMessageTitle(LogEntry entry)
    {
        return entry.Action switch
        {
            ModerationAction.Warn => Phrases.Warning,
            ModerationAction.Kick => Phrases.Kick,
            ModerationAction.Ban => Phrases.Ban,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}