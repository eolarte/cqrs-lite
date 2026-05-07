using CqrsLite;

namespace CqrsLite.TestHandlers;

public sealed record ScannedGreetingQuery(string Name) : IQuery<string>;

public sealed class ScannedGreetingQueryHandler : IQueryHandler<ScannedGreetingQuery, string>
{
    public Task<string> Handle(ScannedGreetingQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Scanned {query.Name}");
    }
}

public sealed class UnrelatedScannedType
{
    public string Value { get; } = "ignored";
}
