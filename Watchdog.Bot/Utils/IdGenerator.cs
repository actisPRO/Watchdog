using System.Text.RegularExpressions;

namespace Watchdog.Bot.Utils;

public static class IdGenerator
{
    private static readonly DateTimeOffset Epoch = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly Random Random = new Random();

    public static string GenerateId() => ReplaceDigits(GenerateTimestamp() + GenerateRandomness());

    private static string GenerateTimestamp()
    {
        var now = DateTimeOffset.UtcNow;
        var timestamp = now.ToUnixTimeMilliseconds() - Epoch.ToUnixTimeMilliseconds();
        return timestamp.ToString("X");
    }

    private static string GenerateRandomness()
    {
        var random = Random.Next(0, 0x1000);
        return random.ToString("X3");
    }
    
    private static string ReplaceDigits(string input)
    {
        var result = input.ToCharArray();
        for (int i = 0; i < result.Length; i++)
        {
            if (char.IsDigit(result[i])) 
            {
                var digit = int.Parse(result[i].ToString()); 
                if (digit >= 0 && digit <= 9) 
                {
                    var uppercaseLetter = (char)(digit + 65); 
                    result[i] = uppercaseLetter; 
                }
            }
        }

        return new string(result);
    }
}