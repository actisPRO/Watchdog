using System.Text.RegularExpressions;

namespace Watchdog.Bot.Utils;

public static class IdGenerator
{
    private static readonly DateTimeOffset Epoch = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly Random Random = new Random();

    public static string GenerateId() => RandomizeDigits(GenerateTimestamp());

    private static string GenerateTimestamp()
    {
        var now = DateTimeOffset.UtcNow;
        var timestamp = now.ToUnixTimeMilliseconds() - Epoch.ToUnixTimeMilliseconds();
        return timestamp.ToString("X");
    }
    
    private static string RandomizeDigits(string input)
    {
        var result = input.ToCharArray();
        for (int i = 0; i < result.Length; i++)
            if (char.IsDigit(result[i]))
                result[i] = (char)Random.Next('A', 'Z' + 1);

        return new string(result);
    }
}