using DSharpPlus.Entities;

namespace Watchdog.Bot.Extensions;

public static class DiscordGuildExtensions
{
    public static string ToNiceString(this DiscordGuild guild) => $"{guild.Name} ({guild.Id})";
}