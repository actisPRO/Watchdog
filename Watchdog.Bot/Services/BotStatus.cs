using DSharpPlus;
using DSharpPlus.Entities;

namespace Watchdog.Bot.Services.Interfaces;

public class BotStatus : IBotStatus
{
    public async Task UpdateBotStatusAsync(DiscordClient client)
    {
        var membersCount = 0;

        foreach (var guild in client.Guilds.Values)
        {
            membersCount += guild.Members.Count(x => !x.Value.IsBot);
        }
        
        await client.UpdateStatusAsync(new DiscordActivity($"over {membersCount} members!",
            ActivityType.Watching));
    }
}