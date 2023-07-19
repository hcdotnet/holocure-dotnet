using System;
using System.Linq;
using HCDN.Desktop.Launch;

namespace HCDN.Desktop;

internal static class Program {
    [STAThread]
    public static int Main(string[] args) {
        var launchType = DetermineLaunchType(args);
        return GameLauncher.Launch(launchType, args);
    }

    private static LaunchType DetermineLaunchType(string[] args) {
        // If "--staging" is passed, the update daemon is running.
        if (args.Contains("--staging"))
            return LaunchType.UpdateDaemon;

        // Explicitly don't load coremods if the user has them disabled.
        if (args.Contains("--core-mods-disabled"))
            return LaunchType.CoreModsDisabledExplicitly;

        // This is passed by the main game process when re-launching with
        // core-mods.
        if (args.Contains("--core-mods-loaded"))
            return LaunchType.CoreModsDisabledImplicitly;

        // Otherwise just assume we want to launch with core-mods...
        return LaunchType.CoreModsEnabled;
    }
}
