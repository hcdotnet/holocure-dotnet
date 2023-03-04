``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-EVUISN : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-WOQYGC : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|        Method |  Runtime |       Mean |    Error |   StdDev | Ratio |
|-------------- |--------- |-----------:|---------:|---------:|------:|
| FnaVector2Add | .NET 6.0 | 4,441.1 ns | 40.80 ns | 38.16 ns |  1.00 |
| FnaVector2Add | .NET 7.0 | 4,436.5 ns | 51.06 ns | 47.76 ns |  1.00 |
|               |          |            |          |          |       |
| SysVector2Add | .NET 6.0 |   512.0 ns |  3.07 ns |  2.40 ns |  1.00 |
| SysVector2Add | .NET 7.0 |   515.4 ns |  6.79 ns |  6.35 ns |  1.01 |
