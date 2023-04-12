using System.Threading.Tasks;

namespace HCDN.API.Updating; 

/// <summary>
///     Describes an object which may check for updates for some arbitrary
///     resource.
/// </summary>
public interface IUpdater {
    /// <summary>
    ///     Checks for an update asynchronously.
    /// </summary>
    /// <param name="reporter">
    ///     An <see cref="IUpdateReporter"/> which will be used to report the
    ///     progress of the update check.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> which will complete with a boolean
    ///     indicating whether an update is available.
    /// </returns>
    Task<bool> HasUpdateAsync(IUpdateReporter reporter);
    
    /// <summary>
    ///     Downloads the update asynchronously.
    /// </summary>
    /// <param name="reporter">
    ///     An <see cref="IUpdateReporter"/> which will be used to report the
    ///     progress downloading the update.
    /// </param>
    /// <returns>
    ///    A <see cref="Task"/> which will complete when the update has been
    ///   downloaded.
    /// </returns>
    Task DownloadUpdateAsync(IUpdateReporter reporter);
    
    /// <summary>
    ///     Installs the update asynchronously.
    /// </summary>
    /// <param name="reporter">
    ///     An <see cref="IUpdateReporter"/> which will be used to report the
    ///     progress of the update installation.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> which will complete when the update has been
    ///     installed.
    /// </returns>
    /// <remarks>
    ///     This method should be called after <see cref="DownloadUpdateAsync"/>.
    /// </remarks>
    Task InstallUpdateAsync(IUpdateReporter reporter);
}
