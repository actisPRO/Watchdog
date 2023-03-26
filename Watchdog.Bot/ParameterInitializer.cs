using Watchdog.Bot.Constants;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot;

public sealed class ParameterInitializer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ParameterInitializer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InitializeAsync()
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var parameterService = scope.ServiceProvider.GetRequiredService<IParameterService>();

        await parameterService.RegisterParameterAsync<ulong>(new()
        {
            Name = ParameterNames.ModerationLogChannelId,
            Value = 0
        });
    }
}