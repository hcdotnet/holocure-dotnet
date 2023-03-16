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

    public Identifier Identity => Identifier.NULL;

    public T? Value { get; set; } = null;

    bool IAsset.Invalidated => false;

    public IAssetManager? Manager { get; set; }

    void IAsset.Invalidate() {
    }

    void IDisposable.Dispose() { }
}
