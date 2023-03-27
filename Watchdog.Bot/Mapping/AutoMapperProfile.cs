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

        CreateMap<LogEntry, ModerationLogEntry>()
            .ForMember(x => x.GuildId, opt => opt.MapFrom(x => x.Guild.Id))
            .ForMember(x => x.Guild, opt => opt.Ignore())
            .ForMember(x => x.ExecutorId, opt => opt.MapFrom(x => x.Executor.Id))
            .ForMember(x => x.TargetId, opt => opt.MapFrom(x => x.Target.Id));
    }
}