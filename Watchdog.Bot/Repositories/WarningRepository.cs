using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public sealed class WarningRepository : Repository<Warning>, IWarningRepository
{
    public WarningRepository(DbContext context) : base(context)
    {
    }
}