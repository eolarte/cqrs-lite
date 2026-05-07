using CqrsLite;

namespace CqrsLite.InvalidHandlers;

public sealed record DuplicateGreetingQuery(string Name) : IQuery<string>;

public sealed class DuplicateGreetingQueryHandlerOne : IQueryHandler<DuplicateGreetingQuery, string>
{
    public Task<string> Handle(DuplicateGreetingQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(query.Name);
    }
}

public sealed class DuplicateGreetingQueryHandlerTwo : IQueryHandler<DuplicateGreetingQuery, string>
{
    public Task<string> Handle(DuplicateGreetingQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(query.Name);
    }
}
