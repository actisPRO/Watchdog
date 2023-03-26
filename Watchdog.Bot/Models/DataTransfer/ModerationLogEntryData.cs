using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Models.DataTransfer;

public sealed record ModerationLogEntryData
{
    public required ulong GuildId { get; set; }
    
    public required ModerationAction Action { get; set; }

    public required ulong ExecutorId { get; set; }

    public required ulong TargetId { get; set; }

    public required DateTimeOffset Timestamp { get; set; }
    
    public DateTimeOffset? ValidUntil { get; set; }
    
    public required string Reason { get; set; }
}