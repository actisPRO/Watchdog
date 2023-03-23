using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Repositories;

namespace Watchdog.Bot.Tests.Helpers;

public sealed class SampleRepository : Repository<SampleModel>
{
    public SampleRepository(DbContext context) : base(context)
    {
    }
}