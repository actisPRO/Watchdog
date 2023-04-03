using System.Reflection;
using Watchdog.Bot.Attributes;

namespace Watchdog.Bot.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Register all services that have the ServiceAttribute
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.GetCustomAttribute<ServiceAttribute>() != null)
            .Aggregate(services, (current, type) => current.AddTransient(type));
    }
}