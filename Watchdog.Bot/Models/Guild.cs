using System.ComponentModel.DataAnnotations.Schema;

namespace Watchdog.Bot.Models;

public sealed class Guild : IEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required ulong Id { get; set; }

    public object[] GetIdentity() => new object[] { Id };
}