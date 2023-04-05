using System.ComponentModel;
using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Models.Database;

public sealed class ModerationLogEntry : IEntity
{
    public Guid Id { get; set; }

    public required ulong GuildId { get; set; }

    public Guild Guild { get; set; } = default!;
    
    public required ModerationAction Action { get; set; }

    public required ulong ExecutorId { get; set; }

    public required ulong TargetId { get; set; }

    public required DateTimeOffset Timestamp { get; set; }
    
    public DateTimeOffset? ValidUntil { get; set; }
    
    public required string Reason { get; set; }
    
    [DefaultValue("{}")]
    public string AdditionalData { get; set; } = string.Empty;

    public object[] GetIdentity() => new object[] { Id };
}