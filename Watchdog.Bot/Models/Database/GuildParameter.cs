using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Watchdog.Bot.Models.Database;

[PrimaryKey(nameof(Name), nameof(GuildId))]
public sealed class GuildParameter : IEntity
{
    [ForeignKey(nameof(Parameter))]
    public required string Name { get; set; }

    public Parameter Parameter { get; set; } = default!;
    
    public required ulong GuildId { get; set; }
    
    public Guild Guild { get; set; } = default!;
    
    public required string Value { get; set; }
    
    public object[] GetIdentity() => new object[] { Name, GuildId };
}