using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IGuildRepository, GuildRepository>()
            .AddTransient<IParameterRepository, ParameterRepository>()
            .AddTransient<IGuildParameterRepository, GuildParameterRepository>()
            .AddTransient<ILogRepository, LogRepository>()
            .AddTransient<IUsageStatisticsRepository, UsageStatisticsRepository>()
            .AddTransient<IWarningRepository, WarningRepository>();
    }
}