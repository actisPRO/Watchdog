using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Models.DataTransfer;

public sealed record ModerationLogEntryData
{
    public ulong GuildId { get; private init; }

    public ModerationAction Action { get; private init; }

    public ulong ExecutorId { get; private init; }

    public ulong TargetId { get; private init; }

    public DateTimeOffset Timestamp { get; private init; }

    public DateTimeOffset? ValidUntil { get; private init; }

    public string Reason { get; private init; } = string.Empty;

    private ModerationLogEntryData()
    {
    }

    #region Private factory methods

    private static ModerationLogEntryData CreateWithoutDuration(
        ModerationAction action,
        ulong guildId,
        ulong executorId,
        ulong targetId,
        string reason,
        DateTimeOffset timestamp)
    {
        return new ModerationLogEntryData
        {
            GuildId = guildId,
            ExecutorId = executorId,
            TargetId = targetId,
            Action = action,
            Reason = reason,
            Timestamp = timestamp
        };
    }

    private static ModerationLogEntryData CreateWithDuration(
        ModerationAction action,
        ulong guildId,
        ulong executorId,
        ulong targetId,
        string reason,
        DateTimeOffset timestamp,
        TimeSpan duration)
    {
        return new ModerationLogEntryData
        {
            GuildId = guildId,
            ExecutorId = executorId,
            TargetId = targetId,
            Action = action,
            Reason = reason,
            Timestamp = DateTimeOffset.UtcNow,
            ValidUntil = timestamp + duration
        };
    }

    #endregion

    #region Public factory methods

    public static ModerationLogEntryData CreateForWarning(ulong guildId, ulong executorId, ulong targetId, string reason,
        DateTimeOffset timestamp) => CreateWithoutDuration(ModerationAction.Warn, guildId, executorId, targetId, reason, timestamp);

    public static ModerationLogEntryData CreateForKick(ulong guildId, ulong executorId, ulong targetId, string reason,
        DateTimeOffset timestamp) => CreateWithoutDuration(ModerationAction.Kick, guildId, executorId, targetId, reason, timestamp);

    public static ModerationLogEntryData CreateForBan(ulong guildId, ulong executorId, ulong targetId, string reason,
        DateTimeOffset timestamp, TimeSpan duration) =>
        CreateWithDuration(ModerationAction.Ban, guildId, executorId, targetId, reason, timestamp, duration);

    #endregion
}