using System;
using HCDN.API;

namespace HCDN.Rendering;

public class Asset<T> : IAsset<T> where T : class {
    public Identifier Identity { get; }

    private T? value;
    
    public T? Value => GetValue();

    public bool Invalidated { get; protected set; }

    public IAssetManager? Manager { get; set; }
    
    public Asset(Identifier identity) {
        Identity = identity;
        Invalidated = true;
    }
    
    public Asset(Identifier identity, T? value) {
        Identity = identity;
        this.value = value;
    }

    private T? GetValue() {
        if (!Invalidated)
            return value;

        // TODO: Throw here or something?
        if (Manager is null)
            return value;

        return value = Manager.GetAssetValue<T>(Identity);
    }

    void IAsset.Invalidate() {
        if (Manager is not null)
            Invalidated = true;
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing && Value is IDisposable disposable)
            disposable.Dispose();
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
