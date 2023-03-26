using System.Reflection;
using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AsyncEventListenerAttribute : Attribute
{
    public EventType Event { get; }

    public AsyncEventListenerAttribute(EventType eventType)
    {
        Event = eventType;
    }

    public static AsyncEventListenerAttribute? GetAttribute(MethodInfo methodInfo)
    {
        return methodInfo.GetCustomAttribute<AsyncEventListenerAttribute>();
    }
}