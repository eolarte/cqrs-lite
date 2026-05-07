using CqrsLite;

namespace CqrsLite.AmbiguousHandlers;

public sealed record AmbiguousCommand : ICommand;

/// <summary>
/// A handler that illegally implements both the void-result form (ICommandHandler&lt;T&gt;)
/// and the explicit Unit-result form (ICommandHandler&lt;T, Unit&gt;) of the same command.
/// Because ICommand : ICommand&lt;Unit&gt;, the two interfaces refer to the same message
/// contract via two different generic shapes, which is invalid by the scanner rules.
/// </summary>
public sealed class AmbiguousCommandHandler :
    ICommandHandler<AmbiguousCommand>,
    ICommandHandler<AmbiguousCommand, Unit>
{
    public Task Handle(AmbiguousCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;

    Task<Unit> ICommandHandler<AmbiguousCommand, Unit>.Handle(AmbiguousCommand command, CancellationToken cancellationToken)
        => Task.FromResult(Unit.Value);
}
