``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-AALAUH : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-ELKIGH : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|                     Method |  Runtime |        Mean |     Error |    StdDev | Ratio | RatioSD |
|--------------------------- |--------- |------------:|----------:|----------:|------:|--------:|
|         FnaVector2Subtract | .NET 6.0 |  4,415.4 ns |  48.85 ns |  45.70 ns |  1.00 |    0.00 |
|         FnaVector2Subtract | .NET 7.0 |  4,419.1 ns |  22.77 ns |  19.01 ns |  1.00 |    0.01 |
|                            |          |             |           |           |       |         |
|         SysVector2Subtract | .NET 6.0 |    519.8 ns |   9.91 ns |   9.73 ns |  1.00 |    0.00 |
|         SysVector2Subtract | .NET 7.0 |    513.7 ns |   5.57 ns |   4.65 ns |  0.99 |    0.02 |
|                            |          |             |           |           |       |         |
| FnaVector2SubtractNoInline | .NET 6.0 | 11,341.2 ns | 198.28 ns | 185.47 ns |  1.00 |    0.00 |
| FnaVector2SubtractNoInline | .NET 7.0 | 11,440.9 ns | 137.42 ns | 128.54 ns |  1.01 |    0.02 |
|                            |          |             |           |           |       |         |
| SysVector2SubtractNoInline | .NET 6.0 |  1,573.8 ns |   6.81 ns |   5.31 ns |  1.00 |    0.00 |
| SysVector2SubtractNoInline | .NET 7.0 |  1,362.8 ns |   6.44 ns |   5.38 ns |  0.87 |    0.00 |
