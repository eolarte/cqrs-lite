using BenchmarkDotNet.Attributes;
using CqrsLite;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsLite.Benchmarks;

/// <summary>
/// Measures the four CQRS Lite hot paths:
///   1. Warm ICommand dispatch
///   2. Warm ICommand&lt;TResult&gt; dispatch
///   3. Warm IQuery&lt;TResult&gt; dispatch
///   4. ScanHandlers registration-time cost
///
/// Dispatch benchmarks run after GlobalSetup populates the delegate cache so
/// first-dispatch compilation is excluded from every measured iteration.
/// The scanning benchmark rebuilds a fresh ServiceCollection per iteration to
/// isolate registration-time reflection; no dispatch is performed in its measured path.
/// </summary>
[MemoryDiagnoser]
public class CqrsLiteBenchmarks
{
    private ServiceProvider? _serviceProvider;
    private IDispatcher _dispatcher = null!;

    private readonly BenchmarkCommand _command = new();
    private readonly BenchmarkResultCommand _resultCommand = new();
    private readonly BenchmarkQuery _query = new();

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder =>
        {
            builder.AddCommandHandler<BenchmarkCommand, BenchmarkCommandHandler>();
            builder.AddCommandHandler<BenchmarkResultCommand, string, BenchmarkResultCommandHandler>();
            builder.AddQueryHandler<BenchmarkQuery, string, BenchmarkQueryHandler>();
        });

        _serviceProvider = services.BuildServiceProvider();
        _dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();

        // Warm the delegate cache before measurement begins so all three
        // dispatch benchmarks measure only the steady-state path.
        await _dispatcher.Send(_command);
        await _dispatcher.Send(_resultCommand);
        await _dispatcher.Query(_query);
    }

    [GlobalCleanup]
    public void GlobalCleanup() => _serviceProvider?.Dispose();

    [Benchmark(Description = "Warm ICommand dispatch")]
    public Task WarmVoidCommandDispatch() => _dispatcher.Send(_command);

    [Benchmark(Description = "Warm ICommand<TResult> dispatch")]
    public Task<string> WarmResultCommandDispatch() => _dispatcher.Send(_resultCommand);

    [Benchmark(Description = "Warm IQuery<TResult> dispatch")]
    public Task<string> WarmQueryDispatch() => _dispatcher.Query(_query);

    /// <summary>
    /// Measures only ScanHandlers registration-time reflection.
    /// IDispatcher is never invoked inside this measured iteration.
    /// </summary>
    [Benchmark(Description = "ScanHandlers registration")]
    public void HandlerScanningRegistration()
    {
        var services = new ServiceCollection();
        services.AddCqrsLite(builder =>
            builder.ScanHandlers(typeof(BenchmarkCommandHandler).Assembly));
    }
}
