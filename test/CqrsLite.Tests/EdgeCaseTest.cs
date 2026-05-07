using CqrsLite;
using Xunit;

namespace CqrsLite.Tests;

public sealed class EdgeCaseHandlerTest
{
    [Fact]
    public void AHandlerImplementingBothICommandHandlerAndICommandHandlerUnitPathsWouldCauseAmbiguity()
    {
        // This test demonstrates that a handler can implement both:
        // - ICommandHandler<TCommand> where TCommand : ICommand
        // - ICommandHandler<TCommand, Unit> where TCommand : ICommand<Unit>
        // Since ICommand : ICommand<Unit>, this creates two valid handler contracts for the same message.
        
        var handler = typeof(AmbiguousHandler);
        var interfaces = handler.GetInterfaces()
            .Where(i => i.IsGenericType)
            .Where(i => i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) || 
                       i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
            .ToArray();
        
        // This handler implements TWO different handler interfaces for the same command
        Assert.Equal(2, interfaces.Length);
    }
    
    private sealed record AmbiguousCommand : ICommand;
    
    private sealed class AmbiguousHandler : 
        ICommandHandler<AmbiguousCommand>,           // Handles Send(ICommand)
        ICommandHandler<AmbiguousCommand, Unit>      // Handles Send<Unit>(ICommand<Unit>)
    {
        public Task Handle(AmbiguousCommand command, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
        Task<Unit> ICommandHandler<AmbiguousCommand, Unit>.Handle(AmbiguousCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}
