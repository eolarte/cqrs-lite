using Microsoft.Extensions.DependencyInjection;

namespace CqrsLite;

public static class CqrsLiteServiceCollectionExtensions
{
    public static IServiceCollection AddCqrsLite(this IServiceCollection services, Action<CqrsLiteBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (!services.Any(descriptor => descriptor.ServiceType == typeof(IDispatcher)))
        {
            services.AddTransient<IDispatcher, Dispatcher>();
        }

        configure?.Invoke(new CqrsLiteBuilder(services));
        return services;
    }
}
