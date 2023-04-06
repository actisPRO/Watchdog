using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Services.Interfaces;

public interface IWarningService
{
    Task<int> WarnMemberAsync(WarningData warningData);
}