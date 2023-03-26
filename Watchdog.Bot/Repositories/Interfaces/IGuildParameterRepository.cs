using Watchdog.Bot.Models.Database;

namespace Watchdog.Bot.Repositories.Interfaces;

public interface IGuildParameterRepository : IRepository<GuildParameter>
{
    Task<GuildParameter?> GetByNameAndGuildIdAsync(string name, ulong guildId);
}