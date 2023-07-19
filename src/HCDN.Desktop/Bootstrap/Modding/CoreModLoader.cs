using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using HCDN.API.Modding;

namespace HCDN.Desktop.Bootstrap.Modding;

internal sealed class CoreModLoader : IModLoader<ICoreInitializer> {
    private class IsolatedLoadContext : AssemblyLoadContext {
        // Isolate this ALC from outside assemblies, including system assemblies.

        public IsolatedLoadContext() : base(true) { }

        protected override Assembly? Load(AssemblyName assemblyName) {
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) {
            return IntPtr.Zero;
        }
    }

    public IDictionary<string, ICoreInitializer> Mods { get; } = new Dictionary<string, ICoreInitializer>();

    public int RunGame(string[] args) {
        var alc = new IsolatedLoadContext();
        alc.Resolving += (context, name) => {
            var assembly = Assembly.Load(name.Name);
            return assembly;
        };

        alc.Resolving += (context, name) => {
            var assembly = Assembly.Load(name.Name);
            return assembly;
        };

        foreach (var asm in AssemblyLoadContext.Default.Assemblies)
            alc.LoadFromAssemblyName(asm.GetName());

        return Main(args, alc);
    }

    private int Main(string[] args, AssemblyLoadContext alc) {
        var assembly = alc.LoadFromAssemblyName(typeof(Program).Assembly.GetName());
        var type = assembly.GetType("HCDN.Desktop.Program");
        var method = type!.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
        return (int?) method!.Invoke(null, new object[] { args }) ?? 1;
    }
}
