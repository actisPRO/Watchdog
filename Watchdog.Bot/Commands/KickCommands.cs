using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Strings;

namespace Watchdog.Bot.Commands;

[SlashRequireGuild]
[SlashCommandPermissions(Permissions.KickMembers)]
public sealed class KickCommands : ApplicationCommandModule
{
    private readonly IKickService _kickService;

    public KickCommands(IKickService kickService)
    {
        _kickService = kickService.ThrowIfNull();
    }
    
    [SlashCommand("kick", "Kicks a member from the server")]
    public async Task KickMember(InteractionContext ctx, 
        [Option("member", "Member to kick")] DiscordUser user,
        [Option("reason", "Kick reason")] string reason)
    {
        var username = user.ToNiceString();
        await _kickService.KickMemberAsync((DiscordMember)user, ctx.Member, reason);
        var message = string.Format(Phrases.ModeratorConfirmation_Kick, username).AsSuccess();
        await ctx.CreateResponseAsync(message, ephemeral: true);
    }
}