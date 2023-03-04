``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-EVUISN : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-WOQYGC : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|             Method |  Runtime |       Mean |    Error |   StdDev | Ratio |
|------------------- |--------- |-----------:|---------:|---------:|------:|
| FnaVector2Subtract | .NET 6.0 | 4,418.8 ns | 39.74 ns | 35.23 ns |  1.00 |
| FnaVector2Subtract | .NET 7.0 | 4,402.4 ns | 23.46 ns | 19.59 ns |  1.00 |
|                    |          |            |          |          |       |
| SysVector2Subtract | .NET 6.0 |   511.7 ns |  1.85 ns |  1.44 ns |  1.00 |
| SysVector2Subtract | .NET 7.0 |   515.1 ns |  5.05 ns |  3.95 ns |  1.01 |
