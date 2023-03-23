namespace Watchdog.Bot.Options;

public sealed record DatabaseOptions
{
    public const string SectionName = "Database";
    
    public required string ConnectionString { get; init; }
}