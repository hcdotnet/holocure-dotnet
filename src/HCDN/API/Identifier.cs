using System;

namespace HCDN.API;

public readonly record struct Identifier(string Namespace, string Name) {
    public static readonly Identifier NULL = new();
    
    public override string ToString() => $"{Namespace}:{Name}";

    public static Identifier Parse(string value) {
        if (!TryParse(value, out var result)) {
            throw new FormatException("Invalid identifier format.");
        }

        return result;
    }

    public static bool TryParse(string value, out Identifier result) {
        var parts = value.Split(':', 2);

        if (parts.Length != 2) {
            result = default;
            return false;
        }

        result = new Identifier(parts[0], parts[1]);
        return true;
    }

    public static implicit operator string(Identifier value) => value.ToString();
}
