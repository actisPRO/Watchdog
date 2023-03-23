namespace Watchdog.Bot.Extensions;

public static class ObjectExtensions
{
    public static T ThrowIfNull<T>(this T? obj)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));
        return obj;
    }
}