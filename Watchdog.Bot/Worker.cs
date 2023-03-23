namespace Watchdog.Bot;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DiscordBotClient _discordBotClient;

    public Worker(ILogger<Worker> logger, DiscordBotClient discordBotClient)
    {
        _logger = logger;
        _discordBotClient = discordBotClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _discordBotClient.ExecuteAsync();
    }
}