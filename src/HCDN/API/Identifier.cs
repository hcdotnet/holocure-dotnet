using System;
using HCDN.Exceptions;

namespace HCDN.API;

/// <summary>
///     A simple string-based identifier, which has a <see cref="Namespace"/>
///     and regular content <see cref="Name"/>.
/// </summary>
/// <param name="Namespace">
///     The namespace of the mod that owns this identifier; the mod's name.
/// </param>
/// <param name="Name"></param>
/// <remarks>
///     Both the <see cref="Namespace"/> and <see cref="Name"/> should be in
///     alphanumeric <c>snake_case</c>.
/// </remarks>
public readonly record struct Identifier(string Namespace, string Name) {
    /// <summary>
    ///     A null identifier, the default instance.
    /// </summary>
    public static readonly Identifier NULL = new();
    
    public const string HCDN = "hcdn";

    /// <summary>
    ///     Returns the string representation of this identifier.
    /// </summary>
    /// <returns>"<see cref="Namespace"/><c>:</c><see cref="Name"/>"</returns>
    public override string ToString() {
        return $"{Namespace}:{Name}";
    }

    /// <summary>
    ///     Parses an identifier from a string.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>The parsed identifier.</returns>
    /// <exception cref="FormatException">The parse was unsuccessful.</exception>
    public static Identifier Parse(string value) {
        if (!TryParse(value, out var result))
            throw new IdentifierFormatException(value);

        return result;
    }

    /// <summary>
    ///     Safely parses an identifier from a string.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The resulting identifier instance.</param>
    /// <returns>Whether the parse was successful.</returns>
    public static bool TryParse(string value, out Identifier result) {
        var parts = value.Split(':', 2);

        if (parts.Length != 2) {
            result = NULL;
            return false;
        }

        result = new Identifier(parts[0], parts[1]);
        return true;
    }

    public static implicit operator string(Identifier value) => value.ToString();
}
