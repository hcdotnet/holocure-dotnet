using System.Collections.Generic;

namespace HCDN.API.Modding; 

/// <summary>
///     Handles the loading of mods at runtime.
/// </summary>
public interface IModLoader<TInitializer> {
    /// <summary>
    ///     Mods, by name.
    /// </summary>
    IDictionary<string, TInitializer> Mods { get; }
}
