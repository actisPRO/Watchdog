using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public sealed class ParameterRepository : Repository<Parameter>, IParameterRepository
{
    public ParameterRepository(DbContext context) : base(context)
    {
    }
}