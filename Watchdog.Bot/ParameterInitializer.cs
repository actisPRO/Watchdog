using System.Diagnostics;
using Watchdog.Bot.Constants;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot;

public sealed class ParameterInitializer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ParameterInitializer> _logger;

    public ParameterInitializer(IServiceScopeFactory serviceScopeFactory, 
        ILogger<ParameterInitializer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        _logger.LogInformation("Started parameter initialization");
        
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();

        await parameterService.RegisterParameterAsync<ulong>(new()
        {
            Name = ParameterNames.ModerationLogChannelId,
            Value = 0
        });
        
        stopwatch.Stop();
        _logger.LogInformation("Finished parameter initialization in {ElapsedMilliseconds} ms", 
            stopwatch.ElapsedMilliseconds);
    }
}