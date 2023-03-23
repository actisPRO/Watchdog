using Microsoft.EntityFrameworkCore;

namespace Watchdog.Bot.Tests.Repositories.Helpers;

public sealed class InMemoryContext : DbContext
{
    public DbSet<SampleModel> SampleModels { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
}