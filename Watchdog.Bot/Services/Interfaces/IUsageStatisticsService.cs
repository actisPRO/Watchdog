namespace Watchdog.Bot.Services.Interfaces;

public interface IUsageStatisticsService
{
    Task IncrementAsync(string key, ulong guildId);

    Task IncrementByAsync(string key, ulong guildId, long value);
}