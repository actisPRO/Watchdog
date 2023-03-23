using System.ComponentModel.DataAnnotations.Schema;
using DSharpPlus;
using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Models;

[PrimaryKey(nameof(GuildId), nameof(RestrictedAction))]
public sealed class PermissionOverride : IEntity
{
    public required ulong GuildId { get; set; }

    public Guild Guild { get; set; } = default!;

    [ForeignKey(nameof(Permission))] 
    public required RestrictedAction RestrictedAction { get; set; }

    public Permission Permission { get; set; } = default!;

    public required Permissions Override { get; set; }

    public object[] GetIdentity() => new object[] { GuildId, RestrictedAction };
}