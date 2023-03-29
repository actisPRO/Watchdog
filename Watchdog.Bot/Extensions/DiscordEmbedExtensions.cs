using DSharpPlus.Entities;

namespace Watchdog.Bot.Extensions;

public static class DiscordEmbedExtensions
{
    /// <summary>
    /// Add three fields.
    /// Made for message logging.
    /// </summary>
    public static DiscordEmbedBuilder AddTagretFields(this DiscordEmbedBuilder embed, DiscordUser? author, DiscordChannel channel, DiscordAuditLogMessageEntry? deleteLog)
    {
        embed.AddField("**Автор**", $"**{author.Username}#{author.Discriminator}** ({author.Mention})", true);
        embed.AddField("**Удалил**", $"**{deleteLog?.UserResponsible.Username ?? author.Username}#{deleteLog?.UserResponsible.Discriminator ?? author.Discriminator}** ({deleteLog?.UserResponsible.Mention ?? author.Mention})", true);
        embed.AddField("**Канал**", $"**{channel.Name}** ({channel.Mention})", true);
        return embed;
    }
    
    public static DiscordEmbedBuilder AddBulkFields(this DiscordEmbedBuilder embed, DiscordChannel channel, DiscordAuditLogMessageEntry? deleteLog)
    {
        embed.AddField("**Удалил**",
            deleteLog?.UserResponsible == null
                ? $"**Неизвестно**"
                : $"**{deleteLog?.UserResponsible.Username}#{deleteLog?.UserResponsible.Discriminator}** ({deleteLog?.UserResponsible.Mention})", true);
        embed.AddField("**Канал**", $"**{channel.Name}** ({channel.Mention})", true);
        return embed;
    }
}