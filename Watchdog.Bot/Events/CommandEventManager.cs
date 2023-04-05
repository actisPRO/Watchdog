using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Events;

public sealed class CommandEventManager : BaseEventManager
{
    private readonly ILogger<CommandEventManager> _logger;

    public CommandEventManager(ILogger<CommandEventManager> logger)
    {
        _logger = logger;
    }

    [AsyncEventListener(EventType.SlashCommandErrored)]
    public Task OnSlashCommandErrored(SlashCommandsExtension slashCommandsExtension, SlashCommandErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error occurred while executing slash command {CommandName}", args.Context.CommandName);
        return Task.CompletedTask;
    }
}