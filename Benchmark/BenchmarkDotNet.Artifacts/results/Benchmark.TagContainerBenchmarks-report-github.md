```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.22631.5335/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600 3.50GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2


```
| Method              | Mean          | Error       | StdDev      | Gen0   | Gen1   | Allocated |
|-------------------- |--------------:|------------:|------------:|-------:|-------:|----------:|
| AddBatchTags        | 29,719.491 ns | 285.0894 ns | 266.6728 ns | 0.4883 |      - |    8176 B |
| RemoveBatchTags     | 22,100.089 ns |  94.3951 ns |  83.6788 ns |      - |      - |         - |
| CheckBatchExistence | 32,977.508 ns | 122.1473 ns | 108.2804 ns |      - |      - |         - |
| HasAllTags_LargeSet | 28,877.163 ns | 558.5099 ns | 643.1808 ns | 4.3640 | 0.5493 |   73320 B |
| HasAnyTag_LargeSet  | 30,685.959 ns | 482.7531 ns | 451.5676 ns | 4.3640 | 0.5493 |   73320 B |
| AddMaxStackTag      |      5.210 ns |   0.0790 ns |   0.0660 ns |      - |      - |         - |
| RemoveFromMaxStack  |      4.356 ns |   0.0567 ns |   0.0531 ns |      - |      - |         - |
