using DSharpPlus.Entities;

namespace Watchdog.Bot.Models.DataTransfer;

public sealed record WarningData
{
    public required DiscordMember User { get; init; } = default!;

    public required DiscordGuild Guild { get; init; } = default!;

    public required DiscordUser Moderator { get; init; } = default!;
    
    public required string Reason { get; init; } = default!;
}