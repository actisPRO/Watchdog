using AutoFixture;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Repositories;
using Watchdog.Bot.Repositories.Interfaces;

namespace Watchdog.Bot.Tests.Helpers.Generators;

public sealed class GuildGenerator
{
    private readonly IGuildRepository _guildRepository;

    public GuildGenerator(DatabaseContext context)
    {
        _guildRepository = new GuildRepository(context);
    }

    public async Task<Guild> GenerateGuildAsync()
    {
        var fixture = new Fixture()
            .Build<Guild>()
            .With(x => x.CreatedAt, DateTimeOffset.UtcNow)
            .Without(x => x.DatabaseEntryCreatedAt)
            .Create();
        return await _guildRepository.AddAsync(fixture);
    }
}