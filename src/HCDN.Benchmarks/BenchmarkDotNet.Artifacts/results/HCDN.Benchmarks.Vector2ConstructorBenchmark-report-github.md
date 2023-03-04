``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-EVUISN : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-WOQYGC : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|                Method |  Runtime |     Mean |    Error |  StdDev | Ratio | RatioSD |
|---------------------- |--------- |---------:|---------:|--------:|------:|--------:|
| FnaVector2Constructor | .NET 6.0 | 534.4 ns |  4.15 ns | 3.47 ns |  1.00 |    0.00 |
| FnaVector2Constructor | .NET 7.0 | 536.5 ns |  4.68 ns | 3.65 ns |  1.00 |    0.01 |
|                       |          |          |          |         |       |         |
| SysVector2Constructor | .NET 6.0 | 680.2 ns |  5.35 ns | 4.74 ns |  1.00 |    0.00 |
| SysVector2Constructor | .NET 7.0 | 683.0 ns | 10.15 ns | 9.49 ns |  1.01 |    0.02 |
