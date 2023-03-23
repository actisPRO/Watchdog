using System.Reflection;

namespace Watchdog.Bot.Tests.Helpers.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class NonTransactionalAttribute : Attribute
{
    public static bool IsNonTransactional(Type type) 
        => type.GetCustomAttributes(typeof(NonTransactionalAttribute), true).Any();

    public static bool IsNonTransactional(MethodInfo methodInfo) 
        => methodInfo.GetCustomAttributes(typeof(NonTransactionalAttribute), true).Any();
}