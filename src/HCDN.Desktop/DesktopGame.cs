using HCDN.API.Modding;
using HCDN.API.Updating;
using HCDN.Graphics;

namespace HCDN.Desktop;

/// <summary>
///     The desktop implementation of <see cref="HoloCureGame"/>.
/// </summary>
/// <seealso cref="HoloCureGame"/>
internal sealed partial class DesktopGame : HoloCureGame {
    public DesktopGame(
        IModLoader<IModInitializer> modLoader,
        IAssetManager assetManager,
        IUpdater gameUpdater
    ) : base(
        modLoader,
        assetManager,
        gameUpdater
    ) { }
}
