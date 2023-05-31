using System.Threading.Tasks;
using HCDN.API;
using HCDN.API.Modding;
using HCDN.API.Updating;
using HCDN.Graphics;
using Microsoft.Xna.Framework;

namespace HCDN;

/// <summary>
///     The main <see cref="Game"/> implementation, which manages core data for
///     this game instance.
/// </summary>
public abstract partial class HoloCureGame : Game,
                                             IGame {
    /// <summary>
    ///     The <see cref="IModLoader"/> instance for this game.
    /// </summary>
    public IModLoader ModLoader { get; }

    /// <summary>
    ///     The <see cref="IAssetManager"/> instance for this game.
    /// </summary>
    public IAssetManager AssetManager { get; }

    /// <summary>
    ///     The <see cref="IUpdater"/> instance for updating this game.
    /// </summary>
    public IUpdater GameUpdater { get; }

    /// <summary>
    ///     Initializes a new instance of <see cref="HoloCureGame"/> with all
    ///     required external dependencies.
    /// </summary>
    /// <param name="modLoader">
    ///     The <see cref="IModLoader"/> to use for loading mods.
    /// </param>
    /// <param name="assetManager">
    ///     The <see cref="IAssetManager"/> to use for loading assets.
    /// </param>
    /// <param name="gameUpdater">
    ///     The <see cref="IUpdater"/> to use for updating this game.
    /// </param>
    protected HoloCureGame(IModLoader modLoader, IAssetManager assetManager, IUpdater gameUpdater) {
        ModLoader = modLoader;
        AssetManager = assetManager;
        GameUpdater = gameUpdater;
    }

    protected override void Initialize() {
        base.Initialize();

        CheckForGameUpdates();
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);

        ApplyGameUpdate();
    }
}
