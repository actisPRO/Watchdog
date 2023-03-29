using System.Reflection;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Models.Utilities;

namespace Watchdog.Bot.Events;

public abstract class BaseEventManager : IEventManager
{
    public void RegisterEvents(DiscordClient client)
    {
        var eventListeners = from m in GetType().GetMethods()
            let attribute = AsyncEventListenerAttribute.GetAttribute(m)
            where attribute != null
            select new ListenerMethod { Method = m, Attribute = attribute };
        
        foreach (var listener in eventListeners)
            Register(client, listener.Method, listener.Attribute.Event);
    }
    
    private Task OnClientEventTask(MethodInfo listener, DiscordClient sender, object eventArgs)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await (Task)listener.Invoke(this, new[] { sender, eventArgs })!;
            }
            catch (Exception e)
            {
                sender.Logger.LogError(e, $"Uncaught exception in the listener thread");
            }
        });

        return Task.CompletedTask;
    }

    private Task OnCommandEventTask(MethodInfo listener, SlashCommandsExtension sender, object eventArgs)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await (Task)listener.Invoke(this, new[] { sender, eventArgs })!;
            }
            catch (Exception e)
            {
                sender.Client.Logger.LogError(e, $"Uncaught exception in the listener thread");
            }
        });

        return Task.CompletedTask;
    }

    private void Register(DiscordClient client, MethodInfo listener, EventType eventType)
    {
        async Task OnClientEvent(DiscordClient sender, object eventArgs)
            => await OnClientEventTask(listener, sender, eventArgs);

        async Task OnCommandEvent(SlashCommandsExtension sender, object eventArgs)
            => await OnCommandEventTask(listener, sender, eventArgs);

        #region Events

        switch (eventType)
        {
            case EventType.SocketErrored:
                client.SocketErrored += OnClientEvent;
                break;
            case EventType.SocketOpened:
                client.SocketOpened += OnClientEvent;
                break;
            case EventType.SocketClosed:
                client.SocketClosed += OnClientEvent;
                break;
            case EventType.Ready:
                client.Ready += OnClientEvent;
                break;
            case EventType.Resumed:
                client.Resumed += OnClientEvent;
                break;
            case EventType.Heartbeated:
                client.Heartbeated += OnClientEvent;
                break;
            case EventType.Zombied:
                client.Zombied += OnClientEvent;
                break;
            case EventType.ChannelCreated:
                client.ChannelCreated += OnClientEvent;
                break;
            case EventType.ChannelUpdated:
                client.ChannelUpdated += OnClientEvent;
                break;
            case EventType.ChannelDeleted:
                client.ChannelDeleted += OnClientEvent;
                break;
            case EventType.DmChannelDeleted:
                client.DmChannelDeleted += OnClientEvent;
                break;
            case EventType.ChannelPinsUpdated:
                client.ChannelPinsUpdated += OnClientEvent;
                break;
            case EventType.GuildCreated:
                client.GuildCreated += OnClientEvent;
                break;
            case EventType.GuildAvailable:
                client.GuildAvailable += OnClientEvent;
                break;
            case EventType.GuildUpdated:
                client.GuildUpdated += OnClientEvent;
                break;
            case EventType.GuildDeleted:
                client.GuildDeleted += OnClientEvent;
                break;
            case EventType.GuildUnavailable:
                client.GuildUnavailable += OnClientEvent;
                break;
            case EventType.GuildDownloadCompleted:
                client.GuildDownloadCompleted += OnClientEvent;
                break;
            case EventType.GuildEmojisUpdated:
                client.GuildEmojisUpdated += OnClientEvent;
                break;
            case EventType.GuildStickersUpdated:
                client.GuildStickersUpdated += OnClientEvent;
                break;
            case EventType.GuildIntegrationsUpdated:
                client.GuildIntegrationsUpdated += OnClientEvent;
                break;
            case EventType.ScheduledGuildEventCreated:
                client.ScheduledGuildEventCreated += OnClientEvent;
                break;
            case EventType.ScheduledGuildEventUpdated:
                client.ScheduledGuildEventUpdated += OnClientEvent;
                break;
            case EventType.ScheduledGuildEventDeleted:
                client.ScheduledGuildEventDeleted += OnClientEvent;
                break;
            case EventType.ScheduledGuildEventCompleted:
                client.ScheduledGuildEventCompleted += OnClientEvent;
                break;
            case EventType.ScheduledGuildEventUserAdded:
                client.ScheduledGuildEventUserAdded += OnClientEvent;
                break;
            case EventType.ScheduledGuildEventUserRemoved:
                client.ScheduledGuildEventUserRemoved += OnClientEvent;
                break;
            case EventType.GuildBanAdded:
                client.GuildBanAdded += OnClientEvent;
                break;
            case EventType.GuildBanRemoved:
                client.GuildBanRemoved += OnClientEvent;
                break;
            case EventType.GuildMemberAdded:
                client.GuildMemberAdded += OnClientEvent;
                break;
            case EventType.GuildMemberRemoved:
                client.GuildMemberRemoved += OnClientEvent;
                break;
            case EventType.GuildMemberUpdated:
                client.GuildMemberUpdated += OnClientEvent;
                break;
            case EventType.GuildMembersChunked:
                client.GuildMembersChunked += OnClientEvent;
                break;
            case EventType.GuildRoleCreated:
                client.GuildRoleCreated += OnClientEvent;
                break;
            case EventType.GuildRoleUpdated:
                client.GuildRoleUpdated += OnClientEvent;
                break;
            case EventType.GuildRoleDeleted:
                client.GuildRoleDeleted += OnClientEvent;
                break;
            case EventType.InviteCreated:
                client.InviteCreated += OnClientEvent;
                break;
            case EventType.InviteDeleted:
                client.InviteDeleted += OnClientEvent;
                break;
            case EventType.MessageCreated:
                client.MessageCreated += OnClientEvent;
                break;
            case EventType.MessageAcknowledged:
                client.MessageAcknowledged += OnClientEvent;
                break;
            case EventType.MessageUpdated:
                client.MessageUpdated += OnClientEvent;
                break;
            case EventType.MessageDeleted:
                client.MessageDeleted += OnClientEvent;
                break;
            case EventType.MessagesBulkDeleted:
                client.MessagesBulkDeleted += OnClientEvent;
                break;
            case EventType.MessageReactionAdded:
                client.MessageReactionAdded += OnClientEvent;
                break;
            case EventType.MessageReactionRemoved:
                client.MessageReactionRemoved += OnClientEvent;
                break;
            case EventType.MessageReactionsCleared:
                client.MessageReactionsCleared += OnClientEvent;
                break;
            case EventType.MessageReactionRemovedEmoji:
                client.MessageReactionRemovedEmoji += OnClientEvent;
                break;
            case EventType.PresenceUpdated:
                client.PresenceUpdated += OnClientEvent;
                break;
            case EventType.UserSettingsUpdated:
                client.UserSettingsUpdated += OnClientEvent;
                break;
            case EventType.UserUpdated:
                client.UserUpdated += OnClientEvent;
                break;
            case EventType.VoiceStateUpdated:
                client.VoiceStateUpdated += OnClientEvent;
                break;
            case EventType.VoiceServerUpdated:
                client.VoiceServerUpdated += OnClientEvent;
                break;
            case EventType.ThreadCreated:
                client.ThreadCreated += OnClientEvent;
                break;
            case EventType.ThreadUpdated:
                client.ThreadUpdated += OnClientEvent;
                break;
            case EventType.ThreadDeleted:
                client.ThreadDeleted += OnClientEvent;
                break;
            case EventType.ThreadListSynced:
                client.ThreadListSynced += OnClientEvent;
                break;
            case EventType.ThreadMemberUpdated:
                client.ThreadMemberUpdated += OnClientEvent;
                break;
            case EventType.ThreadMembersUpdated:
                client.ThreadMembersUpdated += OnClientEvent;
                break;
            case EventType.IntegrationCreated:
                client.IntegrationCreated += OnClientEvent;
                break;
            case EventType.IntegrationUpdated:
                client.IntegrationUpdated += OnClientEvent;
                break;
            case EventType.IntegrationDeleted:
                client.IntegrationDeleted += OnClientEvent;
                break;
            case EventType.StageInstanceCreated:
                client.StageInstanceCreated += OnClientEvent;
                break;
            case EventType.StageInstanceUpdated:
                client.StageInstanceUpdated += OnClientEvent;
                break;
            case EventType.StageInstanceDeleted:
                client.StageInstanceDeleted += OnClientEvent;
                break;
            case EventType.InteractionCreated:
                client.InteractionCreated += OnClientEvent;
                break;
            case EventType.ComponentInteractionCreated:
                client.ComponentInteractionCreated += OnClientEvent;
                break;
            case EventType.ModalSubmitted:
                client.ModalSubmitted += OnClientEvent;
                break;
            case EventType.ContextMenuInteractionCreated:
                client.ContextMenuInteractionCreated += OnClientEvent;
                break;
            case EventType.TypingStarted:
                client.TypingStarted += OnClientEvent;
                break;
            case EventType.UnknownEvent:
                client.UnknownEvent += OnClientEvent;
                break;
            case EventType.WebhooksUpdated:
                client.WebhooksUpdated += OnClientEvent;
                break;
            case EventType.ClientErrored:
                client.ClientErrored += OnClientEvent;
                break;
            case EventType.SlashCommandErrored:
                client.GetSlashCommands().SlashCommandErrored += OnCommandEvent;
                break;
            case EventType.SlashCommandInvoked:
                client.GetSlashCommands().SlashCommandInvoked += OnCommandEvent;
                break;
            case EventType.SlashCommandExecuted:
                client.GetSlashCommands().SlashCommandExecuted += OnCommandEvent;
                break;
            case EventType.ContextMenuErrored:
                client.GetSlashCommands().ContextMenuErrored += OnCommandEvent;
                break;
            case EventType.ContextMenuInvoked:
                client.GetSlashCommands().ContextMenuInvoked += OnCommandEvent;
                break;
            case EventType.ContextMenuExecuted:
                client.GetSlashCommands().ContextMenuExecuted += OnCommandEvent;
                break;
            case EventType.AutocompleteErrored:
                client.GetSlashCommands().AutocompleteErrored += OnCommandEvent;
                break;
            case EventType.AutocompleteExecuted:
                client.GetSlashCommands().AutocompleteExecuted += OnCommandEvent;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        #endregion
    }
}