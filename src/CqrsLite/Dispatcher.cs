using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsLite;

public sealed class Dispatcher : IDispatcher
{
    // These static dictionaries cache compiled expression-tree delegates keyed by message type.
    // They are pure type-to-invocation mappings — no DI scope references, handler instances, or
    // caller data are captured inside the lambdas. Every cached delegate receives the Dispatcher
    // instance (and therefore its IServiceProvider) as an explicit parameter at call time, so
    // sharing across multiple Dispatcher instances (e.g., different DI scopes) is safe and
    // intentional. The cache grows monotonically with distinct message types used across the
    // process lifetime, which is expected and low-impact for typical application workloads.
    private static readonly ConcurrentDictionary<Type, Func<Dispatcher, ICommand, CancellationToken, Task>> CommandDispatchers = new();
    private static readonly ConcurrentDictionary<(Type MessageType, Type ResultType), Func<Dispatcher, object, CancellationToken, Task<object?>>> CommandResultDispatchers = new();
    private static readonly ConcurrentDictionary<(Type MessageType, Type ResultType), Func<Dispatcher, object, CancellationToken, Task<object?>>> QueryDispatchers = new();

    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public Task Send(ICommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var dispatcher = CommandDispatchers.GetOrAdd(command.GetType(), static commandType => CreateCommandDispatcher(commandType));
        return dispatcher(this, command, cancellationToken);
    }

    public async Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var dispatcher = CommandResultDispatchers.GetOrAdd((command.GetType(), typeof(TResult)), static key => CreateCommandResultDispatcher(key.MessageType, key.ResultType));
        return (TResult)(await dispatcher(this, command, cancellationToken).ConfigureAwait(false))!;
    }

    public async Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var dispatcher = QueryDispatchers.GetOrAdd((query.GetType(), typeof(TResult)), static key => CreateQueryDispatcher(key.MessageType, key.ResultType));
        return (TResult)(await dispatcher(this, query, cancellationToken).ConfigureAwait(false))!;
    }

    private static Func<Dispatcher, ICommand, CancellationToken, Task> CreateCommandDispatcher(Type commandType)
    {
        var dispatcherParameter = Expression.Parameter(typeof(Dispatcher), "dispatcher");
        var commandParameter = Expression.Parameter(typeof(ICommand), "command");
        var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

        var body = Expression.Call(
            dispatcherParameter,
            nameof(SendCommandCoreAsync),
            [commandType],
            Expression.Convert(commandParameter, commandType),
            cancellationTokenParameter);

        return Expression.Lambda<Func<Dispatcher, ICommand, CancellationToken, Task>>(body, dispatcherParameter, commandParameter, cancellationTokenParameter).Compile();
    }

    private static Func<Dispatcher, object, CancellationToken, Task<object?>> CreateCommandResultDispatcher(Type commandType, Type resultType)
    {
        var dispatcherParameter = Expression.Parameter(typeof(Dispatcher), "dispatcher");
        var commandParameter = Expression.Parameter(typeof(object), "command");
        var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

        var body = Expression.Call(
            typeof(Dispatcher),
            nameof(BoxResultAsync),
            [resultType],
            Expression.Call(
                dispatcherParameter,
                nameof(SendCommandCoreAsync),
                [commandType, resultType],
                Expression.Convert(commandParameter, commandType),
                cancellationTokenParameter));

        return Expression.Lambda<Func<Dispatcher, object, CancellationToken, Task<object?>>>(body, dispatcherParameter, commandParameter, cancellationTokenParameter).Compile();
    }

    private static Func<Dispatcher, object, CancellationToken, Task<object?>> CreateQueryDispatcher(Type queryType, Type resultType)
    {
        var dispatcherParameter = Expression.Parameter(typeof(Dispatcher), "dispatcher");
        var queryParameter = Expression.Parameter(typeof(object), "query");
        var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

        var body = Expression.Call(
            typeof(Dispatcher),
            nameof(BoxResultAsync),
            [resultType],
            Expression.Call(
                dispatcherParameter,
                nameof(QueryCoreAsync),
                [queryType, resultType],
                Expression.Convert(queryParameter, queryType),
                cancellationTokenParameter));

        return Expression.Lambda<Func<Dispatcher, object, CancellationToken, Task<object?>>>(body, dispatcherParameter, queryParameter, cancellationTokenParameter).Compile();
    }

    private static async Task<object?> BoxResultAsync<TResult>(Task<TResult> task)
    {
        return await task.ConfigureAwait(false);
    }

    private async Task SendCommandCoreAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        _ = await ExecutePipelineAsync<TCommand, Unit>(
            command,
            cancellationToken,
            () => InvokeCommandHandlerAsync(command, cancellationToken)).ConfigureAwait(false);
    }

    private Task<TResult> SendCommandCoreAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>
    {
        return ExecutePipelineAsync<TCommand, TResult>(
            command,
            cancellationToken,
            () => ResolveSingleHandler<ICommandHandler<TCommand, TResult>>(typeof(TCommand)).Handle(command, cancellationToken));
    }

    private Task<TResult> QueryCoreAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
        where TQuery : IQuery<TResult>
    {
        return ExecutePipelineAsync<TQuery, TResult>(
            query,
            cancellationToken,
            () => ResolveSingleHandler<IQueryHandler<TQuery, TResult>>(typeof(TQuery)).Handle(query, cancellationToken));
    }

    private async Task<Unit> InvokeCommandHandlerAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        await ResolveSingleHandler<ICommandHandler<TCommand>>(typeof(TCommand)).Handle(command, cancellationToken).ConfigureAwait(false);
        return Unit.Value;
    }

    private Task<TResult> ExecutePipelineAsync<TMessage, TResult>(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TResult> handler)
    {
        var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TMessage, TResult>>().ToArray();

        MessageHandlerDelegate<TResult> next = handler;

        for (var index = behaviors.Length - 1; index >= 0; index--)
        {
            var behavior = behaviors[index];
            var currentNext = next;
            next = () => behavior.Handle(message, cancellationToken, currentNext);
        }

        return next();
    }

    private THandler ResolveSingleHandler<THandler>(Type messageType)
    {
        var handlers = _serviceProvider.GetServices<THandler>().ToArray();

        return handlers.Length switch
        {
            0 => throw new HandlerNotFoundException($"No Handler is registered for message type '{messageType.FullName}'."),
            > 1 => throw new MultipleHandlersFoundException($"More than one Handler is registered for message type '{messageType.FullName}'."),
            _ => handlers[0]
        };
    }
}
