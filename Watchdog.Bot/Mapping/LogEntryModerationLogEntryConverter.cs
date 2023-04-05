using AutoMapper;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Mapping;

public sealed class LogEntryModerationLogEntryConverter : ITypeConverter<LogEntry, ModerationLogEntry>
{
    public ModerationLogEntry Convert(LogEntry source, ModerationLogEntry destination, ResolutionContext context)
    {
        return new()
        {
            GuildId = source.Guild.Id,
            ExecutorId = source.Executor.Id,
            RelatedObjectId = source.RelatedObjectId,
            TargetId = source.Target.Id,
            Action = source.Action,
            Reason = source.Reason,
            Timestamp = source.Timestamp,
            ValidUntil = source.ValidUntil,
            AdditionalData = CreateAdditionalDataJson(source.AdditionalData)
        };
    }

    private static string CreateAdditionalDataJson((string key, string value)[] input)
    {
        var pairs = input.Select(x => $"\"{CreateSafeJsonString(x.key)}\":\"{CreateSafeJsonString(x.value)}\"");
        return "{" + string.Join(",", pairs) + "}";
    }
    
    private static string CreateSafeJsonString(string value)
    {
        return value.Replace("\n", "\\u000a")
            .Replace("\r", "\\u000d")
            .Replace("\t", "\\u0009")
            .Replace("\"", "\\u0022")
            .Replace("\\", "\\u005c");
    }
}