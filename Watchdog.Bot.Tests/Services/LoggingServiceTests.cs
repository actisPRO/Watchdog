using DSharpPlus.Entities;
using FluentAssertions;
using Moq;
using Watchdog.Bot.Constants;
using Watchdog.Bot.Enums;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services;
using Watchdog.Bot.Services.Interfaces;
using Watchdog.Bot.Tests.Helpers;
using Watchdog.Bot.Tests.Helpers.Extensions;

namespace Watchdog.Bot.Tests.Services;

public sealed class LoggingServiceTests : DbBaseTest
{
    private readonly Mock<IParameterService> _parameterServiceMock = new();

    private ILoggingService _loggingService = default!;
    private ILogRepository _logRepository = default!;

    [OneTimeSetUp]
    public void TestOneTimeSetUp()
    {
        _parameterServiceMock.Setup(x =>
                x.GetGuildParameterValueAsync<ulong>(It.Is<string>(s => s == ParameterNames.ModerationLogChannelId), 
                    It.IsAny<ulong>()))
            .ReturnsAsync(new ParameterResponseData<ulong>()
            {
                Name = ParameterNames.ModerationLogChannelId,
                Value = 1,
                DefaultValue = 0,
            });

        _logRepository = new LogRepository(Context);
        _loggingService = new LoggingService(CreateLogger<LoggingService>(), Mapper,
            _logRepository, _parameterServiceMock.Object);
    }

    [Test]
    public async Task LogAsyncTest_WithoutChannelLogging()
    {
        // Arrange
        var guild = await CreateTestGuild();
        var moderator = CreateTestUser(1, "Moderator", "0001");
        var user = CreateTestUser(2, "User", "0002");
        var timestamp = DateTimeOffset.UtcNow;

        var entry = LogEntry.CreateForWarning(guild, "ABC", moderator, user, "Test reason", timestamp, 5);
        
        // Act
        await _loggingService.LogAsync(entry);
        var actualEntry = (await _logRepository.GetAllAsync()).FirstOrDefault();

        // Assert
        actualEntry.Should().NotBeNull();
        actualEntry!.Action.Should().Be(ModerationAction.Warn);
        actualEntry.RelatedObjectId.Should().Be("ABC");
        actualEntry.GuildId.Should().Be(guild.Id);
        actualEntry.ExecutorId.Should().Be(moderator.Id);
        actualEntry.TargetId.Should().Be(user.Id);
        actualEntry.Reason.Should().Be("Test reason");
        actualEntry.ValidUntil.Should().BeNull();
        actualEntry.AdditionalData.Should().Be($"{{\"{AdditionalDataFields.WarningNumber}\":\"5\"}}");
    }

    private async Task<DiscordGuild> CreateTestGuild()
    {
        var dbGuild = await GuildGenerator.GenerateGuildAsync();
        
        var guild = ObjectCreator.CreateInstance<DiscordGuild>();
        guild.SetProperty(nameof(guild.Id), dbGuild.Id);
        
        return guild;
    }

    private DiscordUser CreateTestUser(ulong id, string username, string discriminator)
    {
        var user = ObjectCreator.CreateInstance<DiscordUser>();
        user.SetProperty(nameof(user.Id), id);
        user.SetProperty(nameof(user.Username), username);
        user.SetProperty(nameof(user.Discriminator), discriminator);

        return user;
    }
}