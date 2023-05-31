using JetBrains.Annotations;

namespace HCDN.API.Modding;

/// <summary>
///     A core mod initializer, which is ran by the core mod game process that
///     launches the main game.
/// </summary>
/// <seealso cref="IModInitializer"/>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface ICoreInitializer { }
