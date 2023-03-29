using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Services;

public sealed class MessageLogService : IMessageLogService
{
    public async Task LogDeletedMessage(DiscordGuild guild, DiscordMessage message, ulong messageLogsChannelId)
    {
        var messageLogChannel = guild.GetChannel(messageLogsChannelId);
        if (messageLogChannel == null) return; // If channel doesn't exist - return

        var deleteLog = (await guild.GetAuditLogsAsync(action_type: AuditLogActionType.MessageDelete)).OfType<DiscordAuditLogMessageEntry?>().FirstOrDefault(x => x != null && x.Target.Id == message.Author.Id);

        var logMessage = new DiscordMessageBuilder();

        var embed = new DiscordEmbedBuilder()
            .WithDescription("Сообщение было удалено")
            .AddField("**Удалённое сообщение:**", $"```{message.Content}```")
            .AddTagretFields(message.Author, message.Channel, deleteLog)
            .WithFooter($"Id сообщения: {message.Id}")
            .WithColor(new DiscordColor("ff6d96"))
            .WithTimestamp(DateTime.Now);
        
        List<DiscordEmbed> embeds = new() { embed };

        if (message.Attachments.Count != 0)
        {
            await messageLogChannel.SendMessageAsync(new DiscordMessageBuilder().AddEmbed(embed));
            embeds = await BuildLogAttachmentEmbeds(logMessage, message, deleteLog);
        }
        
        logMessage.AddEmbeds(embeds);
        await messageLogChannel.SendMessageAsync(logMessage);
    }

    private static async Task<List<DiscordEmbed>> BuildLogAttachmentEmbeds(DiscordMessageBuilder logMessage, DiscordMessage message, DiscordAuditLogMessageEntry? deleteLog)
    {
        List<DiscordEmbed> embeds = new();
        int attachmentCount = 1;
            
        foreach (var attachment in message.Attachments)
        {
            using HttpClient httpClient = new();
            var memoryStream = await httpClient.GetStreamAsync(attachment.Url);

            logMessage.AddFile($"image{attachmentCount}.png", memoryStream);

            var attachmentEmbed = new DiscordEmbedBuilder()
                .WithDescription($"**Вложение #{attachmentCount}**")
                .WithImageUrl($"attachment://image{attachmentCount}.png")
                .WithColor(new DiscordColor("ff6d96"));
                
            // Need to define author if user send 10 images because message can't have more than 10 embeds
            if (message.Attachments.Count == 10 && attachmentCount == 1)
            {
                attachmentEmbed.AddTagretFields(message.Author, message.Channel, deleteLog);
                embeds.Clear();
                embeds.Add(attachmentEmbed);
            }
            else
            {
                embeds.Add(attachmentEmbed);
            }

            attachmentCount++;
        }

        return embeds;
    }

    public async Task LogUpdatedMessage(DiscordGuild guild, DiscordMessage message, DiscordMessage messageAfter, ulong messageLogsChannelId)
    {
        var messageLogChannel = guild.GetChannel(messageLogsChannelId);
        if (messageLogChannel == null) return; // If channel doesn't exist - return

        var logMessage = new DiscordMessageBuilder()
            .AddEmbed(new DiscordEmbedBuilder()
                .WithTimestamp(DateTime.Now)
                .WithDescription($"[**Сообщение**]({messageAfter.JumpLink}) было отредактировано")
                .AddField("**Старое содержимое:**", $"```{message.Content}```")
                .AddField("**Новое содержимое:**", $"```{messageAfter.Content}```")
                .AddField("**Автор**", $"{messageAfter.Author.Username}#{messageAfter.Author.Discriminator} ({messageAfter.Author.Mention})", true)
                .AddField("**Канал**", $"{messageAfter.Channel.Name} ({messageAfter.Channel.Mention})", true)
                .WithFooter($"Id сообщения: {messageAfter.Id}")
                .WithColor(new DiscordColor("60afff")));

        await messageLogChannel.SendMessageAsync(logMessage);
    }

    public async Task LogBulkDeletedMessages(DiscordGuild guild, IReadOnlyList<DiscordMessage> messages, ulong messageLogsChannelId)
    {
        var messageLogChannel = guild.GetChannel(messageLogsChannelId);
        if (messageLogChannel == null) return; // If channel doesn't exist - return
        
        var deleteLog = (await guild.GetAuditLogsAsync(action_type: AuditLogActionType.MessageBulkDelete)).OfType<DiscordAuditLogMessageEntry?>().FirstOrDefault();

        var logMessage = new DiscordMessageBuilder()
            .AddEmbed(new DiscordEmbedBuilder()
                .WithDescription($"Очищено **{messages.Count}** сообщений")
                .AddBulkFields(messages.First().Channel, deleteLog)
                .WithColor(new DiscordColor("60afff"))
                .WithTimestamp(DateTime.Now));

        await messageLogChannel.SendMessageAsync(logMessage);
    }
}