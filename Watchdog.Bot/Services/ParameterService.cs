using AutoMapper;
using Watchdog.Bot.Exceptions;
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
    private readonly IGuildParameterRepository _guildParameterRepository;

    public ParameterService(ILogger<ParameterService> logger, IMapper mapper, 
        IParameterRepository parameterRepository, IGuildParameterRepository guildParameterRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _parameterRepository = parameterRepository;
        _guildParameterRepository = guildParameterRepository;
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

    public async Task<ParameterResponseData<T>> GetGuildParameterValueAsync<T>(string parameterName, ulong guildId) where T : IConvertible
    {
        var guildParameter = await _guildParameterRepository.GetByNameAndGuildIdAsync(parameterName, guildId);
        if (guildParameter != null)
        {
            ThrowIfWrongType<T>(guildParameter.Parameter);
            return new ParameterResponseData<T>()
            {
                Name = guildParameter.Name,
                Value = (T)Convert.ChangeType(guildParameter.Value, typeof(T)),
                DefaultValue = (T)Convert.ChangeType(guildParameter.Parameter.Value, typeof(T))
            };
        }
        
        var parameter = await _parameterRepository.GetByIdAsync(parameterName);
        if (parameter != null)
        {
            ThrowIfWrongType<T>(parameter);
            var value = (T)Convert.ChangeType(parameter.Value, typeof(T));
            return new ParameterResponseData<T>()
            {
                Name = parameter.Name,
                Value = value,
                DefaultValue = value
            };
        }
        
        throw new ObjectNotFoundException("Parameter with the name '" + parameterName + "' does not exist.");
    }

    public async Task OverrideParameterValueAsync<T>(GuildParameterCreationData<T> parameterData) where T : IConvertible
    {
        var parameter = await _parameterRepository.GetByIdAsync(parameterData.Name);
        
        if (parameter == null)
            throw new ObjectNotFoundException("Parameter with the name '" + parameterData.Name + "' does not exist.");
        ThrowIfWrongType<T>(parameter);
        
        var guildParameter = await _guildParameterRepository.GetByNameAndGuildIdAsync(parameterData.Name, parameterData.GuildId);
        var mappedEntity = _mapper.Map<GuildParameter>(parameterData);
        
        if (guildParameter == null)
            await _guildParameterRepository.AddAsync(mappedEntity);
        else
            await _guildParameterRepository.UpdateAsync(mappedEntity);
    }

    private static void ThrowIfWrongType<T>(Parameter parameter) where T : IConvertible
    {
        if (parameter.Type != typeof(T).ToString())
            throw new InvalidCastException("Parameter type mismatch. Expected: " + typeof(T) + " but was: " + parameter.Type);
    }
}