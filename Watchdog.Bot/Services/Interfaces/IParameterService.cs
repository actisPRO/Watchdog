using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Services.Interfaces;

public interface IParameterService
{
    Task RegisterParameterAsync<T>(ParameterCreationData<T> parameter) where T : IConvertible;

    Task<ParameterResponseData<T>> GetGuildParameterValueAsync<T>(string parameterName, ulong guildId) where T : IConvertible;
    
    Task OverrideParameterValueAsync<T>(GuildParameterCreationData<T> parameterData) where T : IConvertible;
}