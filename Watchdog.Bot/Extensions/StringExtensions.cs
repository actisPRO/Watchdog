namespace Watchdog.Bot.Extensions;

public static class StringExtensions
{
    public static string WithPrefix(this string input, string prefix, bool addWhitespace = true)
    {
        if (addWhitespace) prefix = prefix + " ";
        return prefix + input;
    }
    
    public static string AsSuccess(this string input)
    {
        return input.WithPrefix(":white_check_mark:");
    }
    
    public static string AsError(this string input)
    {
        return input.WithPrefix(":no_entry:");
    }
}