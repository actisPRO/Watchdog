using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public sealed class UsageStatisticsRepository : Repository<UsageStatistic>, IUsageStatisticsRepository
{
    public UsageStatisticsRepository(DbContext context) : base(context)
    {
    }
}