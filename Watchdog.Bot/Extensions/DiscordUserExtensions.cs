using DSharpPlus.Entities;

namespace Watchdog.Bot.Extensions;

public static class DiscordUserExtensions
{
    public static string ToNiceString(this DiscordUser user)
    {
        return $"{user.Username}#{user.Discriminator} ({user.Id})";
    }

    public static string FullName(this DiscordUser user)
    {
        return $"{user.Username}#{user.Discriminator}";
    }
}