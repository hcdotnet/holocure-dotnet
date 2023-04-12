using NuGet.Protocol.Core.Types;

namespace HCDN.Desktop.NuGet; 

/// <summary>
///     A package (search) and its associated <see cref="SourceRepository"/>.
/// </summary>
/// <param name="Repository">The repository the package was found in.</param>
/// <param name="Metadata">The metadata of the package.</param>
public record PackageInfo(SourceRepository Repository, IPackageSearchMetadata Metadata);
