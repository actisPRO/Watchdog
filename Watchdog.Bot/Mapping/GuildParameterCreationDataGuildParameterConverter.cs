using System.Globalization;
using AutoMapper;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Mapping;

public sealed class GuildParameterCreationDataGuildParameterConverter<T> : ITypeConverter<GuildParameterCreationData<T>, GuildParameter> 
    where T : IConvertible

{
    public GuildParameter Convert(GuildParameterCreationData<T> source, GuildParameter destination, ResolutionContext context)
    {
        return new()
        {
            Name = source.Name,
            GuildId = source.GuildId,
            Value = System.Convert.ToString(source.Value, CultureInfo.InvariantCulture) ?? string.Empty
        };
    }
}