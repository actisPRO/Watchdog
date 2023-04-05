using FluentAssertions;
using Watchdog.Bot.Utils;

namespace Watchdog.Bot.Tests.Utils;

public sealed class TimeUtilsTests
{
    [Test]
    [TestCase("1d10h30m", 1, 10, 30, 0)]
    [TestCase("1d30m", 1, 0, 30, 0)]
    [TestCase("30m10s", 0, 0, 30, 10)]
    [TestCase("7d10h30m", 7, 10, 30, 0)]
    public void GetFutureDate_ReturnsExpectedDate(string input, int expectedDay, int expectedHour, int expectedMinute,
        int expectedSecond)
    {
        // Arrange
        var expected = DateTime.UtcNow.Add(new(expectedDay, expectedHour, expectedMinute, expectedSecond));

        // Act
        var futureDate = TimeParserUtils.GetFutureDate(input);

        // Assert
        futureDate.Year.Should().Be(expected.Year);
        futureDate.Month.Should().Be(expected.Month);
        futureDate.Day.Should().Be(expected.Day);
        futureDate.Minute.Should().Be(expected.Minute);
        futureDate.Second.Should().Be(expected.Second);
    }

    [Test]
    [TestCase("1d2h30m", 1, 2, 30, 0)]
    [TestCase("5s", 0, 0, 0, 5)]
    [TestCase("1d", 1, 0, 0, 0)]
    [TestCase("30m10s", 0, 0, 30, 10)]
    [TestCase("7d10h30m", 7, 10, 30, 0)]
    [TestCase("30m", 0, 0, 30, 0)]
    public void ParseTimeSpan_ShouldReturnCorrectTimeSpan(string input, int expectedDay, int expectedHour,
        int expectedMinute, int expectedSecond)
    {
        // Arrange
        var expected = new TimeSpan(expectedDay, expectedHour, expectedMinute, expectedSecond);

        // Act
        var result = TimeParserUtils.ParseTimeSpan(input);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    [TestCase("input")]
    [TestCase("1d2h30m40sinvalid")]
    [TestCase("1d2h30m40")]
    public void ParseTimeSpan_ShouldThrowExceptionForInvalidInput(string input)
    {
        // Assert: Expects exception

        var result = () => TimeParserUtils.ParseTimeSpan(input);

        result.Should().Throw<InvalidOperationException>();
    }
}