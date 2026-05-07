using System;
using System.Linq;

// Simulate the interface hierarchy
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

// Test handler implementing ICommandHandler<TCommand> where TCommand : ICommand
public class TestCommandHandler : ICommandHandler<TestCommand>
{
}

public class TestCommand : ICommand
{
}

class Program
{
    static void Main()
    {
        var handler = typeof(TestCommandHandler);
        var interfaces = handler.GetInterfaces();
        
        Console.WriteLine($"TestCommandHandler implements {interfaces.Length} interfaces:");
        foreach (var iface in interfaces)
        {
            Console.WriteLine($"  - {iface.Name}");
            if (iface.IsGenericType)
            {
                Console.WriteLine($"    Generic definition: {iface.GetGenericTypeDefinition().Name}");
                Console.WriteLine($"    Type arguments: {string.Join(", ", iface.GetGenericArguments().Select(t => t.Name))}");
            }
        }
        
        // Check what TestCommand implements
        Console.WriteLine($"\nTestCommand implements {typeof(TestCommand).GetInterfaces().Length} interfaces:");
        foreach (var iface in typeof(TestCommand).GetInterfaces())
        {
            Console.WriteLine($"  - {iface.Name}");
            if (iface.IsGenericType)
            {
                Console.WriteLine($"    Generic definition: {iface.GetGenericTypeDefinition().Name}");
                Console.WriteLine($"    Type arguments: {string.Join(", ", iface.GetGenericArguments().Select(t => t.Name))}");
            }
        }
    }
}
