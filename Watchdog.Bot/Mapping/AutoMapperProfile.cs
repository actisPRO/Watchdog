using AutoMapper;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap(typeof(ParameterCreationData<>), typeof(Parameter))
            .ConvertUsing(typeof(ParameterCreationDataParameterConverter<>));
    }
}