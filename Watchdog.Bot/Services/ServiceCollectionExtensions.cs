using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddTransient<IParameterService, ParameterService>()
            .AddTransient<IGuildService, GuildService>();
    }
}