using AutoMapper;
using Watchdog.Bot.Models.Database;
using Watchdog.Bot.Models.DataTransfer;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Services.Interfaces;

namespace Watchdog.Bot.Services;

public sealed class ParameterService : IParameterService
{
    private readonly ILogger<ParameterService> _logger;
    private readonly IMapper _mapper;
    private readonly IParameterRepository _parameterRepository;
    
    public ParameterService(ILogger<ParameterService> logger, IMapper mapper, IParameterRepository parameterRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _parameterRepository = parameterRepository;
    }

    public async Task RegisterParameterAsync<T>(ParameterCreationData<T> parameter) where T : IConvertible
    {
        var existingEntity = await _parameterRepository.GetByIdAsync(parameter.Name);
        if (existingEntity == null)
        {
            await _parameterRepository.AddAsync(_mapper.Map<Parameter>(parameter));
        }
        else
        {
            existingEntity = _mapper.Map<Parameter>(parameter);
            await _parameterRepository.UpdateAsync(existingEntity);
        }
    }
}