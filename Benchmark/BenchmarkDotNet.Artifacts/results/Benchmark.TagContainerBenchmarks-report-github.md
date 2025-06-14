```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.22631.5335/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600 3.50GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100
  [Host]   : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  InvocationCount=1  
UnrollFactor=1  

```
| Method              | Mean       | Error      | StdDev     | Allocated |
|-------------------- |-----------:|-----------:|-----------:|----------:|
| AddBatchTags        | 245.018 μs | 18.1056 μs | 53.1006 μs |     400 B |
| RemoveBatchTags     | 252.280 μs | 13.3970 μs | 38.8672 μs |     400 B |
| CheckBatchExistence | 214.025 μs | 14.2589 μs | 41.3676 μs |     400 B |
| HasAllTags_LargeSet | 242.153 μs |  5.5661 μs | 15.5162 μs |     504 B |
| HasAnyTag_LargeSet  |   7.130 μs |  0.4678 μs |  1.3346 μs |     504 B |
| AddMaxStackTag      |   3.870 μs |  0.3508 μs |  1.0123 μs |     400 B |
| RemoveFromMaxStack  |   3.051 μs |  0.2739 μs |  0.7903 μs |     400 B |
