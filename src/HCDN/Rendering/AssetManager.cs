using System;
using System.Collections.Generic;
using System.Linq;
using HCDN.API;
using HCDN.Extensions;

namespace HCDN.Rendering;

/// <summary>
///     The default implementation of <see cref="IAssetManager"/>.
/// </summary>
public class AssetManager : IAssetManager {
    protected readonly Dictionary<Identifier, WeakReference<IAsset>> Assets = new();
    protected readonly Dictionary<Identifier, IAssetLoader> Loaders = new();

    public virtual void RegisterLoader(IAssetLoader loader) {
        // TODO: Do we want to say anything if the loader is already registered?
        if (Loaders.ContainsKey(loader.Identity))
            UnregisterLoader(loader.Identity);

        // We may eventually want to handle adding and removing items with more
        // care, since this order determines the order in which loaders are
        // searched for assets...
        Loaders.Add(loader.Identity, loader);
    }

    public virtual void UnregisterLoader(Identifier identity) {
        // Safe to just call Remove without checking if the identity exists in
        // the dictionary.
        Loaders.Remove(identity);
    }

    public virtual bool TryGetLoader(Identifier identity, out IAssetLoader? loader) {
        return Loaders.TryGetValue(identity, out loader);
    }

    public virtual bool TryGetAsset<T>(Identifier identity, out IAsset<T> asset) where T : class {
        // Check the Assets dictionary, which serves as both a cache and a
        // general record of all assets served by this manager (relevant to
        // asset invalidation).
        if (
            Assets.TryGetValue(identity, out var weakAsset)
         && weakAsset.TryGetTarget(out var target)
         && target is IAsset<T> tTarget
         && !tTarget.IsNull()
        ) {
            asset = tTarget;
            return true;
        }

        var assetVal = GetAssetValue<T>(identity);

        if (assetVal is not null) {
            asset = new Asset<T>(identity, assetVal) {
                Manager = this,
            };
            return true;
        }

        asset = NullAsset<T>.INSTANCE;
        return false;
    }

    public virtual IAsset<T> GetAsset<T>(Identifier identity) where T : class {
        TryGetAsset(identity, out IAsset<T> asset);
        return asset;
    }

    public virtual T? GetAssetValue<T>(Identifier identity) where T : class {
        foreach (var loader in Loaders.Values.Reverse()) {
            var lAsset = loader.LoadAsset<T>(identity);
            if (lAsset is null)
                continue;

            return lAsset;
        }

        return null;
    }

    public virtual void InvalidateAssets(IEnumerable<Identifier> identities) {
        foreach (var identity in identities)
            if (Assets.TryGetValue(identity, out var asset) && asset.TryGetTarget(out var target))
                target.Invalidate();
    }
}
