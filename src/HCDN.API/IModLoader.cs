using System.Collections.Generic;

namespace HCDN.API; 

/// <summary>
///     Handles the loading of mods at runtime.
/// </summary>
public interface IModLoader {
    /// <summary>
    ///     Mods, by name.
    /// </summary>
    IDictionary<string, IMod> Mods { get; }
}
