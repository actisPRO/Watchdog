namespace Watchdog.Bot.Options;

public sealed record DiscordOptions
{
    public const string SectionName = "Discord";
    
    public required string Token { get; init; }
    public bool Debug { get; set; } = false;
    public ulong? DebugGuild { get; set; }
}