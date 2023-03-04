``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2604/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-AALAUH : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-ELKIGH : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|                                Method |  Runtime |     Mean |   Error |  StdDev | Ratio |
|-------------------------------------- |--------- |---------:|--------:|--------:|------:|
|   FnaVector2ConstructorInt32MultiCtor | .NET 6.0 | 535.7 ns | 2.47 ns | 2.31 ns |  1.00 |
|   FnaVector2ConstructorInt32MultiCtor | .NET 7.0 | 525.9 ns | 1.72 ns | 1.61 ns |  0.98 |
|                                       |          |          |         |         |       |
|   SysVector2ConstructorInt32MultiCtor | .NET 6.0 | 568.1 ns | 4.20 ns | 3.93 ns |  1.00 |
|   SysVector2ConstructorInt32MultiCtor | .NET 7.0 | 667.6 ns | 2.76 ns | 2.45 ns |  1.18 |
|                                       |          |          |         |         |       |
|  FnaVector2ConstructorInt32SingleCtor | .NET 6.0 | 531.9 ns | 2.98 ns | 2.64 ns |  1.00 |
|  FnaVector2ConstructorInt32SingleCtor | .NET 7.0 | 527.1 ns | 2.47 ns | 2.07 ns |  0.99 |
|                                       |          |          |         |         |       |
|  SysVector2ConstructorInt32SingleCtor | .NET 6.0 | 497.6 ns | 4.42 ns | 4.13 ns |  1.00 |
|  SysVector2ConstructorInt32SingleCtor | .NET 7.0 | 452.2 ns | 0.81 ns | 0.67 ns |  0.91 |
|                                       |          |          |         |         |       |
|  FnaVector2ConstructorSingleMultiCtor | .NET 6.0 | 507.6 ns | 1.16 ns | 1.03 ns |  1.00 |
|  FnaVector2ConstructorSingleMultiCtor | .NET 7.0 | 503.5 ns | 1.51 ns | 1.42 ns |  0.99 |
|                                       |          |          |         |         |       |
|  SysVector2ConstructorSingleMultiCtor | .NET 6.0 | 559.4 ns | 0.93 ns | 0.83 ns |  1.00 |
|  SysVector2ConstructorSingleMultiCtor | .NET 7.0 | 558.2 ns | 2.19 ns | 1.83 ns |  1.00 |
|                                       |          |          |         |         |       |
| FnaVector2ConstructorSingleSingleCtor | .NET 6.0 | 507.1 ns | 1.46 ns | 1.37 ns |  1.00 |
| FnaVector2ConstructorSingleSingleCtor | .NET 7.0 | 504.0 ns | 3.15 ns | 2.63 ns |  0.99 |
|                                       |          |          |         |         |       |
| SysVector2ConstructorSingleSingleCtor | .NET 6.0 | 465.8 ns | 1.92 ns | 1.70 ns |  1.00 |
| SysVector2ConstructorSingleSingleCtor | .NET 7.0 | 453.0 ns | 1.99 ns | 1.77 ns |  0.97 |
