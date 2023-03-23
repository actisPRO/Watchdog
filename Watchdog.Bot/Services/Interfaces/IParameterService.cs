
using Watchdog.Bot.Models.DataTransfer;

namespace Watchdog.Bot.Services.Interfaces;

public interface IParameterService
{
    Task RegisterParameterAsync<T>(ParameterCreationData<T> parameter) where T : IConvertible;
}