using JetBrains.Annotations;

namespace HCDN.API.Modding;

/// <summary>
///     A runtime mod initializer, which is ran when a mod is first initialized
///     during the main game launch.
/// </summary>
/// <seealso cref="ICoreInitializer"/>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IModInitializer { }
