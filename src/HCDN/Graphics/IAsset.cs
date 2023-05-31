using System;
using HCDN.API;

namespace HCDN.Graphics;

/// <summary>
///     Represents an asset handled by an <see cref="IAssetLoader"/> and an
///     <see cref="IAssetManager"/>.
/// </summary>
public interface IAsset : IDisposable {
    /// <summary>
    ///     The asset's identity.
    /// </summary>
    Identifier Identity { get; }

    /// <summary>
    ///     The asset's value.
    /// </summary>
    object? Value { get; }

    /// <summary>
    ///     Whether this asset has been invalidated.
    /// </summary>
    bool Invalidated { get; }

    /// <summary>
    ///     The asset manager which this asset was loaded from, if applicable.
    /// </summary>
    IAssetManager? Manager { get; set; }

    /// <summary>
    ///     Invalidates this asset, causing it to be reloaded from its manager.
    ///     If no manager is present, this does not do much.
    /// </summary>
    void Invalidate();
}

/// <summary>
///     Represents an asset handled by an <see cref="IAssetLoader"/> and an
///     <see cref="IAssetManager"/>.
/// </summary>
/// <typeparam name="T">The asset's type.</typeparam>
public interface IAsset<out T> : IAsset where T : class {
    /// <summary>
    ///     The asset's value.
    /// </summary>
    new T? Value { get; }

    object? IAsset.Value => Value;
    // set => Value = value as T;
}
