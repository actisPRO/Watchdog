namespace Watchdog.Bot.Exceptions;

public sealed class ObjectExistsException : Exception
{
    public ObjectExistsException(string message) : base(message)
    {
    }
    
    public ObjectExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
    public ObjectExistsException()
    {
    }
}