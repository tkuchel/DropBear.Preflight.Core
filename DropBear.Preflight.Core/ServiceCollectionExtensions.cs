using Microsoft.Extensions.DependencyInjection;

namespace DropBear.Preflight.Core;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the PreflightManager to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the PreflightManager to.</param>
    /// <param name="configure">A delegate to configure the PreflightManagerConfig.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddPreflightManager(this IServiceCollection services, Action<PreflightManagerConfig> configure)
    {
        // Register the PreflightManagerConfig with the specified configuration
        services.Configure(configure);

        // Register the PreflightManager as a singleton
        services.AddSingleton<PreflightManager>();

        return services;
    }
}
