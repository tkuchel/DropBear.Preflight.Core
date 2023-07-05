using Microsoft.Extensions.DependencyInjection;

namespace DropBear.Preflight.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPreflightManager(this IServiceCollection services, Action<PreflightManagerConfig> configure)
    {
        // Register the PreflightManagerConfig with the specified configuration
        services.Configure(configure);

        // Register the PreflightManager as a singleton
        services.AddSingleton<PreflightManager>();

        return services;
    }
}
