using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Repositories;

public sealed class GuildParameterRepository : Repository<GuildParameter>, IGuildParameterRepository
{
    public GuildParameterRepository(DbContext context) : base(context)
    {
    }

    public async Task<GuildParameter?> GetByNameAndGuildIdAsync(string name, ulong guildId)
    {
        return await Context.Set<GuildParameter>()
            .AsNoTracking().Include(x => x.Parameter).FirstOrDefaultAsync(x => x.GuildId == guildId && x.Name == name);
    }

    public override async Task<GuildParameter?> GetByIdAsync(params object[] identity)
    {
        try
        {
            if (identity.Length != 2) throw new ArgumentException("Invalid amount of identity parameters", nameof(identity));

            var name = (string)identity[0];
            var guildId = (ulong)identity[1];

            return await GetByNameAndGuildIdAsync(name, guildId);
        }
        catch (InvalidCastException)
        {
            throw new ArgumentException("Invalid identity parameters", nameof(identity));
        }
    }
}