using NuGet.Protocol.Core.Types;

namespace HCDN.Desktop.NuGet;

/// <summary>
///     A package download result.
/// </summary>
/// <param name="DownloadResult">The result of the download operation.</param>
/// <param name="Path">The path to the downloaded package.</param>
public record PackageDownload(DownloadResourceResult DownloadResult, string Path);
