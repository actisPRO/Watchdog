using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Watchdog.Bot.Options;

namespace Watchdog.Bot;

public sealed class DatabaseContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions;

    public DatabaseContext(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseOptions.ConnectionString);
    }
}