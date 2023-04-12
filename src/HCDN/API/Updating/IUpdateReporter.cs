namespace HCDN.API.Updating; 

/// <summary>
///     Reports the progress of an update.
/// </summary>
public interface IUpdateReporter {
    /// <summary>
    ///     Reports the progress of an update.
    /// </summary>
    /// <param name="progress">The progress update.</param>
    void Report(UpdateProgress progress);
}
