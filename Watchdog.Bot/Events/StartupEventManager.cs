using DSharpPlus;
using DSharpPlus.EventArgs;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Events;

public sealed class StartupEventManager : BaseEventManager
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public StartupEventManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    [AsyncEventListener(EventType.Ready)]
    public Task SendMessageOnReady(DiscordClient sender, ReadyEventArgs e)
    {
        sender.Logger.LogInformation("Discord client is ready");
        return Task.CompletedTask;
    }
    
    [AsyncEventListener(EventType.GuildDownloadCompleted)]
    public async Task ClientOnGuildDownloadCompletedAsync(DiscordClient client, GuildDownloadCompletedEventArgs e)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var botStatusService = scope.ServiceProvider.GetRequiredService<IBotStatus>();
        await botStatusService.UpdateBotStatusAsync(client);
    }
}