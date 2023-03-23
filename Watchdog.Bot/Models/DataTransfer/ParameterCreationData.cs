namespace Watchdog.Bot.Models.DataTransfer;

public record ParameterCreationData<T> where T : IConvertible
{
    public required string Name { get; set; }
    
    public required T Value { get; set; }
}