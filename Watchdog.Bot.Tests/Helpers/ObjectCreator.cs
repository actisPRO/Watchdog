using System.Reflection;

namespace Watchdog.Bot.Tests.Helpers;

public static class ObjectCreator
{
    public static T CreateInstance<T>() where T : class
    {
        Type type = typeof(T);
        ConstructorInfo? constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

        if (constructor == null)
            throw new ArgumentException($"Type '{type.FullName}' does not have an internal constructor.");

        T? instance = constructor.Invoke(null) as T;
        if (instance == null)
            throw new Exception($"Failed to create instance of type '{type.FullName}'.");

        return instance;
    }
}