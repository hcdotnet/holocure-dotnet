using BenchmarkDotNet.Attributes;
using FnaVector2 = Microsoft.Xna.Framework.Vector2;
using SysVector2 = System.Numerics.Vector2;

namespace HCDN.Benchmarks; 

public class Vector2ConstructorBenchmark {
    private const int vector_count = 1000;
    
    private readonly FnaVector2[] fnaVectors = new FnaVector2[vector_count];
    private readonly SysVector2[] sysVectors = new SysVector2[vector_count];
    
    [Benchmark]
    public void FnaVector2Constructor() {
        for (var i = 0; i < vector_count; i++)
            fnaVectors[i] = new FnaVector2(i, i);
    }
    
    [Benchmark]
    public void SysVector2Constructor() {
        for (var i = 0; i < vector_count; i++)
            sysVectors[i] = new SysVector2(i, i);
    }
}

public class Vector2AddBenchmark {
    private const int vector_count = 1000;
    
    private readonly FnaVector2[] fnaVectors = new FnaVector2[vector_count];
    private readonly SysVector2[] sysVectors = new SysVector2[vector_count];
    
    [GlobalSetup]
    public void Setup() {
        for (var i = 0; i < vector_count; i++) {
            fnaVectors[i] = new FnaVector2(i, i);
            sysVectors[i] = new SysVector2(i, i);
        }
    }
    
    [Benchmark]
    public void FnaVector2Add() {
        for (var i = 0; i < vector_count; i++)
            fnaVectors[i] += fnaVectors[i];
    }
    
    [Benchmark]
    public void SysVector2Add() {
        for (var i = 0; i < vector_count; i++)
            sysVectors[i] += sysVectors[i];
    }
}

public class Vector2SubtractBenchmark {
    private const int vector_count = 1000;
    
    private readonly FnaVector2[] fnaVectors = new FnaVector2[vector_count];
    private readonly SysVector2[] sysVectors = new SysVector2[vector_count];
    
    [GlobalSetup]
    public void Setup() {
        for (var i = 0; i < vector_count; i++) {
            fnaVectors[i] = new FnaVector2(i, i);
            sysVectors[i] = new SysVector2(i, i);
        }
    }
    
    [Benchmark]
    public void FnaVector2Subtract() {
        for (var i = 0; i < vector_count; i++)
            fnaVectors[i] -= fnaVectors[i];
    }
    
    [Benchmark]
    public void SysVector2Subtract() {
        for (var i = 0; i < vector_count; i++)
            sysVectors[i] -= sysVectors[i];
    }
}

public class Vector2MultiplyBenchmark {
    private const int vector_count = 1000;
    
    private readonly FnaVector2[] fnaVectors = new FnaVector2[vector_count];
    private readonly SysVector2[] sysVectors = new SysVector2[vector_count];
    
    [GlobalSetup]
    public void Setup() {
        for (var i = 0; i < vector_count; i++) {
            fnaVectors[i] = new FnaVector2(i, i);
            sysVectors[i] = new SysVector2(i, i);
        }
    }
    
    [Benchmark]
    public void FnaVector2Multiply() {
        for (var i = 0; i < vector_count; i++)
            fnaVectors[i] *= fnaVectors[i];
    }
    
    [Benchmark]
    public void SysVector2Multiply() {
        for (var i = 0; i < vector_count; i++)
            sysVectors[i] *= sysVectors[i];
    }
}

public class Vector2DivideBenchmark {
    private const int vector_count = 1000;
    
    private readonly FnaVector2[] fnaVectors = new FnaVector2[vector_count];
    private readonly SysVector2[] sysVectors = new SysVector2[vector_count];
    
    [GlobalSetup]
    public void Setup() {
        for (var i = 0; i < vector_count; i++) {
            fnaVectors[i] = new FnaVector2(i, i);
            sysVectors[i] = new SysVector2(i, i);
        }
    }
    
    [Benchmark]
    public void FnaVector2Divide() {
        for (var i = 0; i < vector_count; i++)
            fnaVectors[i] /= fnaVectors[i];
    }
    
    [Benchmark]
    public void SysVector2Divide() {
        for (var i = 0; i < vector_count; i++)
            sysVectors[i] /= sysVectors[i];
    }
}
