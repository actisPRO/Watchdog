using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Services;

public sealed class UsageStatisticsService : IUsageStatisticsService
{
    private readonly IUsageStatisticsRepository _usageStatistics;

    public UsageStatisticsService(IUsageStatisticsRepository usageStatistics)
    {
        _usageStatistics = usageStatistics;
    }

    public async Task IncrementAsync(string key, ulong guildId)
    {
        await IncrementByAsync(key, guildId, 1);
    }

    public async Task IncrementByAsync(string key, ulong guildId, int value)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var statistic = await _usageStatistics.GetByIdAsync(key, guildId, today);
        
        if (statistic is null)
        {
            statistic = new UsageStatistic
            {
                Key = key,
                GuildId = guildId,
                Date = today,
                Value = value
            };
            await _usageStatistics.AddAsync(statistic);
        }
        else
        {
            statistic.Value += value;
            await _usageStatistics.UpdateAsync(statistic);
        }
    }
}