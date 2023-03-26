using System.Reflection;
using Watchdog.Bot.Attributes;

namespace Watchdog.Bot.Models.Utilities;

public struct ListenerMethod
{
    public required MethodInfo Method { get; init; }
    public required AsyncEventListenerAttribute Attribute { get; init; }
}