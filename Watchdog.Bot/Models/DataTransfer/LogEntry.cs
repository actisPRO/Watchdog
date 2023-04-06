using DSharpPlus.Entities;
using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Models.DataTransfer;

public sealed record LogEntry
{
    public DiscordGuild Guild { get; private init; } = default!;

    public ModerationAction Action { get; private init; }

    public DiscordUser Executor { get; private init; } = default!;

    public DiscordUser Target { get; private init; } = default!;

    public DateTimeOffset Timestamp { get; private init; }

    public DateTimeOffset? ValidUntil { get; private init; }

    public string Reason { get; private init; } = string.Empty;

    private LogEntry()
    {
    }

    #region Private factory methods

    private static LogEntry CreateWithoutDuration(
        ModerationAction action,
        DiscordGuild guild,
        DiscordUser executor,
        DiscordUser target,
        string reason,
        DateTimeOffset timestamp)
    {
        return new LogEntry
        {
            Guild = guild,
            Executor = executor,
            Target = target,
            Action = action,
            Reason = reason,
            Timestamp = timestamp
        };
    }

    private static LogEntry CreateWithDuration(
        ModerationAction action,
        DiscordGuild guild,
        DiscordUser executor,
        DiscordUser target,
        string reason,
        DateTimeOffset timestamp,
        TimeSpan duration)
    {
        return new LogEntry
        {
            Guild = guild,
            Executor = executor,
            Target = target,
            Action = action,
            Reason = reason,
            Timestamp = DateTimeOffset.UtcNow,
            ValidUntil = timestamp + duration
        };
    }

    #endregion

    #region Public factory methods

    public static LogEntry CreateForWarning(DiscordGuild guild, DiscordUser executor, DiscordUser target, string reason,
        DateTimeOffset timestamp) => CreateWithoutDuration(ModerationAction.Warn, guild, executor, target, reason, timestamp);

    public static LogEntry CreateForKick(DiscordGuild guild, DiscordUser executor, DiscordUser target, string reason,
        DateTimeOffset timestamp) => CreateWithoutDuration(ModerationAction.Kick, guild, executor, target, reason, timestamp);

    public static LogEntry CreateForBan(DiscordGuild guild, DiscordUser executor, DiscordUser target, string reason,
        DateTimeOffset timestamp, TimeSpan duration) =>
        CreateWithDuration(ModerationAction.Ban, guild, executor, target, reason, timestamp, duration);

    #endregion
}