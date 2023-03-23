using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public sealed class GuildRepository : Repository<Guild>, IGuildRepository
{
    public GuildRepository(DbContext context) : base(context)
    {
    }
}