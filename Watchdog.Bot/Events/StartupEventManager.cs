using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Watchdog.Bot.Events;

public sealed class StartupEventManager : IEventManager
{
    public void RegisterEvents(DiscordClient client)
    {
        client.Ready += SendMessageOnReady;    
    }

    private Task SendMessageOnReady(DiscordClient sender, ReadyEventArgs e)
    {
        sender.Logger.LogInformation("Discord client is ready");
        return Task.CompletedTask;
    }
}