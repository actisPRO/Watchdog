using System.Reflection;

namespace Watchdog.Bot.Tests.Helpers.Extensions;

public static class ObjectExtensions
{
    public static void SetProperty<T>(this T obj, string propertyName, object value)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        
        Type type = obj.GetType();
        PropertyInfo? property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (property == null)
            throw new ArgumentException($"Property '{propertyName}' not found on type '{type.FullName}'.");

        if (!property.CanWrite || property.GetSetMethod(nonPublic: true) == null)
            throw new ArgumentException($"Property '{propertyName}' does not have a setter.");

        property.SetValue(obj, value);
    }
}