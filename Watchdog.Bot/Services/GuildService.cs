using System.Diagnostics;
using DSharpPlus.Entities;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Services;

public sealed class GuildService : IGuildService
{
    private readonly ILogger<GuildService> _logger;
    private readonly IGuildRepository _guildRepository;

    public GuildService(ILogger<GuildService> logger, IGuildRepository guildRepository)
    {
        _logger = logger;
        _guildRepository = guildRepository;
    }

    public async Task CreateOrUpdateGuildAsync(DiscordGuild guild)
    {
        var guildEntity = await _guildRepository.GetByIdAsync(guild.Id);

        if (guildEntity == null)
        {
            await _guildRepository.AddAsync(new()
            {
                Id = guild.Id,
                CreatedAt = guild.CreationTimestamp
            });
            
            _logger.LogInformation("Guild {GuildName} ({GuildId}) was added to the database", guild.Name, guild.Id);
        }
    }
}