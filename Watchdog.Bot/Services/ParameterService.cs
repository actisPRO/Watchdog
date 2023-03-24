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
        var mappedEntity = _mapper.Map<Parameter>(parameter);
        if (existingEntity == null)
        {
            await _parameterRepository.AddAsync(mappedEntity);
            _logger.LogInformation("Created new parameter '{Name}' with value: '{Value}' [{Type}]", mappedEntity.Name, mappedEntity.Value,
                mappedEntity.Type);
        }
        else if (existingEntity.Value != mappedEntity.Value || existingEntity.Type != mappedEntity.Type)
        {
            await _parameterRepository.UpdateAsync(mappedEntity);
            _logger.LogInformation("Updated parameter '{Name}'. Value: '{Value}' [{Type}]", mappedEntity.Name, mappedEntity.Value,
                mappedEntity.Type);
        }
    }
}