using System;
using System.Text;

namespace HCDN.Exceptions;

/// <summary>
///     Thrown when an identifier is in an invalid format.
/// </summary>
public class IdentifierFormatException : FormatException {
    public IdentifierFormatException(string identifier) : base(MakeMessage(identifier)) { }

    private static string MakeMessage(string identifier) {
        var sb = new StringBuilder();
        sb.AppendLine($"The identifier \"{identifier}\" is in an invalid format and could not be parsed correctly.");

        // Attempt to diagnose the issue.
        if (string.IsNullOrWhiteSpace(identifier))
            sb.AppendLine("The identifier is null, empty, or whitespace.");
        else if (!identifier.Contains(':'))
            sb.AppendLine("The identifier does not contain a ':' separating the namespace and name.");
        else
            sb.AppendLine("The issue could not be easily diagnosed.");

        return sb.ToString();
    }
}
