using DSharpPlus;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Services;

public sealed class WarningService : IWarningService
{
    public async Task WarnMemberAsync(DiscordClient client, WarningData warningData)
    {
        throw new NotImplementedException();
    }
}