using HCDN.API.Modding;

namespace HCDN.API;

/// <summary>
///     The main game interface, which manages core data for this game instance.
/// </summary>
public interface IGame {
    /// <summary>
    ///     The <see cref="IModLoader{TInitializer}"/> instance for this game.
    /// </summary>
    IModLoader<IModInitializer> ModLoader { get; }
}
