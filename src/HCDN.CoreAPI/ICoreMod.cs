using JetBrains.Annotations;
using Mono.Cecil;

namespace HCDN.CoreAPI;

/// <summary>
///     Describes a core mod - that is, a mod which performs first-pass edits to
///     assemblies as they're loaded.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface ICoreMod {
    /// <summary>
    ///     Exposes access to <see cref="AssemblyDefinition"/>s of loaded
    ///     assemblies in order to modify them.
    /// </summary>
    /// <param name="assembly">
    ///     The assembly to edit/transform/what have you.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this core mod applied any edits,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Proper return values are important as it is what is used to keep
    ///     track of which core mod makes edits to which assemblies. Relevant
    ///     for error tracking and the like.
    /// </remarks>
    bool ModifyAssembly(AssemblyDefinition assembly);
}
