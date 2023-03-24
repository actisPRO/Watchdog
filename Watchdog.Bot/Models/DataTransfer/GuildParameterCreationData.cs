namespace Watchdog.Bot.Models.DataTransfer;

public sealed record GuildParameterCreationData<T>
{
    public required string Name { get; init; }
    
    public required ulong GuildId { get; init; }
    
    public required T Value { get; init; }
}