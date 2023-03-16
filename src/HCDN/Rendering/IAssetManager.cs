using System.Collections.Generic;
using HCDN.API;

namespace HCDN.Rendering;

/// <summary>
///     Represents an asset manager, which handles the loading and unloading of
///     assets.
/// </summary>
public interface IAssetManager {
    /// <summary>
    ///     Registers a new asset loader for this manager.
    /// </summary>
    /// <param name="loader">The loader to register.</param>
    void RegisterLoader(IAssetLoader loader);

    /// <summary>
    ///     Unregisters an asset loader from this manager.
    /// </summary>
    /// <param name="identity">The identity of the loader to unregister.</param>
    void UnregisterLoader(Identifier identity);

    /// <summary>
    ///     Attempts to retrieve a registered loader for this manager.
    /// </summary>
    /// <param name="identity">The identity of the loader.</param>
    /// <param name="loader">The resolved loader, if applicable.</param>
    /// <returns>
    ///     <see langword="true"/> if the loader was found, otherwise
    ///     <see langword="false"/>.
    /// </returns>
    bool TryGetLoader(Identifier identity, out IAssetLoader? loader);

    /// <summary>
    ///     Attempts to retrieve an asset from this manager, sourced from the
    ///     provided loaders.
    /// </summary>
    /// <param name="identity">The asset's identity.</param>
    /// <param name="asset">
    ///     The resolved asset, which will be <see cref="NullAsset{T}"/> if one
    ///     is not found.
    /// </param>
    /// <typeparam name="T">The asset type.</typeparam>
    /// <returns>
    ///     <see langword="true"/> if the asset was found, otherwise
    ///     <see langword="false"/>. Whether the asset was found depends on the
    ///     return value of
    ///     <see cref="Extensions.Extensions.IsNull{T}(IAsset{T})"/>.
    /// </returns>
    bool TryGetAsset<T>(Identifier identity, out IAsset<T> asset) where T : class;

    /// <summary>
    ///     Retrieves an asset from this manager, sourced from the provided
    ///     loaders.
    /// </summary>
    /// <param name="identity">The asset's identity.</param>
    /// <typeparam name="T">The asset type.</typeparam>
    /// <returns>
    ///     The resolved asset, which will be <see cref="NullAsset{T}"/> if one
    ///     is not found.
    /// </returns>
    IAsset<T> GetAsset<T>(Identifier identity) where T : class;
    
    /// <summary>
    ///     Retrieves the value of an asset from this manager, sourced from the
    ///     provided loaders, and not wrapped in an <see cref="IAsset{T}"/>.
    /// </summary>
    /// <param name="identity">The asset's identity.</param>
    /// <typeparam name="T">The asset type.</typeparam>
    /// <returns>
    ///     The resolved asset value, which will be <see langword="null"/> if
    ///     one is not found.
    /// </returns>
    T? GetAssetValue<T>(Identifier identity) where T : class;

    /// <summary>
    ///     Invalidates the given assets, causing any instances to be reloaded
    ///     by this manager if they have been requested.
    /// </summary>
    /// <param name="identities">The identities to invalidate.</param>
    void InvalidateAssets(IEnumerable<Identifier> identities);
}
