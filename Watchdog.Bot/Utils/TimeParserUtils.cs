using System.Text.RegularExpressions;

namespace Watchdog.Bot.Utils;

public static class TimeParserUtils
{
    private static readonly Regex MyRegex = new(
        "^((?<days>\\d+)d)?((?<hours>\\d+)h)?((?<minutes>\\d+)m)?((?<seconds>\\d+)s)?$",
        RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.RightToLeft |
        RegexOptions.CultureInvariant);

    public static DateTime GetFutureDate(string input)
    {
        return DateTime.UtcNow.Add(ParseTimeSpan(input));
    }

    //Парсит формат даты на подобии 1d2h30m, 1d, 30m10s
    //Добавлены дни, взято отсюда: 
    //https://stackoverflow.com/questions/47702094/parse-the-string-26h44m3s-to-timespan-in-c-sharp
    public static TimeSpan ParseTimeSpan(string input)
    {
        var m = MyRegex.Match(input.Trim());

        var ds = m.Groups["days"].Success ? int.Parse(m.Groups["days"].Value) : 0;
        var hs = m.Groups["hours"].Success ? int.Parse(m.Groups["hours"].Value) : 0;
        var ms = m.Groups["minutes"].Success ? int.Parse(m.Groups["minutes"].Value) : 0;
        var ss = m.Groups["seconds"].Success ? int.Parse(m.Groups["seconds"].Value) : 0;

        var result = TimeSpan.FromSeconds(ds * 24 * 60 * 60 + hs * 60 * 60 + ms * 60 + ss);
        if (result.TotalSeconds < 1) throw new InvalidOperationException("Unable to convert time!");
        return result;
    }
}