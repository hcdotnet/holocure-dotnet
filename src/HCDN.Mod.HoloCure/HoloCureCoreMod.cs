using HCDN.API;
using HCDN.CoreAPI;
using Mono.Cecil;

namespace HCDN.Mod.HoloCure; 

/// <summary>
///     An implementation of <see cref="ICoreMod"/>. These types are initialized
///     in a separate context from <see cref="IMod"/> types and the rest of the
///     assembly. Be careful while using them!
/// </summary>
public sealed class HoloCureCoreMod : ICoreMod {
    public bool ModifyAssembly(AssemblyDefinition assembly) {
        return false;
    }
}
