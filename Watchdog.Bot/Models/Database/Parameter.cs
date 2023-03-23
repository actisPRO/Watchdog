using System.ComponentModel.DataAnnotations;

namespace Watchdog.Bot.Models.Database;

public sealed class Parameter : IEntity
{
    [Key]
    public required string Name { get; set; }
    
    public required string Value { get; set; }
    
    public required string Type { get; set; }
    
    public object[] GetIdentity() => new object[] { Name };
}