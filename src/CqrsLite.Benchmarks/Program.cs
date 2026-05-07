using BenchmarkDotNet.Running;
using CqrsLite.Benchmarks;

// Run all four benchmark categories: warm ICommand dispatch, warm ICommand<TResult> dispatch,
// warm IQuery<TResult> dispatch, and ScanHandlers registration.
BenchmarkRunner.Run<CqrsLiteBenchmarks>(null, args);
