using CqrsLite;

namespace CqrsLite.Benchmarks;

// =============================================================================
// Benchmark fixture message types — deterministic, in-memory, no external I/O
// =============================================================================

public sealed record BenchmarkCommand : ICommand;

public sealed record BenchmarkResultCommand : ICommand<string>;

public sealed record BenchmarkQuery : IQuery<string>;

// =============================================================================
// Benchmark fixture Handlers — no external I/O, logging sinks, or side effects
// =============================================================================

public sealed class BenchmarkCommandHandler : ICommandHandler<BenchmarkCommand>
{
    public Task Handle(BenchmarkCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}

public sealed class BenchmarkResultCommandHandler : ICommandHandler<BenchmarkResultCommand, string>
{
    public Task<string> Handle(BenchmarkResultCommand command, CancellationToken cancellationToken)
        => Task.FromResult("ok");
}

public sealed class BenchmarkQueryHandler : IQueryHandler<BenchmarkQuery, string>
{
    public Task<string> Handle(BenchmarkQuery query, CancellationToken cancellationToken)
        => Task.FromResult("ok");
}
