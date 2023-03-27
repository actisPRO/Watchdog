using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models.Database;

namespace Watchdog.Bot.Repositories.Interfaces;

public sealed class LogRepository : Repository<ModerationLogEntry>, ILogRepository
{
    public LogRepository(DbContext context) : base(context)
    {
    }
}