using Watchdog.Bot.Strings;

namespace Watchdog.Bot.Constants;

public static class AdditionalDataFields
{
    public const string WarningNumber = "WarningCount";
    
    public static string GetTranslation(string key)
    {
        return key switch
        {
            WarningNumber => Phrases.WarningNumber,
            _ => key
        };
    }
}