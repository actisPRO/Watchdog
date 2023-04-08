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
public sealed class MuteCommands : ApplicationCommandModule
{
    private readonly IMuteService _muteService;

    public MuteCommands(IMuteService muteService)
    {
        _muteService = muteService;
    }


    [SlashCommand("mute", "Mute member on a server")]
    public async Task TimeoutMember(InteractionContext ctx,
        [Option("member", "Member to mute")] DiscordUser user,
        [Option("time", "Mute time in [XhYmZs] format")]
        string duration,
        [Option("reason", "Mute reason")] string reason)
    {
        var username = user.ToNiceString();
        await _muteService.MuteMemberAsync((DiscordMember)user, ctx.Member, duration, reason);
        var message = string.Format(Phrases.ModerationConfirmation_Timeout, username).AsSuccess();
        await ctx.CreateResponseAsync(message, true);
    }

    [SlashCommand("unmute", "Unmute member on a server")]
    public async Task RemoveMemberTimeout(InteractionContext ctx,
        [Option("member", "Member to unmute")] DiscordUser user,
        [Option("reason", "Unmute reason")] string reason)
    {
        var username = user.ToNiceString();
        await _muteService.UnmuteMemberAsync((DiscordMember)user, ctx.Member, reason);
        var message = string.Format(Phrases.ModerationConfirmation_RemoveTimeout, username).AsSuccess();
        await ctx.CreateResponseAsync(message, true);
    }
}