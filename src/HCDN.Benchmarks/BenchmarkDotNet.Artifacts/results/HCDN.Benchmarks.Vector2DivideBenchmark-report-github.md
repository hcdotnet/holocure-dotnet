``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-EVUISN : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-WOQYGC : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|           Method |  Runtime |       Mean |    Error |   StdDev | Ratio | RatioSD |
|----------------- |--------- |-----------:|---------:|---------:|------:|--------:|
| FnaVector2Divide | .NET 6.0 | 4,456.9 ns | 66.78 ns | 59.20 ns |  1.00 |    0.00 |
| FnaVector2Divide | .NET 7.0 | 4,444.5 ns | 53.27 ns | 47.22 ns |  1.00 |    0.02 |
|                  |          |            |          |          |       |         |
| SysVector2Divide | .NET 6.0 |   780.9 ns |  7.89 ns |  6.99 ns |  1.00 |    0.00 |
| SysVector2Divide | .NET 7.0 |   784.7 ns |  7.32 ns |  6.49 ns |  1.00 |    0.01 |
