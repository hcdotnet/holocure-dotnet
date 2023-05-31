using System;

namespace HCDN.API.Modding;

// TODO: We'll just use the assembly version for the mod version; they should
// always be the same.

/// <summary>
///     Provides vital information about a mod, attached to a single assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
public sealed class ModAttribute : Attribute {
    /// <summary>
    ///     The mod's identifier; a namespace in <see cref="Identifier"/>.
    /// </summary>
    public string ModId { get; set; }

    /// <summary>
    ///     Creates a new <see cref="ModAttribute"/> with the given mod ID.
    /// </summary>
    /// <param name="modId">The mod ID.</param>
    public ModAttribute(string modId) {
        ModId = modId;
    }
}
