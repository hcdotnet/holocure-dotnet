using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace HCDN.Benchmarks; 

[MemoryDiagnoser(displayGenColumns: false)]
[DisassemblyDiagnoser]
// [HideColumns("Error", "StdDev", "Median", "RatioSD")]
internal static class Program {
    public static void Main(string[] args) {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}