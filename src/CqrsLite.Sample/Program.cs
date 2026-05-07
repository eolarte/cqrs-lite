using CqrsLite;
using Microsoft.Extensions.DependencyInjection;

// --- Composition root ---
// ScanHandlers(Assembly) is available as an opt-in alternative to the explicit registration below.
var services = new ServiceCollection();
services.AddCqrsLite(builder =>
{
    builder.AddCommandHandler<PlaceOrderCommand, PlaceOrderCommandHandler>();
    builder.AddQueryHandler<GetOrderStatusQuery, string, GetOrderStatusQueryHandler>();

    // Register an open-generic Pipeline Behavior that wraps every dispatch.
    builder.AddPipelineBehavior(typeof(LoggingBehavior<,>));
});

using var serviceProvider = services.BuildServiceProvider();
var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

// --- Dispatch a Command (write-side) ---
await dispatcher.Send(new PlaceOrderCommand("order-42"));
Console.WriteLine("[Sample] Command dispatch completed.");

// --- Dispatch a Query (read-side) ---
var status = await dispatcher.Query(new GetOrderStatusQuery("order-42"));
Console.WriteLine($"[Sample] Query dispatch returned: {status}");

// =============================================================================
// Message types
// =============================================================================

record PlaceOrderCommand(string OrderId) : ICommand;

record GetOrderStatusQuery(string OrderId) : IQuery<string>;

// =============================================================================
// Handlers
// =============================================================================

class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand>
{
    public Task Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Handler] Placed order '{command.OrderId}'.");
        return Task.CompletedTask;
    }
}

class GetOrderStatusQueryHandler : IQueryHandler<GetOrderStatusQuery, string>
{
    public Task<string> Handle(GetOrderStatusQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Handler] Fetching status for '{query.OrderId}'.");
        return Task.FromResult($"Order '{query.OrderId}' is confirmed.");
    }
}

// =============================================================================
// Pipeline Behavior — logs before and after each Handler invocation
// =============================================================================

class LoggingBehavior<TMessage, TResult> : IPipelineBehavior<TMessage, TResult>
{
    public async Task<TResult> Handle(
        TMessage message,
        CancellationToken cancellationToken,
        MessageHandlerDelegate<TResult> next)
    {
        Console.WriteLine($"[Pipeline] {typeof(TMessage).Name} started.");
        var result = await next().ConfigureAwait(false);
        Console.WriteLine($"[Pipeline] {typeof(TMessage).Name} completed.");
        return result;
    }
}
