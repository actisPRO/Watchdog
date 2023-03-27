using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Models;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Options;

namespace Watchdog.Bot;

public sealed class DatabaseContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions;

    private DbSet<Guild> Guilds { get; set; } = default!;
    private DbSet<Parameter> Parameters { get; set; } = default!;
    private DbSet<GuildParameter> GuildParameters { get; set; } = default!;
    private DbSet<ModerationLogEntry> ModerationLog { get; set; } = default!;

    public DatabaseContext(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ModerationLogEntry>()
            .Property(x => x.Action)
            .HasConversion(x => x.ToString(), x => Enum.Parse<ModerationAction>(x));
        
        modelBuilder.Entity<Guild>()
            .Property(x => x.DatabaseEntryCreatedAt)
            .HasDefaultValueSql("current_timestamp")
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseOptions.ConnectionString);
    }
}