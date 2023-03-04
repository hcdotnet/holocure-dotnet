``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-AALAUH : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-ELKIGH : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|                Method |  Runtime |        Mean |     Error |    StdDev | Ratio |
|---------------------- |--------- |------------:|----------:|----------:|------:|
|         FnaVector2Add | .NET 6.0 |  4,393.4 ns |  36.51 ns |  34.15 ns |  1.00 |
|         FnaVector2Add | .NET 7.0 |  4,469.6 ns |  45.39 ns |  40.24 ns |  1.02 |
|                       |          |             |           |           |       |
|         SysVector2Add | .NET 6.0 |    508.9 ns |   1.71 ns |   1.52 ns |  1.00 |
|         SysVector2Add | .NET 7.0 |    515.1 ns |   2.66 ns |   2.08 ns |  1.01 |
|                       |          |             |           |           |       |
| FnaVector2AddNoInline | .NET 6.0 | 11,190.5 ns |  72.45 ns |  64.23 ns |  1.00 |
| FnaVector2AddNoInline | .NET 7.0 | 11,369.5 ns | 150.05 ns | 140.36 ns |  1.02 |
|                       |          |             |           |           |       |
| SysVector2AddNoInline | .NET 6.0 |  1,580.0 ns |  13.89 ns |  12.31 ns |  1.00 |
| SysVector2AddNoInline | .NET 7.0 |  1,375.4 ns |   9.50 ns |   7.93 ns |  0.87 |
