using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Events;

public sealed class BasicGuildEventManager : IEventManager
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BasicGuildEventManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void RegisterEvents(DiscordClient client)
    {
        client.GuildAvailable += RegisterGuildInDbAsync;
    }

    private async Task RegisterGuildInDbAsync(DiscordClient sender, GuildCreateEventArgs e)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var guilds = scope.ServiceProvider.GetService<IGuildService>().ThrowIfNull();
        await guilds.CreateOrUpdateGuildAsync(e.Guild);
    }
}