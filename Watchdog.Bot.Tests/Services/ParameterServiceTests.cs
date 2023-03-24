using System.Globalization;
using FluentAssertions;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Repositories;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Tests.Services;

public sealed class ParameterServiceTests : DbBaseTest
{
    private IParameterService _parameterService = default!;
    private IParameterRepository _parameterRepository = default!;
    private IGuildParameterRepository _guildParameterRepository = default!;
    
    [OneTimeSetUp]
    public void TestOneTimeSetUp()
    {
        _parameterRepository = new ParameterRepository(Context);
        _guildParameterRepository = new GuildParameterRepository(Context);
        _parameterService = new ParameterService(CreateLogger<ParameterService>(), Mapper, _parameterRepository, _guildParameterRepository);
    }

    [Test]
    [TestCase("some text")]
    [TestCase(156)]
    [TestCase(156.5f)]
    [TestCase(true)]
    [TestCase((ulong) 123213)]
    public async Task RegisterParameterAsync_Create_Test<T>(T value) where T : IConvertible
    {
        // Arrange
        var parameter = new ParameterCreationData<T>
        {
            Name = Guid.NewGuid().ToString(),
            Value = value
        };

        // Act
        await _parameterService.RegisterParameterAsync(parameter);
        var result = await _parameterRepository.GetByIdAsync(parameter.Name);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(parameter.Name);
        result.Value.Should().Be(Convert.ToString(parameter.Value, CultureInfo.InvariantCulture));
        result.Type.Should().Be(typeof(T).Name);
    }

    [Test]
    [TestCase("some text", "new text")]
    [TestCase(156, 200)]
    [TestCase(156.5f, 254f)]
    [TestCase(true, false)]
    [TestCase((ulong) 123213, (ulong) 123)]
    public async Task RegisterParameterAsync_Update_Test<T>(T initialValue, T newValue) where T : IConvertible
    {
        // Arrange
        var parameter = new ParameterCreationData<T>
        {
            Name = Guid.NewGuid().ToString(),
            Value = initialValue
        };
        await _parameterService.RegisterParameterAsync(parameter);
        
        // Act
        parameter.Value = newValue;
        await _parameterService.RegisterParameterAsync(parameter);
        var result = await _parameterRepository.GetByIdAsync(parameter.Name);
        
        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(parameter.Name);
        result.Value.Should().Be(Convert.ToString(parameter.Value, CultureInfo.InvariantCulture));
        result.Type.Should().Be(typeof(T).Name);
    }

    [Test]
    public async Task GetParameterAsync_And_Override_Test()
    {
        // Arrange
        var parameter = new ParameterCreationData<ulong>()
        {
            Name = Guid.NewGuid().ToString(),
            Value = 123
        };
        await _parameterService.RegisterParameterAsync(parameter);

        var guild = await GuildGenerator.GenerateGuildAsync();
        var guildParameter = new GuildParameterCreationData<ulong>()
        {
            Name = parameter.Name,
            GuildId = guild.Id,
            Value = 456
        };
        

        // Act
        await _parameterService.OverrideParameterValueAsync(guildParameter);
        var actual = await _parameterService.GetGuildParameterValueAsync<ulong>(parameter.Name, guild.Id);
        
        // Assert
        actual.Name.Should().Be(guildParameter.Name);
        actual.Value.Should().Be(guildParameter.Value);
        actual.DefaultValue.Should().Be(parameter.Value);
    }

    [Test]
    public async Task GetParameterAsync_Default_Test()
    {
        // Arrange
        var parameter = new ParameterCreationData<ulong>()
        {
            Name = Guid.NewGuid().ToString(),
            Value = 123
        };
        await _parameterService.RegisterParameterAsync(parameter);
        
        // Act
        var actual = await _parameterService.GetGuildParameterValueAsync<ulong>(parameter.Name, 123);
        
        // Assert
        actual.Name.Should().Be(parameter.Name);
        actual.Value.Should().Be(parameter.Value);
        actual.DefaultValue.Should().Be(parameter.Value);
    }
}