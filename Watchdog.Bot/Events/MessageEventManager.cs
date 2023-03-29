using DSharpPlus;
using DSharpPlus.EventArgs;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Events;

public sealed class MessageEventManager : BaseEventManager
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public MessageEventManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [AsyncEventListener(EventType.MessageDeleted)]
    public async Task ClientOnMessageDeleteReceivedAsync(DiscordClient client, MessageDeleteEventArgs args)
    {
        // Don't log messages from bots
        // Author can be null if bot
        if (args.Message.Author?.IsBot == null || args.Message.Author.IsBot) return;
        
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();
        var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
        
        var messageLogsChannelId = (await parameterService.GetGuildParameterValueAsync<ulong>("messages_log_channel_id", args.Guild.Id)).Value;
        await messageLogService.LogDeletedMessageAsync(args.Guild, args.Message, messageLogsChannelId);
    }
    
    [AsyncEventListener(EventType.MessageUpdated)]
    public async Task ClientOnMessageUpdateReceivedAsync(DiscordClient client, MessageUpdateEventArgs args)
    {
        // Don't log messages from bots
        // Author can be null if bot
        if (args.Message.Author?.IsBot == null || args.Message.Author.IsBot) return;
        
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();
        var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
        
        var messageLogsChannelId = (await parameterService.GetGuildParameterValueAsync<ulong>("messages_log_channel_id", args.Guild.Id)).Value;
        await messageLogService.LogUpdatedMessageAsync(args.Guild, args.MessageBefore, args.Message, messageLogsChannelId);
    }

    [AsyncEventListener(EventType.MessagesBulkDeleted)]
    public async Task ClientOnMessagesBulkDeleteReceivedAsync(DiscordClient client, MessageBulkDeleteEventArgs args)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();
        var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
        
        var messageLogsChannelId = (await parameterService.GetGuildParameterValueAsync<ulong>("messages_log_channel_id", args.Guild.Id)).Value;
        await messageLogService.LogBulkDeletedMessagesAsync(args.Guild, args.Messages, messageLogsChannelId);
    }
}