﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Events;

public sealed class BasicGuildEventManager : BaseEventManager
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BasicGuildEventManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    [AsyncEventListener(EventType.GuildAvailable)]
    public async Task RegisterGuildInDbAsync(DiscordClient sender, GuildCreateEventArgs e)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var guilds = scope.ServiceProvider.GetService<IGuildService>().ThrowIfNull();
        await guilds.CreateOrUpdateGuildAsync(e.Guild);
    }
    
    [AsyncEventListener(EventType.GuildCreated)]
    public async Task ClientOnGuildCreatedReceivedAsync(DiscordClient sender, GuildCreateEventArgs e)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var botStatusService = scope.ServiceProvider.GetService<IBotStatus>().ThrowIfNull();
        await botStatusService.UpdateBotStatusAsync(sender);
    }
}