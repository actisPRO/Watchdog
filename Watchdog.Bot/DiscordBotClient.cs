using System.Reflection;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Options;
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

    public DiscordBotClient(ILogger<DiscordBotClient> logger,
        IOptions<DiscordOptions> discordOptions,
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory,
        IEnumerable<IEventManager> eventManagers)
    {
        _logger = logger;
        _discordOptions = discordOptions.Value;
        _serviceProvider = serviceProvider;
        _loggerFactory = loggerFactory;
        _eventManagers = eventManagers;
    }

    public async Task ExecuteAsync()
    {
        var discordClient = new DiscordClient(new()
        {
            Token = _discordOptions.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            LoggerFactory = _loggerFactory,
            LogUnknownEvents = false, // GUILD_AUDIT_LOG_ENTRY_CREATE unknown event in D#+
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
        
        slashCommands.RegisterCommands(Assembly.GetExecutingAssembly(), debugGuildId);

        await discordClient.ConnectAsync();
    }
}