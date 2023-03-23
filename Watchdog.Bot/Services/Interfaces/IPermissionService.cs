using Watchdog.Bot.Models;
using Watchdog.Bot.Models.Database;

namespace Watchdog.Bot.Services.Interfaces;

public interface IPermissionService
{
    Task RegisterPermissionAsync(Permission permission);
}