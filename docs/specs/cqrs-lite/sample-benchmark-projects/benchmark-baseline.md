# CQRS Lite Benchmark Baseline

**Status:** Baseline  
**Date:** 2026-05-07  
**Spec:** `docs/specs/cqrs-lite/sample-benchmark-projects/sample-and-benchmark-projects.md`

---

## Benchmark Command

```
dotnet run -c Release --project src/CqrsLite.Benchmarks/CqrsLite.Benchmarks.csproj
```

---

## Target Framework and Runtime

| Item | Value |
|---|---|
| .NET SDK | 10.0.203 |
| Runtime | .NET 10.0.7 (10.0.726.21808), Arm64 RyuJIT AdvSIMD |
| BenchmarkDotNet | v0.14.0 |
| GC mode | Concurrent Workstation |

---

## Environment Note

Run on an Apple Silicon (M1) developer machine with 8 logical/physical cores running macOS Darwin 25.4.0. Numbers from this run reflect a single-user, low-noise desktop environment. Results on other architectures (e.g., x64) or under server GC will differ; treat these numbers as a relative reference, not an absolute performance contract.

---

## Baseline Numbers

| Benchmark category | Mean | StdDev | Allocated |
|---|---|---|---|
| Warm `ICommand` dispatch | 98.94 ns | 0.39 ns | 224 B |
| Warm `ICommand<TResult>` dispatch | 124.80 ns | 0.36 ns | 440 B |
| Warm `IQuery<TResult>` dispatch | 124.80 ns | 0.44 ns | 440 B |
| `ScanHandlers` registration | 878.22 ns | 2.19 ns | 2,153 B |

All dispatch benchmarks measure the steady-state path after the Dispatcher's internal delegate cache is populated (warmup runs in `[GlobalSetup]`). The `ScanHandlers` benchmark measures only registration-time reflection; no dispatch is performed in its measured iteration.

---

## Interpretation and Comparison Guidance

- **Dispatch overhead is sub-microsecond** for all three warm-dispatch categories. `ICommand` dispatch is lighter (~99 ns, 224 B) than result-returning Command or Query dispatch (~125 ns, 440 B) because void Commands return the cached `Unit.Value` without an additional allocation in the result path.
- **`ScanHandlers` is ~8–9× slower than a single dispatch call** (~878 ns). This is expected: scanning performs assembly reflection at registration time, not at dispatch time. The cost is paid once per `AddCqrsLite` call, not per message.
- When reviewing a future change, treat a **>10% increase** in mean for any category as a signal worth investigating. Low StdDev values in this run (<1% of mean) indicate a stable measurement baseline.
- This artifact is a **comparison aid, not a performance contract**. Do not gate CI on these numbers. Re-run the benchmark suite and update this file deliberately after changes that are likely to affect dispatch or registration-time performance (e.g., changes to `Dispatcher.cs`, `HandlerScanner.cs`, or `CqrsLiteBuilder.cs`).
