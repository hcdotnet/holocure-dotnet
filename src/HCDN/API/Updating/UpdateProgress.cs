using System.Text;

namespace HCDN.API.Updating;

/// <summary>
///     Describes the progress of an update.
/// </summary>
/// <param name="Title">The update step's title.</param>
/// <param name="Message">The update step's additional message.</param>
/// <param name="Progress">The current progress of this update step.</param>
/// <param name="Total">The total possible progress of this update step.</param>
public readonly record struct UpdateProgress(string? Title, string? Message, int Progress, int Total) {
    /// <summary>
    ///     Gets the percentage of progress.
    /// </summary>
    public float Percentage => (float)Progress / Total;

    public UpdateProgress(string? title, int progress, int total) : this(title, null, progress, total) { }

    /// <summary>
    ///     Gets a string representation of this update progress.
    /// </summary>
    /// <returns>
    ///     A string representation of this update progress.
    ///     <br />
    ///     The format will vary depending on the values of <see cref="Title"/>
    ///     and <see cref="Message"/>.
    ///     <br />
    ///     When neither are null, the format will be:
    ///     <c>
    ///         [Title]: [Message] - [Progress]/[Total]
    ///     </c>
    ///     <br />
    ///     When only <see cref="Title"/> is null, the format will be:
    ///     <c>
    ///         [Message] - [Progress]/[Total]
    ///     </c>
    ///     <br />
    ///     When only <see cref="Message"/> is null, the format will be:
    ///     <c>
    ///         [Title] - [Progress]/[Total]
    ///     </c>
    ///     <br />
    ///     When both are null, the format will be:
    ///     <c>
    ///         [Progress]/[Total]
    ///     </c>
    /// </returns>
    public override string ToString() {
        // Refer to summary to quickly understand how this string is formatted.
        var sb = new StringBuilder();

        if (Title != null)
            sb.Append(Title);

        if (Message != null) {
            if (sb.Length > 0)
                sb.Append(": ");

            sb.Append(Message);
        }

        if (sb.Length > 0) {
            sb.Append(" - ");
        }

        sb.Append(Progress);
        sb.Append('/');
        sb.Append(Total);

        return sb.ToString();
    }
}
