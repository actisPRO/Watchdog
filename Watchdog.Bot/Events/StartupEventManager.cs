using DSharpPlus;
using DSharpPlus.EventArgs;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Events;

public sealed class StartupEventManager : BaseEventManager
{
    [AsyncEventListener(EventType.Ready)]
    public Task SendMessageOnReady(DiscordClient sender, ReadyEventArgs e)
    {
        sender.Logger.LogInformation("Discord client is ready");
        return Task.CompletedTask;
    }
}