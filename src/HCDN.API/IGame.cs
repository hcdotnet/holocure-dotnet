namespace HCDN.API;

/// <summary>
///     The main game interface, which manages core data for this game instance.
/// </summary>
public interface IGame {
    /// <summary>
    ///     The <see cref="IModLoader"/> instance for this game.
    /// </summary>
    IModLoader ModLoader { get; }
}
