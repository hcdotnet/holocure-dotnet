using HCDN.API;
using HCDN.API.Updating;
using HCDN.Rendering;

namespace HCDN.Desktop;

/// <summary>
///     The desktop implementation of <see cref="HoloCureGame"/>.
/// </summary>
/// <seealso cref="HoloCureGame"/>
internal sealed partial class DesktopGame : HoloCureGame {
    public DesktopGame(
        IModLoader modLoader,
        IAssetManager assetManager,
        IUpdater gameUpdater) : base(
        modLoader,
        assetManager,
        gameUpdater
    ) { }
}
