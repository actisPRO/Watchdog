namespace Watchdog.Bot;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DiscordBotClient _discordBotClient;
    private readonly ParameterInitializer _parameterInitializer;

    public Worker(ILogger<Worker> logger, 
        DiscordBotClient discordBotClient,
        ParameterInitializer parameterInitializer)
    {
        _logger = logger;
        _discordBotClient = discordBotClient;
        _parameterInitializer = parameterInitializer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _parameterInitializer.InitializeAsync();
        await _discordBotClient.ExecuteAsync();
    }
}