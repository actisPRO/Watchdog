using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Options;
using Watchdog.Bot.Commands;
using Watchdog.Bot.Events;
using Watchdog.Bot.Options;

namespace Watchdog.Bot;

public sealed class DiscordBotClient
{
    private readonly ILogger<DiscordBotClient> _logger;
    private readonly DiscordOptions _discordOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IEnumerable<IEventManager> _eventManagers;
    private readonly IEnumerable<ICommandHandler> _commandHandlers;

    public DiscordBotClient(ILogger<DiscordBotClient> logger, 
        IOptions<DiscordOptions> discordOptions, 
        IServiceProvider serviceProvider, 
        ILoggerFactory loggerFactory, 
        IEnumerable<IEventManager> eventManagers, 
        IEnumerable<ICommandHandler> commandHandlers)
    {
        _logger = logger;
        _discordOptions = discordOptions.Value;
        _serviceProvider = serviceProvider;
        _loggerFactory = loggerFactory;
        _eventManagers = eventManagers;
        _commandHandlers = commandHandlers;
    }

    public async Task ExecuteAsync()
    {
        var discordClient = new DiscordClient(new ()
        {
            Token = _discordOptions.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged,
            LoggerFactory = _loggerFactory
        });

        var slashCommands = discordClient.UseSlashCommands(new()
        {
            Services = _serviceProvider
        });
        
        ulong? debugGuildId = null;
        if (_discordOptions.Debug && _discordOptions.DebugGuild != null)
            debugGuildId = _discordOptions.DebugGuild.Value;
        
        foreach (var eventManager in _eventManagers)
            eventManager.RegisterEvents(discordClient);
        foreach (var commandHandler in _commandHandlers)
            slashCommands.RegisterCommands(commandHandler.GetType(), debugGuildId);

        await discordClient.ConnectAsync();
    }
}