using HCDN.API;
using HCDN.Rendering;
using Microsoft.Xna.Framework;

namespace HCDN;

/// <summary>
///     The main <see cref="Game"/> implementation, which manages core data for
///     this game instance.
/// </summary>
public abstract class HoloCureGame : Game,
                                     IGame {
    /// <summary>
    ///     The <see cref="IModLoader"/> instance for this game.
    /// </summary>
    public IModLoader ModLoader { get; }

    /// <summary>
    ///     The <see cref="IAssetManager"/> instance for this game.
    /// </summary>
    private IAssetManager AssetManager { get; }

    /// <summary>
    ///     Initializes a new instance of <see cref="HoloCureGame"/> with all
    ///     required external dependencies.
    /// </summary>
    /// <param name="modLoader">
    ///     The <see cref="IModLoader"/> to use for loading mods.
    /// </param>
    protected HoloCureGame(IModLoader modLoader) {
        ModLoader = modLoader;
        AssetManager = new AssetManager();
    }
}
