namespace Watchdog.Bot.Models.DataTransfer;

public sealed record ParameterResponseData<T> where T : IConvertible
{
    public required string Name { get; init; }
    
    public required T Value { get; init; }
    
    public required T DefaultValue { get; init; }
}