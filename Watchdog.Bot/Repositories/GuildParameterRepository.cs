using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public sealed class GuildParameterRepository : Repository<GuildParameter>, IGuildParameterRepository
{
    public GuildParameterRepository(DbContext context) : base(context)
    {
    }
}