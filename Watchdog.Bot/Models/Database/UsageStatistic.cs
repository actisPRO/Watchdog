using Microsoft.EntityFrameworkCore;

namespace Watchdog.Bot.Models.Database;

[PrimaryKey(nameof(Key), nameof(GuildId), nameof(Date))]
public sealed class UsageStatistic : IEntity
{
    public required string Key { get; set; }
    
    public required ulong GuildId { get; set; }

    public Guild Guild { get; set; } = default!;
    
    public required DateOnly Date { get; set; }
    
    public required int Value { get; set; }

    public object[] GetIdentity() => new object[] { Key, GuildId, Date };
}