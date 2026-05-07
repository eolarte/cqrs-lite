using System;
using System.Linq;

public readonly record struct Unit
{
    public static Unit Value { get; } = new();
}

public interface ICommand : ICommand<Unit> { }
public interface ICommand<TResult> { }

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
}

// Edge case: Could a handler implement BOTH ICommandHandler<TCommand> (ICommand path)
// AND ICommandHandler<TCommand, Unit> (ICommand<Unit> path)?
public class EdgeCaseCommand : ICommand
{
}

// This would be problematic:
public class EdgeCaseHandler : 
    ICommandHandler<EdgeCaseCommand>,           // ICommandHandler<TCommand> where TCommand : ICommand
    ICommandHandler<EdgeCaseCommand, Unit>      // ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
}

class Program
{
    static void Main()
    {
        var handler = typeof(EdgeCaseHandler);
        var interfaces = handler.GetInterfaces().Where(i => i.IsGenericType).ToArray();
        
        Console.WriteLine($"EdgeCaseHandler implements {interfaces.Length} handler interfaces:");
        foreach (var iface in interfaces)
        {
            Console.WriteLine($"  - {iface}");
        }
        
        // This is the real question: would the HandlerScanner see both?
        var supportedHandlerTypes = new[] 
        {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
        };
        
        var handlerInterfaces = interfaces.Where(interfaceType => 
            interfaceType.IsGenericType &&
            supportedHandlerTypes.Contains(interfaceType.GetGenericTypeDefinition()) &&
            !interfaceType.ContainsGenericParameters
        ).ToArray();
        
        Console.WriteLine($"\nHandlerScanner would detect {handlerInterfaces.Length} handler interfaces:");
        foreach (var iface in handlerInterfaces)
        {
            Console.WriteLine($"  - {iface}");
        }
        
        if (handlerInterfaces.Length > 1)
        {
            Console.WriteLine("\n❌ BUG CONFIRMED: This would violate the 'exactly one handler' constraint!");
        }
    }
}
