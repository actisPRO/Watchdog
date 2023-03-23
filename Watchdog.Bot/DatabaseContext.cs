using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Watchdog.Bot.Models;
using Watchdog.Bot.Options;

namespace Watchdog.Bot;

public sealed class DatabaseContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions;

    private DbSet<Guild> Guilds { get; set; } = default!;
    private DbSet<Parameter> Parameters { get; set; } = default!;
    private DbSet<GuildParameter> GuildParameters { get; set; } = default!;

    public DatabaseContext(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseOptions.ConnectionString);
    }
}