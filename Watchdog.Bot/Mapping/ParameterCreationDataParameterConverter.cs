using System.Globalization;
using AutoMapper;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Mapping;

public class ParameterCreationDataParameterConverter<T> : ITypeConverter<ParameterCreationData<T>, Parameter> 
    where T : IConvertible
{
    public Parameter Convert(ParameterCreationData<T> source, Parameter destination, ResolutionContext context)
    {
        return new()
        {
            Name = source.Name,
            Type = typeof(T).Name,
            Value = System.Convert.ToString(source.Value, CultureInfo.InvariantCulture) ?? string.Empty
        };
    }
}