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
    public async Task ClientOnMessageDeleteReceived(DiscordClient client, MessageDeleteEventArgs args)
    {
        // Don't log messages from bots
        if (args.Message.Author.IsBot) return;
        
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();
        var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
        
        var messageLogsChannelId = (await parameterService.GetGuildParameterValueAsync<ulong>("messages_log_channel_id", args.Guild.Id)).Value;
        await messageLogService.LogDeletedMessage(args.Guild, args.Message, messageLogsChannelId);
    }
    
    [AsyncEventListener(EventType.MessageUpdated)]
    public async Task ClientOnMessageUpdateReceived(DiscordClient client, MessageUpdateEventArgs args)
    {
        // Don't log messages from bots
        if (args.Message.Author.IsBot) return;
        
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();
        var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
        
        var messageLogsChannelId = (await parameterService.GetGuildParameterValueAsync<ulong>("messages_log_channel_id", args.Guild.Id)).Value;
        await messageLogService.LogUpdatedMessage(args.Guild, args.MessageBefore, args.Message, messageLogsChannelId);
    }

    [AsyncEventListener(EventType.MessagesBulkDeleted)]
    public async Task ClientOnMessagesBulkDeleteReceived(DiscordClient client, MessageBulkDeleteEventArgs args)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();
        var messageLogService = scope.ServiceProvider.GetRequiredService<IMessageLogService>();
        
        var messageLogsChannelId = (await parameterService.GetGuildParameterValueAsync<ulong>("messages_log_channel_id", args.Guild.Id)).Value;
        await messageLogService.LogBulkDeletedMessages(args.Guild, args.Messages, messageLogsChannelId);
    }
}