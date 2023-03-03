using HCDN.API;

namespace HCDN.Desktop;

/// <summary>
///     The desktop implementation of <see cref="HoloCureGame"/>.
/// </summary>
/// <seealso cref="HoloCureGame"/>
public sealed class DesktopGame : HoloCureGame {
    public DesktopGame(IModLoader modLoader) : base(modLoader) { }
}
