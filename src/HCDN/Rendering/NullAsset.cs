using System;
using HCDN.API;

namespace HCDN.Rendering; 

/// <summary>
///     The fallback value for an asset that is not found by an
///     <see cref="IAssetLoader"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class NullAsset<T> : IAsset<T> where T : class {
    public static readonly NullAsset<T> INSTANCE = new();

    Identifier IAsset.Identity => Identifier.NULL;

    T? IAsset<T>.Value => null;

    bool IAsset.Invalidated => false;

    IAssetManager? IAsset.Manager { get; set; }

    void IAsset.Invalidate() {
    }

    void IDisposable.Dispose() { }
}
