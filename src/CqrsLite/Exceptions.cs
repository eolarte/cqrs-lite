namespace CqrsLite;

/// <summary>
/// Base class for all CQRS Lite dispatch and registration exceptions.
/// Consumers should catch <see cref="CqrsLiteException"/> to handle all library-generated
/// errors in a single catch block without accidentally swallowing unrelated
/// <see cref="InvalidOperationException"/> instances.
/// </summary>
public abstract class CqrsLiteException : InvalidOperationException
{
    protected CqrsLiteException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Thrown when a message is dispatched for which no Handler is registered.
/// </summary>
public sealed class HandlerNotFoundException : CqrsLiteException
{
    public HandlerNotFoundException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Thrown when a message is dispatched for which more than one Handler is registered.
/// </summary>
public sealed class MultipleHandlersFoundException : CqrsLiteException
{
    public MultipleHandlersFoundException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Thrown when handler scanning detects a duplicate or structurally invalid handler registration.
/// </summary>
public sealed class InvalidHandlerRegistrationException : CqrsLiteException
{
    public InvalidHandlerRegistrationException(string message)
        : base(message)
    {
    }
}
