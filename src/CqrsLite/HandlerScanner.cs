using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsLite;

internal static class HandlerScanner
{
    private static readonly Type[] SupportedHandlerTypes =
    [
        typeof(ICommandHandler<>),
        typeof(ICommandHandler<,>),
        typeof(IQueryHandler<,>)
    ];

    public static void RegisterHandlers(IServiceCollection services, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        if (assemblies.Length == 0)
        {
            throw new ArgumentException("At least one assembly must be provided for Handler scanning.", nameof(assemblies));
        }

        if (assemblies.Any(static assembly => assembly is null))
        {
            throw new ArgumentException("Handler scanning assemblies cannot contain null entries.", nameof(assemblies));
        }

        var pendingRegistrations = new List<(Type ServiceType, Type ImplementationType)>();
        var existingServiceTypes = services.Select(static descriptor => descriptor.ServiceType).ToHashSet();
        var pendingServiceTypes = new HashSet<Type>();

        foreach (var assembly in assemblies.Distinct())
        {
            // assembly.DefinedTypes includes all accessibility levels (public, internal, private nested).
            // Including non-public (internal) concrete types is intentional: the trust boundary is the
            // assembly, which is supplied explicitly by the caller. Internal handler implementations are
            // a valid and common pattern in well-structured libraries and application layers.
            foreach (var candidateType in assembly.DefinedTypes.Where(static type => type is { IsClass: true, IsAbstract: false, ContainsGenericParameters: false }))
            {
                var handlerInterfaces = candidateType.ImplementedInterfaces.Where(IsSupportedClosedHandlerType).ToArray();

                if (handlerInterfaces.Length == 0)
                {
                    continue;
                }

                if (handlerInterfaces.Length > 1)
                {
                    throw new InvalidHandlerRegistrationException($"Type '{candidateType.FullName}' has an invalid Handler shape. Scanned Handler implementations must map to exactly one supported message contract.");
                }

                var serviceType = handlerInterfaces[0];
                if (existingServiceTypes.Contains(serviceType) || !pendingServiceTypes.Add(serviceType))
                {
                    throw new InvalidHandlerRegistrationException($"Duplicate Handler registration detected for message contract '{serviceType.FullName}'.");
                }

                pendingRegistrations.Add((serviceType, candidateType.AsType()));
            }
        }

        foreach (var registration in pendingRegistrations)
        {
            services.AddTransient(registration.ServiceType, registration.ImplementationType);
        }
    }

    private static bool IsSupportedClosedHandlerType(Type interfaceType)
    {
        return interfaceType.IsGenericType &&
               SupportedHandlerTypes.Contains(interfaceType.GetGenericTypeDefinition()) &&
               !interfaceType.ContainsGenericParameters;
    }
}
