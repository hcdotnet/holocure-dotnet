using HCDN.API;

namespace HCDN.Rendering;

/// <summary>
///     Handles the loading of an asset per the request of an
///     <see cref="IAssetManager"/>.
/// </summary>
public interface IAssetLoader {
    /// <summary>
    ///     A unique identifier for this loader.
    /// </summary>
    Identifier Identity { get; }

    /// <summary>
    ///     Attempts to load an asset of the given type.
    /// </summary>
    /// <param name="identity">
    ///     The identity by which this asset is identified.
    /// </param>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///     An instance if found, otherwise <see langword="null"/>.
    /// </returns>
    T? LoadAsset<T>(Identifier identity) where T : class;
}
