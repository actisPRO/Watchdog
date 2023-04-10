using DSharpPlus;
using DSharpPlus.Entities;

namespace Watchdog.Bot.Services.Interfaces;

public class BotStatus : IBotStatus
{
    public async Task UpdateBotStatusAsync(DiscordClient client, bool updateCache = false)
    {
        var membersCount = 0;

        foreach (var guild in client.Guilds.Values)
        {
            if (updateCache)
            {
                var guildReceived = await client.GetGuildAsync(guild.Id);
                membersCount += guildReceived.Members.Count(x => !x.Value.IsBot);
            }
            else
            {
                membersCount += guild.Members.Count(x => !x.Value.IsBot);
            }
        }
        
        await client.UpdateStatusAsync(new DiscordActivity($"over {membersCount} members!",
            ActivityType.Watching));
    }
}