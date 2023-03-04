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
| FnaVector2Multiply | .NET 6.0 | 4,450.2 ns | 48.05 ns | 42.59 ns |  1.00 |
| FnaVector2Multiply | .NET 7.0 | 4,434.1 ns | 39.03 ns | 34.60 ns |  1.00 |
|                    |          |            |          |          |       |
| SysVector2Multiply | .NET 6.0 |   518.6 ns |  6.59 ns |  5.51 ns |  1.00 |
| SysVector2Multiply | .NET 7.0 |   516.8 ns |  4.90 ns |  4.34 ns |  1.00 |
