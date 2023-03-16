using HCDN.API;
using HCDN.Rendering;

namespace HCDN.Extensions;

public static partial class Extensions {
    /// <summary>
    ///     Checks if an asset is <see langword="null"/> based on several
    ///     factors:
    ///     <br />
    ///     <ul>
    ///         <li>
    ///             is the <paramref name="asset"/> instance
    ///             <see langword="null"/>?
    ///         </li>
    ///         <li>
    ///             is <see cref="IAsset{T}.Value"/> <see langword="null"/>?    
    ///         </li>
    ///         <li>
    ///             is the <paramref name="asset"/> an instance of
    ///             <see cref="NullAsset{T}"/>?
    ///         </li>
    ///         <li>
    ///             is <see cref="IAsset.Identity"/> equal to
    ///             <see cref="Identifier.NULL"/>?
    ///         </li>
    ///     </ul>
    /// </summary>
    /// <param name="asset">The asset to check.</param>
    /// <typeparam name="T">The asset's type.</typeparam>
    /// <returns>
    ///     <see langword="true"/> if the asset is considered
    ///     <see langword="null"/>, otherwise <see langword="false"/>.
    /// </returns>
    public static bool IsNull<T>(this IAsset<T>? asset) where T : class {
        if (asset?.Value is null)
            return true;

        if (asset is NullAsset<T>)
            return true;

        return asset.Identity == Identifier.NULL;
    }
}
