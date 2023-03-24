using AutoMapper;
using Microsoft.Extensions.Logging;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Mapping;

namespace Watchdog.Bot.Tests;

public abstract class BaseTest
{
    private IMapper _mapper = default!;
    protected IMapper Mapper => _mapper.ThrowIfNull();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()));
    }
    
    protected ILogger<T> CreateLogger<T>()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<T>();
    }
}