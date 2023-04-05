using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Strings;

namespace Watchdog.Bot.Commands;

[SlashCommandGroup("warn", "Commands for managing warnings")]
public sealed class WarningCommands : ApplicationCommandModule
{
    private readonly IWarningService _warningService;

    public WarningCommands(IWarningService warningService)
    {
        _warningService = warningService;
    }

    [SlashCommand("add", "Adds a warning to a member")]
    public async Task AddWarning(InteractionContext ctx, 
        [Option("member", "Member to warn")] DiscordUser user,
        [Option("reason", "Warning reason")] string reason)
    {
        var warningData = new WarningData()
        {
            Moderator = ctx.Member,
            User = (DiscordMember)user,
            Guild = ctx.Guild,
            Reason = reason
        };
        var warningCount = await _warningService.WarnMemberAsync(warningData);

        var message = string.Format(Phrases.WarningModeratorConfirmation, user.ToNiceString(), warningCount).AsSuccess();
        await ctx.CreateResponseAsync(message, ephemeral: true);
    }
}