namespace CqrsLite;

public readonly record struct Unit
{
    public static Unit Value { get; } = new();
}

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<TResult>
{
}

public interface IQuery<TResult>
{
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}

public delegate Task<TResult> MessageHandlerDelegate<TResult>();

public interface IPipelineBehavior<in TMessage, TResult>
{
    Task<TResult> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TResult> next);
}

public interface IDispatcher
{
    Task Send(ICommand command, CancellationToken cancellationToken = default);

    Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

    Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
