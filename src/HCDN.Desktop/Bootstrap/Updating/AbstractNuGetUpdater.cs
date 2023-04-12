using System;
using System.Threading.Tasks;
using HCDN.API.Updating;
using HCDN.Desktop.NuGet;
using HCDN.Extensions;
using log4net;
using NuGet.Versioning;

namespace HCDN.Desktop.Bootstrap.Updating;

/// <summary>
///     An abstract <see cref="IUpdater"/> implementation which uses NuGet.
/// </summary>
public abstract class AbstractNuGetUpdater : IUpdater {
    /// <summary>
    ///     The ID of the package to update.
    /// </summary>
    public string PackageId { get; }

    /// <summary>
    ///     The current version of the package.
    /// </summary>
    public NuGetVersion CurrentVersion { get; }

    private readonly ILog logger;

    private bool hasUpdate;
    private bool hasCheckedForUpdate;
    private bool hasDownloadedUpdate;

    protected PackageInfo? PkgInfo { get; private set; }

    protected PackageDownload? PkgDownload { get; private set; }

    protected AbstractNuGetUpdater(string packageId, NuGetVersion currentVersion) {
        PackageId = packageId;
        CurrentVersion = currentVersion;

        logger = LogManager.GetLogger(GetType());
    }

    public async Task<bool> HasUpdateAsync(IUpdateReporter reporter) {
        var progress = new UpdateProgress("Checking for update...", 0, 1);
        reporter.Report(progress);

        if (hasCheckedForUpdate) {
            reporter.Report(progress.WithMessage("Update found (already checked)!'").WithProgress(1, 1));
            return hasUpdate;
        }

        hasCheckedForUpdate = true;

        PkgInfo = await NuGetUtil.GetLatestPackageAsync(PackageId, logger);

        if (PkgInfo is null) {
            logger.Warn("No package information found for package " + PackageId);
            reporter.Report(progress.WithMessage("No update found (package could not be found)!").WithProgress(1, 1));
            return false;
        }

        var latestVersion = PkgInfo.Metadata.Identity.Version;
        hasUpdate = latestVersion > CurrentVersion;

        if (hasUpdate) {
            logger.Info("Found update for package " + PackageId + ": " + latestVersion);
            reporter.Report(progress.WithMessage("Update found!").WithProgress(1, 1));
            return true;
        }

        logger.Info("No update available for package " + PackageId);
        reporter.Report(progress.WithMessage("No update found!").WithProgress(1, 1));
        return false;
    }

    public async Task DownloadUpdateAsync(IUpdateReporter reporter) {
        if (!hasCheckedForUpdate)
            throw new InvalidOperationException("Must check for update before downloading it.");

        if (!hasUpdate)
            throw new InvalidOperationException("No update available.");

        if (PkgInfo is null)
            throw new InvalidOperationException("Package information is null (this should never happen).");
        
        var progress = new UpdateProgress("Downloading update...", "Downloading NuGet package...", 0, 3);
        reporter.Report(progress);
        
        if (hasDownloadedUpdate) {
            reporter.Report(progress.WithMessage("Update downloaded (already downloaded)!").WithProgress(2, 3));
            return;
        }
        
        hasDownloadedUpdate = true;

        PkgDownload = await NuGetUtil.DownloadPackageAsync(PkgInfo, logger);
        reporter.Report(progress.WithMessage("Checking download...").WithProgress(1, 3));

        if (PkgDownload is null) {
            reporter.Report(progress.WithMessage("Cannot extract NuGet package, failed to download!").WithProgress(3, 3));
            return;
        }
        
        reporter.Report(progress.WithMessage("Extracting NuGet package...").WithProgress(2, 3));

        await NuGetUtil.ExtractPackageAsync(PkgDownload, logger);
        reporter.Report(progress.WithMessage("Update downloaded and extracted!").WithProgress(3, 3));
    }

    public async Task InstallUpdateAsync(IUpdateReporter reporter) {
        if (!hasCheckedForUpdate)
            throw new InvalidOperationException("Must check for update before installing it.");

        if (!hasUpdate)
            throw new InvalidOperationException("No update available.");

        if (!hasDownloadedUpdate)
            throw new InvalidOperationException("Must download update before installing it.");
        
        if (PkgDownload is null)
            throw new InvalidOperationException("Package download is null (this should never happen).");

        await InnerInstallUpdateAsync(reporter);
    }
    
    protected abstract Task InnerInstallUpdateAsync(IUpdateReporter reporter);
}
