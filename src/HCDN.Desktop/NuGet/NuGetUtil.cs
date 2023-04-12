using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;

namespace HCDN.Desktop.NuGet;

/// <summary>
///     A collection of utility methods for working with NuGet.
/// </summary>
internal static class NuGetUtil {
    public const string STAGING_DIR = "staging";

    /// <summary>
    ///     Gets the latest version of the specified package.
    /// </summary>
    /// <param name="packageId">The package ID of the package.</param>
    /// <param name="logger">The logger to log with.</param>
    /// <returns>
    ///     The latest version of the specified package, or
    ///     <see langword="null"/> if no package was found.
    /// </returns>
    public static async Task<PackageInfo?> GetLatestPackageAsync(string packageId, ILog logger) {
        logger.Debug("Fetching the latest package information for package: " + packageId);

        var settings = Settings.LoadDefaultSettings(null);
        var sourceProvider = new PackageSourceProvider(settings);
        var sources = sourceProvider.LoadPackageSources().ToList();
        var coreV3 = Repository.Provider.GetCoreV3();
        var repositoryProvider = new SourceRepositoryProvider(sourceProvider, coreV3);

        logger.Debug("Found " + sources.Count + " package sources.");

        foreach (var source in sources) {
            if (!source.IsEnabled) {
                logger.Debug("Skipping disabled package source: " + source.Name);
                continue;
            }

            if (source is { IsHttp: false, IsHttps: false, IsLocal: false }) {
                logger.Debug("Skipping non-HTTP/HTTPS/Local package source: " + source.Name);
                continue;
            }

            logger.Debug("Found enabled package source: " + source.Name);

            var sourceRepository = repositoryProvider.CreateRepository(source);
            var metadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>();
            var metadata = (await metadataResource.GetMetadataAsync(
                packageId,
                true,
                false,
                new SourceCacheContext(),
                GetLogger(logger),
                new CancellationToken()
            )).ToArray();

            if (metadata.Length == 0) {
                logger.Debug("No metadata found for package: " + packageId);
                continue;
            }

            logger.Debug("Found " + metadata.Length + " metadata entries for package: " + packageId);
            var latestMetadata = metadata.OrderByDescending(m => m.Identity.Version).First();

            logger.Debug("Found latest version for package: " + packageId + " (" + latestMetadata.Identity.Version + ")");

            return new PackageInfo(sourceRepository, latestMetadata);
        }

        logger.Debug("No package sources found for package: " + packageId);
        return null;
    }

    /// <summary>
    ///     Downloads the specified package. The package will be downloaded to
    ///     the <see cref="STAGING_DIR"/> directory.
    /// </summary>
    /// <param name="packageInfo">The package to download.</param>
    /// <param name="logger">The logger to log with.</param>
    /// <returns>
    ///     The path to the downloaded package, or <see langword="null"/> if the
    ///     package could not be downloaded.
    /// </returns>
    public static async Task<PackageDownload?> DownloadPackageAsync(PackageInfo packageInfo, ILog logger) {
        var dir = Path.Combine(STAGING_DIR, packageInfo.Metadata.Identity.Id.ToLower());

        if (Directory.Exists(dir)) {
            logger.Debug("Deleting existing staging directory: " + dir);
            Directory.Delete(dir, true);
        }

        logger.Debug("Creating staging directory: " + dir);
        Directory.CreateDirectory(dir);

        var downloadResource = await packageInfo.Repository.GetResourceAsync<DownloadResource>();
        var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
            packageInfo.Metadata.Identity,
            new PackageDownloadContext(
                new SourceCacheContext(),
                dir,
                true
            ),
            SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null)),
            GetLogger(logger),
            new CancellationToken()
        );

        // TODO: Is AvailableWithoutStream acceptable?
        if (downloadResult.Status is DownloadResourceResultStatus.Available)
            return new PackageDownload(downloadResult, Path.GetFullPath(dir));

        logger.Debug("Package download failed: " + downloadResult.Status);
        return null;
    }

    /// <summary>
    ///     Extracts the specified package.
    /// </summary>
    /// <param name="packageDownload">The package download to extract.</param>
    /// <param name="logger">The logger to log with.</param>
    public static async Task ExtractPackageAsync(PackageDownload packageDownload, ILog logger) {
        await PackageExtractor.ExtractPackageAsync(
            packageDownload.DownloadResult.PackageSource,
            packageDownload.DownloadResult.PackageStream,
            new PackagePathResolver(packageDownload.Path),
            new PackageExtractionContext(
                PackageSaveMode.Defaultv3,
                XmlDocFileSaveMode.None,
                ClientPolicyContext.GetClientPolicy(
                    Settings.LoadDefaultSettings(null),
                    GetLogger(logger)
                ),
                GetLogger(logger)
            ),
            new CancellationToken()
        );
    }

    /// <summary>
    ///     Creates a new <see cref="ILogger"/> which uses the specified
    ///     <see cref="ILog"/> instance.
    /// </summary>
    /// <param name="logger">The <see cref="ILog"/> instance to use.</param>
    /// <returns>
    ///     A new <see cref="ILogger"/> which uses the specified
    ///     <see cref="ILog"/> instance.
    /// </returns>
    public static ILogger GetLogger(ILog logger) {
        return new Log4NetLogger(logger);
    }
}
