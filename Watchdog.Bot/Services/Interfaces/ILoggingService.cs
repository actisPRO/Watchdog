using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Services.Interfaces;

public interface ILoggingService
{
    Task LogAsync(LogEntry entry);
}