``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-AALAUH : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-ELKIGH : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|                   Method |  Runtime |        Mean |     Error |   StdDev | Ratio |
|------------------------- |--------- |------------:|----------:|---------:|------:|
|         FnaVector2Divide | .NET 6.0 |  4,355.3 ns |   6.73 ns |  5.25 ns |  1.00 |
|         FnaVector2Divide | .NET 7.0 |  4,385.2 ns |  15.08 ns | 14.10 ns |  1.01 |
|                          |          |             |           |          |       |
|         SysVector2Divide | .NET 6.0 |    773.2 ns |   3.20 ns |  2.83 ns |  1.00 |
|         SysVector2Divide | .NET 7.0 |    771.0 ns |   3.05 ns |  2.54 ns |  1.00 |
|                          |          |             |           |          |       |
| FnaVector2DivideNoInline | .NET 6.0 | 13,107.5 ns |  57.90 ns | 51.33 ns |  1.00 |
| FnaVector2DivideNoInline | .NET 7.0 | 13,283.6 ns | 107.50 ns | 95.30 ns |  1.01 |
|                          |          |             |           |          |       |
| SysVector2DivideNoInline | .NET 6.0 |  1,771.6 ns |   3.69 ns |  3.08 ns |  1.00 |
| SysVector2DivideNoInline | .NET 7.0 |  1,582.3 ns |  10.23 ns |  9.57 ns |  0.89 |
