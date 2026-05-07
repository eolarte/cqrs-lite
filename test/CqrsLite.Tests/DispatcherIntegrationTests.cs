using CqrsLite;
using CqrsLite.InvalidHandlers;
using CqrsLite.InvalidShapeHandlers;
using CqrsLite.TestHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsLite.Tests;

public sealed class DispatcherIntegrationTests
{
    [Fact]
    public async Task ACompositionRootCanRegisterOneQueryHandlerExplicitlyResultingInAQueryResultBeingReturned()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder => builder.AddQueryHandler<GetGreetingQuery, string, GetGreetingQueryHandler>());

        using var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var result = await dispatcher.Query(new GetGreetingQuery("Ada"), CancellationToken.None);

        Assert.Equal("Hello Ada", result);
    }

    [Fact]
    public async Task ACompositionRootCanRegisterOneCommandHandlerExplicitlyResultingInSuccessfulCommandDispatch()
    {
        var invocations = new CommandInvocationRecorder();
        var services = new ServiceCollection();
        services.AddSingleton(invocations);
        services.AddCqrsLite(builder => builder.AddCommandHandler<CreateGreetingCommand, CreateGreetingCommandHandler>());

        using var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        await dispatcher.Send(new CreateGreetingCommand("Ada"));

        Assert.Equal(["Ada"], invocations.Names);
    }

    [Fact]
    public async Task ACompositionRootCanRegisterOneResultCommandHandlerExplicitlyResultingInACommandResultBeingReturned()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder => builder.AddCommandHandler<CreateGreetingIdCommand, int, CreateGreetingIdCommandHandler>());

        using var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var result = await dispatcher.Send(new CreateGreetingIdCommand("Ada"));

        Assert.Equal(3, result);
    }

    [Fact]
    public async Task ACallerCanDispatchACommandOrQueryWithNoRegisteredHandlerResultingInAnExplicitRuntimeError()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite();

        using var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var commandException = await Assert.ThrowsAsync<HandlerNotFoundException>(() => dispatcher.Send(new CreateGreetingCommand("Ada")));
        var queryException = await Assert.ThrowsAsync<HandlerNotFoundException>(() => dispatcher.Query(new GetGreetingQuery("Ada")));

        Assert.Contains(nameof(CreateGreetingCommand), commandException.Message);
        Assert.Contains(nameof(GetGreetingQuery), queryException.Message);
    }

    [Fact]
    public async Task ACompositionRootCanScanSelectedAssembliesResultingInTheSameSuccessfulDispatchSemanticsAsExplicitRegistration()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder => builder.ScanHandlers(typeof(ScannedGreetingQueryHandler).Assembly));

        using var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var result = await dispatcher.Query(new ScannedGreetingQuery("Grace"));

        Assert.Equal("Scanned Grace", result);
    }

    [Fact]
    public void ACompositionRootCanScanSelectedAssembliesContainingUnrelatedTypesResultingInOnlySupportedHandlersBeingRegistered()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder => builder.ScanHandlers(typeof(ScannedGreetingQueryHandler).Assembly));

        Assert.DoesNotContain(services, descriptor =>
            descriptor.ServiceType == typeof(UnrelatedScannedType) ||
            descriptor.ImplementationType == typeof(UnrelatedScannedType));

        using var serviceProvider = services.BuildServiceProvider();

        Assert.Null(serviceProvider.GetService<UnrelatedScannedType>());
    }

    [Fact]
    public async Task ACompositionRootCanMixScannedHandlersWithExplicitPipelineBehaviorsResultingInTheSameRuntimeDispatchSemantics()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder =>
        {
            builder.ScanHandlers(typeof(ScannedGreetingQueryHandler).Assembly);
            builder.AddPipelineBehavior<PrefixedScannedGreetingBehavior>();
        });

        using var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var result = await dispatcher.Query(new ScannedGreetingQuery("Grace"));

        Assert.Equal("Observed Scanned Grace", result);
    }

    [Fact]
    public void ACompositionRootCanScanSelectedAssembliesContainingDuplicateHandlersResultingInDeterministicRegistrationFailure()
    {
        var services = new ServiceCollection();

        var duplicateException = Assert.Throws<InvalidHandlerRegistrationException>(() =>
            services.AddCqrsLite(builder => builder.ScanHandlers(typeof(DuplicateGreetingQueryHandlerOne).Assembly)));

        Assert.Contains(nameof(DuplicateGreetingQuery), duplicateException.Message);
    }

    [Fact]
    public void ACompositionRootCanScanSelectedAssembliesContainingANullAssemblyResultingInAClearArgumentError()
    {
        var services = new ServiceCollection();

        var exception = Assert.Throws<ArgumentException>(() =>
            services.AddCqrsLite(builder => builder.ScanHandlers(typeof(ScannedGreetingQueryHandler).Assembly, null!)));

        Assert.Equal("assemblies", exception.ParamName);
        Assert.Contains("null", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ACompositionRootCanScanSelectedAssembliesContainingInvalidHandlerShapesResultingInDeterministicRegistrationFailure()
    {
        var services = new ServiceCollection();

        var invalidShapeException = Assert.Throws<InvalidHandlerRegistrationException>(() =>
            services.AddCqrsLite(builder => builder.ScanHandlers(typeof(InvalidGreetingHandler).Assembly)));

        Assert.Contains(nameof(InvalidGreetingHandler), invalidShapeException.Message);
    }

    // -------------------------------------------------------------------
    // Finding 6: ScanHandlers with zero assemblies
    // -------------------------------------------------------------------

    [Fact]
    public void ACompositionRootCannotScanZeroAssembliesResultingInArgumentException()
    {
        var services = new ServiceCollection();

        var exception = Assert.Throws<ArgumentException>(() =>
            services.AddCqrsLite(builder => builder.ScanHandlers()));

        Assert.Equal("assemblies", exception.ParamName);
        Assert.Contains("At least one assembly", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    // -------------------------------------------------------------------
    // Finding 7: Explicit registration followed by scanning the same handler type
    // -------------------------------------------------------------------

    [Fact]
    public void ACompositionRootCannotRegisterAHandlerExplicitlyAndThenScanTheSameHandlerTypeResultingInDuplicateRegistrationFailure()
    {
        var services = new ServiceCollection();

        var exception = Assert.Throws<InvalidHandlerRegistrationException>(() =>
            services.AddCqrsLite(builder =>
            {
                // Explicit registration of ScannedGreetingQueryHandler for IQueryHandler<ScannedGreetingQuery, string>
                builder.AddQueryHandler<ScannedGreetingQuery, string, ScannedGreetingQueryHandler>();
                // Scanning the same assembly now finds the same service type already registered
                builder.ScanHandlers(typeof(ScannedGreetingQueryHandler).Assembly);
            }));

        Assert.Contains("Duplicate", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    // -------------------------------------------------------------------
    // Finding 8: ScanHandlers called with duplicate assembly references
    // -------------------------------------------------------------------

    [Fact]
    public async Task ACompositionRootCanScanTheSameAssemblyTwiceResultingInASingleHandlerRegistrationWithoutErrors()
    {
        var services = new ServiceCollection();

        // Passing the same assembly reference twice — Distinct() in HandlerScanner deduplicates it.
        services.AddCqrsLite(builder =>
            builder.ScanHandlers(typeof(ScannedGreetingQueryHandler).Assembly, typeof(ScannedGreetingQueryHandler).Assembly));

        // ScannedGreetingQueryHandler must be registered exactly once.
        var registrations = services.Where(d => d.ImplementationType == typeof(ScannedGreetingQueryHandler)).ToList();
        Assert.Single(registrations);

        using var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        var result = await dispatcher.Query(new ScannedGreetingQuery("Grace"));
        Assert.Equal("Scanned Grace", result);
    }

    // -------------------------------------------------------------------
    // Finding 9: AddCqrsLite called more than once on the same IServiceCollection
    // -------------------------------------------------------------------

    [Fact]
    public void ACompositionRootCallingAddCqrsLiteTwiceRegistersExactlyOneDispatcherAndMergesBothHandlerConfigurations()
    {
        var services = new ServiceCollection();

        // First call registers IDispatcher and a query handler.
        services.AddCqrsLite(builder => builder.AddQueryHandler<GetGreetingQuery, string, GetGreetingQueryHandler>());

        // Second call must not register a second IDispatcher, but must still apply its builder action.
        services.AddCqrsLite(builder => builder.AddQueryHandler<SecondGreetingQuery, string, SecondGreetingQueryHandler>());

        // IDispatcher is registered exactly once.
        var dispatcherRegistrations = services.Where(d => d.ServiceType == typeof(IDispatcher)).ToList();
        Assert.Single(dispatcherRegistrations);

        using var serviceProvider = services.BuildServiceProvider();

        // Both handler configurations are visible to the container.
        Assert.NotNull(serviceProvider.GetService<IQueryHandler<GetGreetingQuery, string>>());
        Assert.NotNull(serviceProvider.GetService<IQueryHandler<SecondGreetingQuery, string>>());
    }

    // -------------------------------------------------------------------
    // Finding 11: AddPipelineBehavior with a non-behavior concrete type
    // -------------------------------------------------------------------

    [Fact]
    public void ACompositionRootAddingANonBehaviorConcreteTypeAsAPipelineBehaviorResultsInArgumentException()
    {
        var services = new ServiceCollection();

        var exception = Assert.Throws<ArgumentException>(() =>
            services.AddCqrsLite(builder => builder.AddPipelineBehavior(typeof(string))));

        Assert.Equal("behaviorType", exception.ParamName);
        Assert.Contains("IPipelineBehavior", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    // -------------------------------------------------------------------
    // Finding 12: AddPipelineBehavior with an open-generic that does not close IPipelineBehavior<,>
    // -------------------------------------------------------------------

    [Fact]
    public void ACompositionRootAddingAnOpenGenericNonBehaviorTypeAsAPipelineBehaviorResultsInArgumentException()
    {
        var services = new ServiceCollection();

        // List<> is open-generic but does not implement IPipelineBehavior<,>.
        var exception = Assert.Throws<ArgumentException>(() =>
            services.AddCqrsLite(builder => builder.AddPipelineBehavior(typeof(List<>))));

        Assert.Equal("behaviorType", exception.ParamName);
        Assert.Contains("IPipelineBehavior", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    // -------------------------------------------------------------------
    // Finding 14: Dispatcher static lambda cache is scope-neutral
    // -------------------------------------------------------------------

    [Fact]
    public async Task TwoSeparateServiceProvidersWithDifferentHandlersForTheSameMessageTypeEachResolveTheirOwnHandler()
    {
        // Build first service provider with HandlerA
        var servicesA = new ServiceCollection();
        servicesA.AddCqrsLite(builder => builder.AddQueryHandler<IsolationQuery, string, IsolationQueryHandlerA>());
        using var providerA = servicesA.BuildServiceProvider();

        // Build second service provider with HandlerB
        var servicesB = new ServiceCollection();
        servicesB.AddCqrsLite(builder => builder.AddQueryHandler<IsolationQuery, string, IsolationQueryHandlerB>());
        using var providerB = servicesB.BuildServiceProvider();

        var dispatcherA = providerA.GetRequiredService<IDispatcher>();
        var dispatcherB = providerB.GetRequiredService<IDispatcher>();

        // The static lambda cache stores type-keyed invocation delegates only —
        // no DI references. Each Dispatcher instance uses its own IServiceProvider
        // at call time, so scope-A and scope-B resolve independent handler instances.
        var resultA = await dispatcherA.Query(new IsolationQuery());
        var resultB = await dispatcherB.Query(new IsolationQuery());

        Assert.Equal("HandlerA", resultA);
        Assert.Equal("HandlerB", resultB);
    }

    private sealed record IsolationQuery : IQuery<string>;

    private sealed class IsolationQueryHandlerA : IQueryHandler<IsolationQuery, string>
    {
        public Task<string> Handle(IsolationQuery query, CancellationToken cancellationToken)
            => Task.FromResult("HandlerA");
    }

    private sealed class IsolationQueryHandlerB : IQueryHandler<IsolationQuery, string>
    {
        public Task<string> Handle(IsolationQuery query, CancellationToken cancellationToken)
            => Task.FromResult("HandlerB");
    }

    // -------------------------------------------------------------------
    // Finding 9 supporting types
    // -------------------------------------------------------------------

    private sealed record SecondGreetingQuery(string Name) : IQuery<string>;

    private sealed class SecondGreetingQueryHandler : IQueryHandler<SecondGreetingQuery, string>
    {
        public Task<string> Handle(SecondGreetingQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Second {query.Name}");
        }
    }

    private sealed record GetGreetingQuery(string Name) : IQuery<string>;

    private sealed class GetGreetingQueryHandler : IQueryHandler<GetGreetingQuery, string>
    {
        public Task<string> Handle(GetGreetingQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Hello {query.Name}");
        }
    }

    private sealed record CreateGreetingCommand(string Name) : ICommand;

    private sealed class CreateGreetingCommandHandler(CommandInvocationRecorder recorder) : ICommandHandler<CreateGreetingCommand>
    {
        public Task Handle(CreateGreetingCommand command, CancellationToken cancellationToken)
        {
            recorder.Names.Add(command.Name);
            return Task.CompletedTask;
        }
    }

    private sealed record CreateGreetingIdCommand(string Name) : ICommand<int>;

    private sealed class CreateGreetingIdCommandHandler : ICommandHandler<CreateGreetingIdCommand, int>
    {
        public Task<int> Handle(CreateGreetingIdCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(command.Name.Length);
        }
    }

    private sealed class CommandInvocationRecorder
    {
        public List<string> Names { get; } = [];
    }

    private sealed class PrefixedScannedGreetingBehavior : IPipelineBehavior<ScannedGreetingQuery, string>
    {
        public async Task<string> Handle(ScannedGreetingQuery message, CancellationToken cancellationToken, MessageHandlerDelegate<string> next)
        {
            var result = await next().ConfigureAwait(false);
            return $"Observed {result}";
        }
    }
}
