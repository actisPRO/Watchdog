using DSharpPlus.Entities;

namespace Watchdog.Bot.Models.DataTransfer;

public sealed record WarningData
{
    public DiscordUser User { get; init; } = default!;

    public DiscordGuild Guild { get; init; } = default!;

    public DiscordUser Moderator { get; init; } = default!;
    
    public string Reason { get; init; } = default!;
}