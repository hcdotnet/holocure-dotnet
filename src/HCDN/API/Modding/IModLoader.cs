using System.Collections.Generic;
using HCDN.API.Modding;

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
