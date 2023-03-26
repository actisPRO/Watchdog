using AutoMapper;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Mapping;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap(typeof(ParameterCreationData<>), typeof(Parameter))
            .ConvertUsing(typeof(ParameterCreationDataParameterConverter<>));
        
        CreateMap(typeof(GuildParameterCreationData<>), typeof(GuildParameter))
            .ConvertUsing(typeof(GuildParameterCreationDataGuildParameterConverter<>));

        CreateMap<ModerationLogEntryData, ModerationLogEntry>();
    }
}