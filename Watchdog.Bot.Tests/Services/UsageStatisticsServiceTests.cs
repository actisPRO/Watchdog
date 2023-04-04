using AutoFixture;
using FluentAssertions;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Repositories;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Tests.Services;

public sealed class UsageStatisticsServiceTests : DbBaseTest
{
    private IUsageStatisticsService _usageStatisticsService = default!;
    private IUsageStatisticsRepository _usageStatisticsRepository = default!;

    [SetUp]
    public void Setup()
    {
        _usageStatisticsRepository = new UsageStatisticsRepository(Context);
        _usageStatisticsService = new UsageStatisticsService(_usageStatisticsRepository);
    }

    [Test]
    [TestCase(100, 5)]
    [TestCase(20, -50)]
    [TestCase(0, 1)]
    public async Task IncrementByExistingTest(int initialValue, int incrementBy)
    {
        // Arrange
        var guild = await GuildGenerator.GenerateGuildAsync();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var expected = new UsageStatistic
        {
            Key = "test",
            GuildId = guild.Id,
            Date = date,
            Value = initialValue
        };
        await _usageStatisticsRepository.AddAsync(expected);

        // Act
        await _usageStatisticsService.IncrementByAsync("test", guild.Id, incrementBy);
        var actual = await _usageStatisticsRepository.GetByIdAsync("test", guild.Id, date);
        
        // Assert
        actual.Should().NotBeNull();
        actual!.Value.Should().Be(initialValue + incrementBy);
    }
    
    [Test]
    [TestCase(5)]
    [TestCase(-50)]
    [TestCase(1)]
    public async Task IncrementByTest(int incrementBy)
    {
        // Arrange
        var guild = await GuildGenerator.GenerateGuildAsync();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        // Act
        await _usageStatisticsService.IncrementByAsync("test", guild.Id, incrementBy);
        var actual = await _usageStatisticsRepository.GetByIdAsync("test", guild.Id, date);
        
        // Assert
        actual.Should().NotBeNull();
        actual!.Value.Should().Be(incrementBy);
    }
}