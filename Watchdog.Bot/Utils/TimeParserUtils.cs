using System.Text.RegularExpressions;

namespace Watchdog.Bot.Utils;

public partial class TimeParserUtils
{
    public enum TimeUnit
    {
        Seconds,
        Minutes,
        Hours,
        Days
    }

    [GeneratedRegex("^((?<days>\\d+)d)?((?<hours>\\d+)h)?((?<minutes>\\d+)m)?((?<seconds>\\d+)s)?$",
        RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.RightToLeft |
        RegexOptions.CultureInvariant)]
    private static partial Regex MyRegex();

    public static DateTime GetFutureDate(string input)
    {
        return DateTime.UtcNow.Add(ParseTimeSpan(input));
    }

    //Парсит формат даты на подобии 1d2h30m, 1d, 30m10s
    //Добавлены дни, взято отсюда: 
    //https://stackoverflow.com/questions/47702094/parse-the-string-26h44m3s-to-timespan-in-c-sharp
    public static TimeSpan ParseTimeSpan(string input)
    {
        var m = MyRegex().Match(input.Trim());

        var ds = m.Groups["days"].Success ? int.Parse(m.Groups["days"].Value) : 0;
        var hs = m.Groups["hours"].Success ? int.Parse(m.Groups["hours"].Value) : 0;
        var ms = m.Groups["minutes"].Success ? int.Parse(m.Groups["minutes"].Value) : 0;
        var ss = m.Groups["seconds"].Success ? int.Parse(m.Groups["seconds"].Value) : 0;

        var result = TimeSpan.FromSeconds(ds * 24 * 60 * 60 + hs * 60 * 60 + ms * 60 + ss);
        if (result.TotalSeconds < 1) throw new InvalidOperationException("Unable to convert time!");
        return result;
    }

    public static string FormatTimespan(TimeSpan time)
    {
        var ds = time.Days != 0 ? ToCorrectCase(time, TimeUnit.Days) + " " : "";
        var hs = time.Hours != 0 ? ToCorrectCase(time, TimeUnit.Hours) + " " : "";
        var ms = time.Minutes != 0 ? ToCorrectCase(time, TimeUnit.Minutes) + " " : "";
        var ss = time.Seconds != 0 ? ToCorrectCase(time, TimeUnit.Seconds) : "";
        return time == TimeSpan.Zero ? "0 секунд" : (ds + hs + ms + ss).TrimEnd(' ');
        //return string.Format("{0:%d}д {0:%h}ч {0:%m}м {0:%s}с", time);
    }

    public static string ToCorrectCase(TimeSpan time, TimeUnit unit)
    {
        /*
         * Склонение:
         * Заканчивается на 1, но не на 11 (1, 21, 101, 1121) - номинатив ед. ч.
         * Заканчивается на 2, 3, 4, но не на 12, 13, 14 (2, 23, 954) - генетив ед. ч.
         * Заканчивается на 6, 7, 8, 9, 0, 11, 12, 13, 14 - генетив мн. ч.
         */

        var cts = unit switch
        {
            TimeUnit.Days => $"{time:%d}",
            TimeUnit.Hours => $"{time:%h}",
            TimeUnit.Minutes => $"{time:%m}",
            TimeUnit.Seconds => $"{time:%s}",
            _ => time.ToString()
        };

        if (cts.EndsWith("1") && !cts.EndsWith("11"))
            switch (unit)
            {
                case TimeUnit.Seconds:
                    return $"{cts} секунда";
                case TimeUnit.Minutes:
                    return $"{cts} минута";
                case TimeUnit.Hours:
                    return $"{cts} час";
                case TimeUnit.Days:
                    return $"{cts} день";
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        if ((cts.EndsWith("2") || cts.EndsWith("3") || cts.EndsWith("4")) &&
                 !(cts.EndsWith("12") || cts.EndsWith("13") || cts.EndsWith("14")))
            switch (unit)
            {
                case TimeUnit.Seconds:
                    return $"{cts} секунды";
                case TimeUnit.Minutes:
                    return $"{cts} минуты";
                case TimeUnit.Hours:
                    return $"{cts} часа";
                case TimeUnit.Days:
                    return $"{cts} дня";
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }

        return unit switch
        {
            TimeUnit.Seconds => $"{cts} секунд",
            TimeUnit.Minutes => $"{cts} минут",
            TimeUnit.Hours => $"{cts} часов",
            TimeUnit.Days => $"{cts} дней",
            _ => throw new InvalidOperationException("Unable to convert time!")
        };
    }
}