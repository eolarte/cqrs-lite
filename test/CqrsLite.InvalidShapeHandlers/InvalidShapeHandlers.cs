using CqrsLite;

namespace CqrsLite.InvalidShapeHandlers;

public sealed record InvalidGreetingCommand(string Name) : ICommand;

public sealed record InvalidGreetingQuery(string Name) : IQuery<string>;

public sealed class InvalidGreetingHandler :
    ICommandHandler<InvalidGreetingCommand>,
    IQueryHandler<InvalidGreetingQuery, string>
{
    public Task Handle(InvalidGreetingCommand command, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<string> Handle(InvalidGreetingQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(query.Name);
    }
}
