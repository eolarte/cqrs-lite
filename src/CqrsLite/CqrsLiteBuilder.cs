using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsLite;

public sealed class CqrsLiteBuilder
{
    internal CqrsLiteBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceCollection Services { get; }

    public CqrsLiteBuilder AddCommandHandler<TCommand, THandler>()
        where TCommand : class, ICommand
        where THandler : class, ICommandHandler<TCommand>
    {
        Services.AddTransient<ICommandHandler<TCommand>, THandler>();
        return this;
    }

    public CqrsLiteBuilder AddCommandHandler<TCommand, TResult, THandler>()
        where TCommand : class, ICommand<TResult>
        where THandler : class, ICommandHandler<TCommand, TResult>
    {
        Services.AddTransient<ICommandHandler<TCommand, TResult>, THandler>();
        return this;
    }

    public CqrsLiteBuilder AddQueryHandler<TQuery, TResult, THandler>()
        where TQuery : class, IQuery<TResult>
        where THandler : class, IQueryHandler<TQuery, TResult>
    {
        Services.AddTransient<IQueryHandler<TQuery, TResult>, THandler>();
        return this;
    }

    public CqrsLiteBuilder AddPipelineBehavior<TBehavior>()
        where TBehavior : class
    {
        return AddPipelineBehavior(typeof(TBehavior));
    }

    public CqrsLiteBuilder AddPipelineBehavior(Type behaviorType)
    {
        ArgumentNullException.ThrowIfNull(behaviorType);

        if (behaviorType.IsGenericTypeDefinition)
        {
            if (!ClosesOpenGeneric(behaviorType, typeof(IPipelineBehavior<,>)))
            {
                throw new ArgumentException($"Pipeline Behavior type '{behaviorType.FullName}' must implement IPipelineBehavior<TMessage, TResult>.", nameof(behaviorType));
            }

            Services.AddTransient(typeof(IPipelineBehavior<,>), behaviorType);
            return this;
        }

        var serviceTypes = behaviorType
            .GetInterfaces()
            .Where(static interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
            .ToArray();

        if (serviceTypes.Length == 0)
        {
            throw new ArgumentException($"Pipeline Behavior type '{behaviorType.FullName}' must implement IPipelineBehavior<TMessage, TResult>.", nameof(behaviorType));
        }

        foreach (var serviceType in serviceTypes)
        {
            Services.AddTransient(serviceType, behaviorType);
        }

        return this;
    }

    public CqrsLiteBuilder ScanHandlers(params Assembly[] assemblies)
    {
        HandlerScanner.RegisterHandlers(Services, assemblies);
        return this;
    }

    private static bool ClosesOpenGeneric(Type implementationType, Type openGenericType)
    {
        return implementationType
            .GetInterfaces()
            .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGenericType);
    }
}
