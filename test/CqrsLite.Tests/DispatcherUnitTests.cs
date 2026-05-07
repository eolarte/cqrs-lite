using System.Reflection;
using CqrsLite;

namespace CqrsLite.Tests;

public sealed class DispatcherUnitTests
{
    [Fact]
    public async Task ACallerCanDispatchACommandOrQueryWithMultipleRegisteredHandlersResultingInAnExplicitRuntimeError()
    {
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<IQueryHandler<DuplicateQuery, string>>(new DuplicateQueryHandlerOne(), new DuplicateQueryHandlerTwo()));

        var exception = await Assert.ThrowsAsync<MultipleHandlersFoundException>(() => dispatcher.Query(new DuplicateQuery("Ada")));

        Assert.Contains(nameof(DuplicateQuery), exception.Message);
    }

    [Fact]
    public async Task ACompositionRootCanRegisterPipelineBehaviorsResultingInNestedExecutionInRegistrationOrderAroundOneHandler()
    {
        var events = new List<string>();
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<IQueryHandler<GetPipelineQuery, string>>(new GetPipelineQueryHandler(events))
                .AddEnumerable<IPipelineBehavior<GetPipelineQuery, string>>(new OuterBehavior(events), new InnerBehavior(events)));

        var result = await dispatcher.Query(new GetPipelineQuery("Ada"));

        Assert.Equal("Handled Ada", result);
        Assert.Equal(["outer-before", "inner-before", "handler", "inner-after", "outer-after"], events);
    }

    [Fact]
    public async Task ACompositionRootCanDispatchACommandThroughPipelineBehaviorsResultingInUnitWrappingRemainingInternal()
    {
        var events = new List<string>();
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<ICommandHandler<PipelineCommand>>(new PipelineCommandHandler(events))
                .AddEnumerable<IPipelineBehavior<PipelineCommand, Unit>>(new OuterCommandBehavior(events), new InnerCommandBehavior(events)));

        await dispatcher.Send(new PipelineCommand("Ada"));

        Assert.Equal(["outer-before", "inner-before", "handler", "inner-after", "outer-after"], events);
    }

    [Fact]
    public async Task ACompositionRootCanDispatchAResultCommandThroughPipelineBehaviorsResultingInACommandResultBeingReturned()
    {
        var events = new List<string>();
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<ICommandHandler<ResultCommand, int>>(new ResultCommandHandler(events))
                .AddEnumerable<IPipelineBehavior<ResultCommand, int>>(new OuterResultCommandBehavior(events), new InnerResultCommandBehavior(events)));

        var result = await dispatcher.Send(new ResultCommand("Ada"));

        Assert.Equal(3, result);
        Assert.Equal(["outer-before", "inner-before", "handler", "inner-after", "outer-after"], events);
    }

    [Fact]
    public async Task APipelineBehaviorCanShortCircuitIntentionallyResultingInAnObservableCallerOutcomeWithoutInvokingTheNextDelegate()
    {
        var handlerInvocations = new HandlerInvocationCounter();
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<IQueryHandler<ShortCircuitQuery, string>>(new ShortCircuitQueryHandler(handlerInvocations))
                .AddEnumerable<IPipelineBehavior<ShortCircuitQuery, string>>(new ShortCircuitBehavior()));

        var result = await dispatcher.Query(new ShortCircuitQuery("Ada"));

        Assert.Equal("Short-circuited Ada", result);
        Assert.Equal(0, handlerInvocations.Count);
    }

    [Fact]
    public async Task AHandlerExceptionPropagatesThroughRegisteredPipelineBehaviorsUnchanged()
    {
        var events = new List<string>();
        var expectedException = new InvalidOperationException("boom");
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<ICommandHandler<ThrowingCommand>>(new ThrowingCommandHandler(expectedException, events))
                .AddEnumerable<IPipelineBehavior<ThrowingCommand, Unit>>(new ExceptionObservingBehavior(events)));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => dispatcher.Send(new ThrowingCommand("Ada")));

        Assert.Same(expectedException, exception);
        Assert.Equal(["behavior-before", "handler", "behavior-finally"], events);
    }

    [Fact]
    public async Task ACallerCanCancelDispatchResultingInTheSameCancellationTokenReachingPipelineBehaviorsAndTheHandler()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var probe = new CancellationProbe();
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<IQueryHandler<CancellableQuery, string>>(new CancellableQueryHandler(probe))
                .AddEnumerable<IPipelineBehavior<CancellableQuery, string>>(new CancellationObservingBehavior(probe)));

        var token = cancellationTokenSource.Token;
        var result = await dispatcher.Query(new CancellableQuery("Ada"), token);

        Assert.Equal("Cancellable Ada", result);
        Assert.Equal(token, probe.BehaviorToken);
        Assert.Equal(token, probe.HandlerToken);
    }

    // -------------------------------------------------------------------
    // Finding 1: Send(ICommand) duplicate handler path
    // -------------------------------------------------------------------

    [Fact]
    public async Task ACallerCanDispatchACommandWithDuplicateHandlersResultingInAnExplicitRuntimeError()
    {
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<ICommandHandler<DuplicateCommand>>(new DuplicateCommandHandlerOne(), new DuplicateCommandHandlerTwo()));

        var exception = await Assert.ThrowsAsync<MultipleHandlersFoundException>(() => dispatcher.Send(new DuplicateCommand("Ada")));

        Assert.Contains(nameof(DuplicateCommand), exception.Message);
    }

    // -------------------------------------------------------------------
    // Finding 2: Send<TResult>(ICommand<TResult>) duplicate handler path
    // -------------------------------------------------------------------

    [Fact]
    public async Task ACallerCanDispatchAResultCommandWithDuplicateHandlersResultingInAnExplicitRuntimeError()
    {
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<ICommandHandler<DuplicateResultCommand, int>>(new DuplicateResultCommandHandlerOne(), new DuplicateResultCommandHandlerTwo()));

        var exception = await Assert.ThrowsAsync<MultipleHandlersFoundException>(() => dispatcher.Send<int>(new DuplicateResultCommand("Ada")));

        Assert.Contains(nameof(DuplicateResultCommand), exception.Message);
    }

    // -------------------------------------------------------------------
    // Finding 3: CancellationToken propagation for Send(ICommand)
    // -------------------------------------------------------------------

    [Fact]
    public async Task ACallerCanCancelCommandDispatchResultingInTheSameCancellationTokenReachingPipelineBehaviorsAndTheHandler()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var probe = new CancellationProbe();
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<ICommandHandler<CancellableCommand>>(new CancellableCommandHandler(probe))
                .AddEnumerable<IPipelineBehavior<CancellableCommand, Unit>>(new CancellationObservingCommandBehavior(probe)));

        var token = cancellationTokenSource.Token;
        await dispatcher.Send(new CancellableCommand("Ada"), token);

        Assert.Equal(token, probe.BehaviorToken);
        Assert.Equal(token, probe.HandlerToken);
    }

    // -------------------------------------------------------------------
    // Finding 4: Pre-cancelled CancellationToken at dispatch time
    // -------------------------------------------------------------------

    [Fact]
    public async Task ACallerPassingAPreCancelledTokenResultsInTheCancelledTokenReachingTheBehaviorAndHandler()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel(); // cancel before dispatch
        var probe = new CancellationProbe();
        var dispatcher = new Dispatcher(
            new TestServiceProvider()
                .AddEnumerable<IQueryHandler<CancellableQuery, string>>(new CancellableQueryHandler(probe))
                .AddEnumerable<IPipelineBehavior<CancellableQuery, string>>(new CancellationObservingBehavior(probe)));

        var token = cts.Token;

        // The dispatcher does not short-circuit on a pre-cancelled token;
        // it propagates the token to behaviors and handlers as specified.
        // This documents the by-design behavior: early-cancellation short-circuiting
        // is caller or handler responsibility, not enforced by the framework.
        var result = await dispatcher.Query(new CancellableQuery("Ada"), token);

        Assert.Equal("Cancellable Ada", result);
        // Use token identity (not just IsCancellationRequested) to confirm the exact
        // caller-supplied token was propagated, not any other cancelled token.
        Assert.Equal(token, probe.BehaviorToken);
        Assert.Equal(token, probe.HandlerToken);
    }

    // -------------------------------------------------------------------
    // Finding 5: Null dispatch guards — ArgumentNullException on null message
    // -------------------------------------------------------------------

    [Fact]
    public async Task ACallerDispatchingANullMessageResultsInArgumentNullException()
    {
        var dispatcher = new Dispatcher(new TestServiceProvider());

        // Send(ICommand) is a non-async method that throws ArgumentNullException synchronously
        // before returning a Task. Assert.ThrowsAsync captures both synchronous throws from
        // Task-returning methods and faulted tasks, so it is correct for all three dispatch paths.
        await Assert.ThrowsAsync<ArgumentNullException>(() => dispatcher.Send((ICommand)null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => dispatcher.Send<string>((ICommand<string>)null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => dispatcher.Query<string>((IQuery<string>)null!));
    }

    // -------------------------------------------------------------------
    // Finding 1/2 supporting types
    // -------------------------------------------------------------------

    private sealed record DuplicateCommand(string Name) : ICommand;

    private sealed class DuplicateCommandHandlerOne : ICommandHandler<DuplicateCommand>
    {
        public Task Handle(DuplicateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class DuplicateCommandHandlerTwo : ICommandHandler<DuplicateCommand>
    {
        public Task Handle(DuplicateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed record DuplicateResultCommand(string Name) : ICommand<int>;

    private sealed class DuplicateResultCommandHandlerOne : ICommandHandler<DuplicateResultCommand, int>
    {
        public Task<int> Handle(DuplicateResultCommand command, CancellationToken cancellationToken) => Task.FromResult(0);
    }

    private sealed class DuplicateResultCommandHandlerTwo : ICommandHandler<DuplicateResultCommand, int>
    {
        public Task<int> Handle(DuplicateResultCommand command, CancellationToken cancellationToken) => Task.FromResult(1);
    }

    // -------------------------------------------------------------------
    // Finding 3 supporting types
    // -------------------------------------------------------------------

    private sealed record CancellableCommand(string Name) : ICommand;

    private sealed class CancellableCommandHandler(CancellationProbe probe) : ICommandHandler<CancellableCommand>
    {
        public Task Handle(CancellableCommand command, CancellationToken cancellationToken)
        {
            probe.HandlerToken = cancellationToken;
            return Task.CompletedTask;
        }
    }

    private sealed class CancellationObservingCommandBehavior(CancellationProbe probe) : IPipelineBehavior<CancellableCommand, Unit>
    {
        public async Task<Unit> Handle(CancellableCommand message, CancellationToken cancellationToken, MessageHandlerDelegate<Unit> next)
        {
            probe.BehaviorToken = cancellationToken;
            return await next().ConfigureAwait(false);
        }
    }

    // -------------------------------------------------------------------
    // Existing tests below (unchanged)
    // -------------------------------------------------------------------

    [Fact]
    public void CommandAndQueryResultContractsRemainInvariantResultingInExactDispatchContracts()
    {
        var commandVariance = typeof(ICommand<>).GetGenericArguments()[0].GenericParameterAttributes & GenericParameterAttributes.VarianceMask;
        var queryVariance = typeof(IQuery<>).GetGenericArguments()[0].GenericParameterAttributes & GenericParameterAttributes.VarianceMask;

        Assert.Equal(GenericParameterAttributes.None, commandVariance);
        Assert.Equal(GenericParameterAttributes.None, queryVariance);
    }

    private sealed record DuplicateQuery(string Name) : IQuery<string>;

    private sealed class DuplicateQueryHandlerOne : IQueryHandler<DuplicateQuery, string>
    {
        public Task<string> Handle(DuplicateQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(query.Name);
        }
    }

    private sealed class DuplicateQueryHandlerTwo : IQueryHandler<DuplicateQuery, string>
    {
        public Task<string> Handle(DuplicateQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(query.Name);
        }
    }

    private sealed record GetPipelineQuery(string Name) : IQuery<string>;

    private sealed class GetPipelineQueryHandler(List<string> events) : IQueryHandler<GetPipelineQuery, string>
    {
        public Task<string> Handle(GetPipelineQuery query, CancellationToken cancellationToken)
        {
            events.Add("handler");
            return Task.FromResult($"Handled {query.Name}");
        }
    }

    private sealed class OuterBehavior(List<string> events) : IPipelineBehavior<GetPipelineQuery, string>
    {
        public async Task<string> Handle(GetPipelineQuery message, CancellationToken cancellationToken, MessageHandlerDelegate<string> next)
        {
            events.Add("outer-before");
            var result = await next().ConfigureAwait(false);
            events.Add("outer-after");
            return result;
        }
    }

    private sealed class InnerBehavior(List<string> events) : IPipelineBehavior<GetPipelineQuery, string>
    {
        public async Task<string> Handle(GetPipelineQuery message, CancellationToken cancellationToken, MessageHandlerDelegate<string> next)
        {
            events.Add("inner-before");
            var result = await next().ConfigureAwait(false);
            events.Add("inner-after");
            return result;
        }
    }

    private sealed record PipelineCommand(string Name) : ICommand;

    private sealed class PipelineCommandHandler(List<string> events) : ICommandHandler<PipelineCommand>
    {
        public Task Handle(PipelineCommand command, CancellationToken cancellationToken)
        {
            events.Add("handler");
            return Task.CompletedTask;
        }
    }

    private sealed class OuterCommandBehavior(List<string> events) : IPipelineBehavior<PipelineCommand, Unit>
    {
        public async Task<Unit> Handle(PipelineCommand message, CancellationToken cancellationToken, MessageHandlerDelegate<Unit> next)
        {
            events.Add("outer-before");
            var result = await next().ConfigureAwait(false);
            events.Add("outer-after");
            return result;
        }
    }

    private sealed class InnerCommandBehavior(List<string> events) : IPipelineBehavior<PipelineCommand, Unit>
    {
        public async Task<Unit> Handle(PipelineCommand message, CancellationToken cancellationToken, MessageHandlerDelegate<Unit> next)
        {
            events.Add("inner-before");
            var result = await next().ConfigureAwait(false);
            events.Add("inner-after");
            return result;
        }
    }

    private sealed record ResultCommand(string Name) : ICommand<int>;

    private sealed class ResultCommandHandler(List<string> events) : ICommandHandler<ResultCommand, int>
    {
        public Task<int> Handle(ResultCommand command, CancellationToken cancellationToken)
        {
            events.Add("handler");
            return Task.FromResult(command.Name.Length);
        }
    }

    private sealed class OuterResultCommandBehavior(List<string> events) : IPipelineBehavior<ResultCommand, int>
    {
        public async Task<int> Handle(ResultCommand message, CancellationToken cancellationToken, MessageHandlerDelegate<int> next)
        {
            events.Add("outer-before");
            var result = await next().ConfigureAwait(false);
            events.Add("outer-after");
            return result;
        }
    }

    private sealed class InnerResultCommandBehavior(List<string> events) : IPipelineBehavior<ResultCommand, int>
    {
        public async Task<int> Handle(ResultCommand message, CancellationToken cancellationToken, MessageHandlerDelegate<int> next)
        {
            events.Add("inner-before");
            var result = await next().ConfigureAwait(false);
            events.Add("inner-after");
            return result;
        }
    }

    private sealed record ShortCircuitQuery(string Name) : IQuery<string>;

    private sealed class ShortCircuitQueryHandler(HandlerInvocationCounter counter) : IQueryHandler<ShortCircuitQuery, string>
    {
        public Task<string> Handle(ShortCircuitQuery query, CancellationToken cancellationToken)
        {
            counter.Count++;
            return Task.FromResult($"Handled {query.Name}");
        }
    }

    private sealed class ShortCircuitBehavior : IPipelineBehavior<ShortCircuitQuery, string>
    {
        public Task<string> Handle(ShortCircuitQuery message, CancellationToken cancellationToken, MessageHandlerDelegate<string> next)
        {
            return Task.FromResult($"Short-circuited {message.Name}");
        }
    }

    private sealed class HandlerInvocationCounter
    {
        public int Count { get; set; }
    }

    private sealed record ThrowingCommand(string Name) : ICommand;

    private sealed class ThrowingCommandHandler(Exception exception, List<string>? events = null) : ICommandHandler<ThrowingCommand>
    {
        public Task Handle(ThrowingCommand command, CancellationToken cancellationToken)
        {
            events?.Add("handler");
            throw exception;
        }
    }

    private sealed class ExceptionObservingBehavior(List<string> events) : IPipelineBehavior<ThrowingCommand, Unit>
    {
        public async Task<Unit> Handle(ThrowingCommand message, CancellationToken cancellationToken, MessageHandlerDelegate<Unit> next)
        {
            events.Add("behavior-before");

            try
            {
                return await next().ConfigureAwait(false);
            }
            finally
            {
                events.Add("behavior-finally");
            }
        }
    }

    private sealed record CancellableQuery(string Name) : IQuery<string>;

    private sealed class CancellableQueryHandler(CancellationProbe probe) : IQueryHandler<CancellableQuery, string>
    {
        public Task<string> Handle(CancellableQuery query, CancellationToken cancellationToken)
        {
            probe.HandlerToken = cancellationToken;
            return Task.FromResult($"Cancellable {query.Name}");
        }
    }

    private sealed class CancellationObservingBehavior(CancellationProbe probe) : IPipelineBehavior<CancellableQuery, string>
    {
        public async Task<string> Handle(CancellableQuery message, CancellationToken cancellationToken, MessageHandlerDelegate<string> next)
        {
            probe.BehaviorToken = cancellationToken;
            return await next().ConfigureAwait(false);
        }
    }

    private sealed class CancellationProbe
    {
        public CancellationToken BehaviorToken { get; set; }

        public CancellationToken HandlerToken { get; set; }
    }

    private sealed class TestServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services = [];

        public TestServiceProvider AddEnumerable<TService>(params TService[] services)
            where TService : class
        {
            _services[typeof(IEnumerable<TService>)] = services;
            return this;
        }

        public object? GetService(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out var service))
            {
                return service;
            }

            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return Array.CreateInstance(serviceType.GetGenericArguments()[0], 0);
            }

            return null;
        }
    }
}
