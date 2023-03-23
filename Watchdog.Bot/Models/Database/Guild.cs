using System.ComponentModel.DataAnnotations.Schema;

namespace Watchdog.Bot.Models.Database;

public sealed class Guild : IEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required ulong Id { get; set; }
    
    public required DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset DatabaseEntryCreatedAt { get; set; }

    public object[] GetIdentity() => new object[] { Id };
}