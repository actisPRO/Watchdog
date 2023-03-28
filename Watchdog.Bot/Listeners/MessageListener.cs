using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Options;
using Watchdog.Bot.Attributes;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Events;
using Watchdog.Bot.Options;

namespace Watchdog.Bot.Listeners;

public sealed class MessageListener : BaseEventManager
{
    private readonly DiscordOptions _discordOptions;

    public MessageListener(IOptions<DiscordOptions> discordOptions)
    {
        _discordOptions = discordOptions.Value;
    }
    
    [AsyncEventListener(EventType.MessageDeleted)]
    public async Task ClientOnMessageDeleteReceived(DiscordClient client, MessageDeleteEventArgs args)
    {
        // Don't log messages from bots
        if (args.Message.Author.IsBot) return;

        var deleteLog = (await args.Guild.GetAuditLogsAsync(action_type: AuditLogActionType.MessageDelete)).Select(x => x as DiscordAuditLogMessageEntry).FirstOrDefault(x => x.Target.Id == args.Message.Author.Id);

        var logMessage = new DiscordMessageBuilder();

        var embed = new DiscordEmbedBuilder()
            .WithDescription("Сообщение было удалено")
            .AddField("**Удалённое сообщение:**", $"```{args.Message.Content}```")
            .AddField("**Автор**", $"**{args.Message.Author.Username}** ({args.Message.Author.Mention})", true)
            .AddField("**Удалил**", $"**{deleteLog?.UserResponsible.Username ?? args.Message.Author.Username}** ({deleteLog?.UserResponsible.Mention ?? args.Message.Author.Mention})", true)
            .AddField("**Канал**", $"**{args.Channel.Name}** ({args.Channel.Mention})", true)
            .WithFooter($"Id сообщения: {args.Message.Id}")
            .WithColor(new DiscordColor("ff6d96"));
        
        List<DiscordEmbed> embeds = new() { embed };

        if (args.Message.Attachments.Count != 0)
        {
            int i = 1;
            
            foreach (var attachment in args.Message.Attachments)
            {
                using HttpClient httpClient = new();
                var memoryStream = await httpClient.GetStreamAsync(attachment.Url);

                logMessage.AddFile($"image{i}.png", memoryStream);

                var attachmentEmbed = new DiscordEmbedBuilder()
                    .WithDescription($"**Вложение #{i}**")
                    .WithImageUrl($"attachment://image{i}.png")
                    .WithColor(new DiscordColor("ff6d96"));
                
                // Need to define author if user send 10 images because message can't have more than 10 embeds
                if (args.Message.Attachments.Count == 10 && i == 1)
                {
                    attachmentEmbed.AddField("**Автор**", $"**{args.Message.Author.Username}** ({args.Message.Author.Mention})");
                    attachmentEmbed.AddField("**Удалил**", $"**{deleteLog?.UserResponsible.Username ?? args.Message.Author.Username}** ({deleteLog?.UserResponsible.Mention ?? args.Message.Author.Mention})", true);
                    attachmentEmbed.AddField("**Канал**", $"**{args.Channel.Name}** ({args.Channel.Mention})", true);
                    
                    await args.Guild.GetChannel(_discordOptions.MessageLogsChannelId).SendMessageAsync(new DiscordMessageBuilder().AddEmbed(embed));
                    embeds.Clear();
                    embeds.Add(attachmentEmbed);
                }
                else
                {
                    embeds.Add(attachmentEmbed);
                }

                i++;
            }
        }
        
        logMessage.AddEmbeds(embeds);
        await args.Guild.GetChannel(_discordOptions.MessageLogsChannelId).SendMessageAsync(logMessage);
    }
}