using FluentAssertions;
using Watchdog.Bot.Utils;

namespace Watchdog.Bot.Tests.Utils;

public sealed class IdGeneratorTest
{
    [Test]
    public void GenerateIdTest()
    {
        var id = IdGenerator.GenerateId();
        var id2 = IdGenerator.GenerateId();

        id.Should().NotBeEmpty();
        id2.Should().NotBeEmpty();
        id.Should().NotBe(id2);
    }
}