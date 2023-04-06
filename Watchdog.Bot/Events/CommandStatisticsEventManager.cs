using System.Collections.Concurrent;
using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.SlashCommands.EventArgs;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Events;

public sealed class CommandStatisticsEventManager : BaseEventManager
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CommandStatisticsEventManager> _logger;
    private readonly ConcurrentDictionary<(ulong member, ulong? guild, string command), Stopwatch> _stopwatches = new();

    public CommandStatisticsEventManager(IServiceScopeFactory serviceScopeFactory, ILogger<CommandStatisticsEventManager> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    [AsyncEventListener(EventType.SlashCommandInvoked)]
    public Task SlashCommandInvoked(DiscordClient client, SlashCommandInvokedEventArgs args)
    {
        _stopwatches[(args.Context.User.Id, args.Context.Guild.Id, args.Context.CommandName)] = Stopwatch.StartNew();
        return Task.CompletedTask;
    }

    [AsyncEventListener(EventType.SlashCommandErrored)]
    public Task SlashCommandErrored(DiscordClient client, SlashCommandErrorEventArgs args)
    {
        var key = (args.Context.User.Id, args.Context.Guild.Id, args.Context.CommandName);
        if (_stopwatches.ContainsKey(key))
        {
            var result = _stopwatches.TryRemove(key, out var stopwatch);
            if (!result) _logger.LogWarning("Failed to remove stopwatch from the dictionary (key: {key})", key);
        }

        _logger.LogWarning(args.Exception, "Command {Command} failed", args.Context.CommandName);
        return Task.CompletedTask;
    }

    [AsyncEventListener(EventType.SlashCommandExecuted)]
    public async Task SlashCommandExecuted(DiscordClient client, SlashCommandExecutedEventArgs args)
    {
        var key = (args.Context.User.Id, args.Context.Guild.Id, args.Context.CommandName);

        if (_stopwatches.ContainsKey(key))
        {
            var result = _stopwatches.TryRemove(key, out var stopwatch);
            if (!result)
            {
                _logger.LogWarning("Failed to remove stopwatch from the dictionary (key: {key})", key);
            }
            else
            {
                stopwatch!.Stop();
                _logger.LogInformation("User {User} executed command {Command} in guild {Guild} in {Time} ms", args.Context.User.ToNiceString(),
                    args.Context.Guild.ToNiceString(), args.Context.CommandName, stopwatch.ElapsedMilliseconds);

                await UpdateCommandUsageStatisticsAsync(args.Context.CommandName, args.Context.Guild.Id, stopwatch.ElapsedMilliseconds);
            }
        }
        
        _logger.LogInformation("User {User} executed command '{Command}' in guild {Guild}", args.Context.User.ToNiceString(),
            args.Context.CommandName, args.Context.Guild.ToNiceString());
    }

    private async Task UpdateCommandUsageStatisticsAsync(string command, ulong guild, long time)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var usageStatistics = scope.ServiceProvider.GetService<IUsageStatisticsService>();
        if (usageStatistics is null)
        {
            _logger.LogCritical($"Failed to get {nameof(IUsageStatisticsService)} from DI container");
            return;
        }
        
        await usageStatistics.IncrementAsync($"{command}_Usages", guild);
        await usageStatistics.IncrementByAsync($"{command}_ExecutionTime", guild, time);
    }
}