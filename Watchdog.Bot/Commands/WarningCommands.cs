using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Strings;

namespace Watchdog.Bot.Commands;

public sealed class WarningCommands : ApplicationCommandModule
{
    private readonly IWarningService _warningService;
    
    public WarningCommands(IWarningService warningService)
    {
        _warningService = warningService.ThrowIfNull();
    }

    [SlashCommand("warn", "Issues a warning to a member")]
    [SlashRequireGuild]
    [SlashCommandPermissions(Permissions.KickMembers)]
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