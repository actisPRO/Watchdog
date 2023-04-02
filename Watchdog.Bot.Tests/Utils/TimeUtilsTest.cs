using FluentAssertions;

namespace Watchdog.Bot.Utils.Tests;

public class TimeUtilsTests
{

    
    [Test]
    [TestCase("1d10h30m", 1, 10, 30, 0)]
    [TestCase("1d30m", 1, 0, 30, 0)]
    [TestCase("30m10s", 0, 0, 30, 10)]
    [TestCase("7d10h30m", 7, 10, 30, 0)]
    public void GetFutureDate_ReturnsExpectedDate(string input, int expectedDay, int expectedHour, int expectedMinute, int expectedSecond)
    {
        // Arrange
        var expected = DateTime.UtcNow.Add(new TimeSpan(expectedDay, expectedHour, expectedMinute, expectedSecond));

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
    [TestCase("30m", 0,0,30, 0)]
    public void ParseTimeSpan_ShouldReturnCorrectTimeSpan(string input, int expectedDay, int expectedHour, int expectedMinute, int expectedSecond)
    {
        // Arrange
        var expected = new TimeSpan(expectedDay, expectedHour, expectedMinute, expectedSecond);
        
        // Act
        var result = TimeParserUtils.ParseTimeSpan(input);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void FormatTimespan_ShouldReturnFormattedString()
    {
        // Arrange
        var time = new TimeSpan(1, 2, 30, 10);
        const string expected = "1 день 2 часа 30 минут 10 секунд";

        // Act
        var result = TimeParserUtils.FormatTimespan(time);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Be(expected);
    }

    [Test]
    [TestCase(TimeParserUtils.TimeUnit.Days, "1 день")]
    [TestCase(TimeParserUtils.TimeUnit.Hours, "2 часа")]
    [TestCase(TimeParserUtils.TimeUnit.Minutes, "30 минут")]
    [TestCase(TimeParserUtils.TimeUnit.Seconds, "0 секунд")]
    public void ToCorrectCase_ShouldReturnCorrectCase(TimeParserUtils.TimeUnit timeUnit, string expected)
    {
        // Arrange
        var time = new TimeSpan(1, 2, 30, 0);

        // Act
        var result = TimeParserUtils.ToCorrectCase(time, timeUnit);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Be(expected);
    }

    [Test]
    [TestCase(TimeParserUtils.TimeUnit.Days, "1 день")]
    [TestCase(TimeParserUtils.TimeUnit.Hours, "1 час")]
    [TestCase(TimeParserUtils.TimeUnit.Minutes, "1 минута")]
    [TestCase(TimeParserUtils.TimeUnit.Seconds, "1 секунда")]
    public void ToCorrectCase_ShouldHandleSingularNominativeCase(TimeParserUtils.TimeUnit timeUnit, string expected)
    {
        // Arrange
        var time = new TimeSpan(1, 1, 1, 1);
        
        // Act
        var result = TimeParserUtils.ToCorrectCase(time, timeUnit);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Be(expected);
    }

    [Test]
    [TestCase(TimeParserUtils.TimeUnit.Days, "2 дня")]
    [TestCase(TimeParserUtils.TimeUnit.Hours, "2 часа")]
    [TestCase(TimeParserUtils.TimeUnit.Minutes, "2 минуты")]
    [TestCase(TimeParserUtils.TimeUnit.Seconds, "2 секунды")]
    public void ToCorrectCase_ShouldHandlePluralGenitiveCase(TimeParserUtils.TimeUnit timeUnit, string expected)
    {
        // Arrange
        var time = new TimeSpan(2, 2, 2, 2);
        
        // Act
        var result = TimeParserUtils.ToCorrectCase(time, timeUnit);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Be(expected);
    }

    [Test]
    [TestCase(TimeParserUtils.TimeUnit.Days, "13 дней")]
    [TestCase(TimeParserUtils.TimeUnit.Hours, "13 часов")]
    [TestCase(TimeParserUtils.TimeUnit.Minutes, "13 минут")]
    [TestCase(TimeParserUtils.TimeUnit.Seconds, "13 секунд")]
    public void ToCorrectCase_ShouldHandlePluralGenitiveCaseWithExceptions(TimeParserUtils.TimeUnit timeUnit, string expected)
    {
        // Arrange
        var time = new TimeSpan(13, 13, 13, 13);
        
        // Act
        var result = TimeParserUtils.ToCorrectCase(time, timeUnit);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Be(expected);
    }
    

    [Test]
    [TestCase(TimeParserUtils.TimeUnit.Seconds, "0 секунд")]
    public void ToCorrectCase_ShouldHandlePluralGenitiveCaseForZero(TimeParserUtils.TimeUnit timeUnit, string expected)
    {
        // Arrange
        var time = new TimeSpan(0, 0, 0);

        // Act
        var result = TimeParserUtils.ToCorrectCase(time, timeUnit);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Be(expected);
    }

    [Test]
    public void ParseTimeSpan_ShouldThrowExceptionForInvalidInput()
    {
        // Assert: Expects exception

        Assert.Throws<InvalidOperationException>(() => TimeParserUtils.ParseTimeSpan("invalid"));
        Assert.Throws<InvalidOperationException>(() => TimeParserUtils.ParseTimeSpan("1d2h30m40sinvalid"));
    }
}