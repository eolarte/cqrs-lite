```

BenchmarkDotNet v0.14.0, macOS 26.4.1 (25E253) [Darwin 25.4.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.203
  [Host]     : .NET 10.0.7 (10.0.726.21808), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 10.0.7 (10.0.726.21808), Arm64 RyuJIT AdvSIMD


```
| Method                            | Mean      | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|---------------------------------- |----------:|---------:|---------:|-------:|-------:|----------:|
| &#39;Warm ICommand dispatch&#39;          |  98.94 ns | 0.442 ns | 0.392 ns | 0.0356 |      - |     224 B |
| &#39;Warm ICommand&lt;TResult&gt; dispatch&#39; | 124.80 ns | 0.425 ns | 0.355 ns | 0.0701 |      - |     440 B |
| &#39;Warm IQuery&lt;TResult&gt; dispatch&#39;   | 124.80 ns | 0.562 ns | 0.439 ns | 0.0701 |      - |     440 B |
| &#39;ScanHandlers registration&#39;       | 878.22 ns | 2.475 ns | 2.194 ns | 0.3424 | 0.0010 |    2153 B |
