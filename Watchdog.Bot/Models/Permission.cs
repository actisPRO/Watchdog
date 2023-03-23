using System.ComponentModel.DataAnnotations;
using DSharpPlus;
using Watchdog.Bot.Enums;

namespace Watchdog.Bot.Models;

public sealed class Permission : IEntity
{
    [Key]
    public required RestrictedAction RestrictedAction { get; set; }
    
    public required Permissions RequiredPermission { get; set; }
    
    public object[] GetIdentity() => new object[] { RestrictedAction };
}