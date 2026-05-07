using CqrsLite;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsLite.Tests;

/// <summary>
/// Verifies that the explicit DI registration pattern demonstrated by the sample application
/// correctly wires Handlers and Pipeline Behaviors through the real CQRS Lite runtime.
/// Types are defined inline to keep this test project self-contained.
/// </summary>
public sealed class SampleIntegrationTests
{
    // ----------------------------------------------------------------
    // Behavior 1: Explicit Command Handler registration dispatches a void Command
    // ----------------------------------------------------------------

    [Fact]
    public async Task SampleExplicitCommandDispatchCompletesWhenHandlerIsRegistered()
    {
        var invocations = new List<string>();
        var services = new ServiceCollection();
        services.AddSingleton(invocations);
        services.AddCqrsLite(builder =>
        {
            builder.AddCommandHandler<SampleCreateOrderCommand, SampleCreateOrderCommandHandler>();
        });
        using var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<IDispatcher>();

        await dispatcher.Send(new SampleCreateOrderCommand("order-1"), CancellationToken.None);

        Assert.Contains("order-1", invocations);
    }

    // ----------------------------------------------------------------
    // Behavior 2: Explicit Query Handler registration returns a Query result
    // ----------------------------------------------------------------

    [Fact]
    public async Task SampleExplicitQueryDispatchReturnsResultWhenHandlerIsRegistered()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder =>
        {
            builder.AddQueryHandler<SampleGetOrderStatusQuery, string, SampleGetOrderStatusQueryHandler>();
        });
        using var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<IDispatcher>();

        var result = await dispatcher.Query(new SampleGetOrderStatusQuery("order-1"), CancellationToken.None);

        Assert.Equal("Order 'order-1' is confirmed.", result);
    }

    // ----------------------------------------------------------------
    // Behavior 3: Registered Pipeline Behavior executes observably in the dispatch chain
    // ----------------------------------------------------------------

    [Fact]
    public async Task SampleRegisteredPipelineBehaviorExecutesObservablyInTheDispatchChain()
    {
        var log = new List<string>();
        var services = new ServiceCollection();
        services.AddSingleton(log);
        services.AddCqrsLite(builder =>
        {
            builder.AddCommandHandler<SampleCreateOrderCommand, SampleCreateOrderCommandHandler>();
            builder.AddQueryHandler<SampleGetOrderStatusQuery, string, SampleGetOrderStatusQueryHandler>();

            // Open-generic Pipeline Behavior — identical pattern used in the sample entry point.
            builder.AddPipelineBehavior(typeof(SampleObservableBehavior<,>));
        });
        using var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<IDispatcher>();

        await dispatcher.Send(new SampleCreateOrderCommand("order-2"), CancellationToken.None);
        await dispatcher.Query(new SampleGetOrderStatusQuery("order-2"), CancellationToken.None);

        Assert.Contains("SampleCreateOrderCommand:before", log);
        Assert.Contains("SampleCreateOrderCommand:after", log);
        Assert.Contains("SampleGetOrderStatusQuery:before", log);
        Assert.Contains("SampleGetOrderStatusQuery:after", log);
    }

    // ----------------------------------------------------------------
    // Inline fixture types — mirror the sample's message and Handler shapes
    // ----------------------------------------------------------------

    private sealed record SampleCreateOrderCommand(string OrderId) : ICommand;

    private sealed class SampleCreateOrderCommandHandler(List<string> invocations)
        : ICommandHandler<SampleCreateOrderCommand>
    {
        public Task Handle(SampleCreateOrderCommand command, CancellationToken cancellationToken)
        {
            invocations.Add(command.OrderId);
            return Task.CompletedTask;
        }
    }

    private sealed record SampleGetOrderStatusQuery(string OrderId) : IQuery<string>;

    private sealed class SampleGetOrderStatusQueryHandler : IQueryHandler<SampleGetOrderStatusQuery, string>
    {
        public Task<string> Handle(SampleGetOrderStatusQuery query, CancellationToken cancellationToken)
            => Task.FromResult($"Order '{query.OrderId}' is confirmed.");
    }

    private sealed class SampleObservableBehavior<TMessage, TResult>(List<string> log)
        : IPipelineBehavior<TMessage, TResult>
    {
        public async Task<TResult> Handle(
            TMessage message,
            CancellationToken cancellationToken,
            MessageHandlerDelegate<TResult> next)
        {
            log.Add($"{typeof(TMessage).Name}:before");
            var result = await next().ConfigureAwait(false);
            log.Add($"{typeof(TMessage).Name}:after");
            return result;
        }
    }
}
