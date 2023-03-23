namespace Watchdog.Bot.Exceptions;

public sealed class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string message) : base(message)
    {
    }
    
    public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
    public ObjectNotFoundException()
    {
    }
}