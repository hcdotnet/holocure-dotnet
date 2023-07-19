using System;
using System.Linq;
using HCDN.Desktop.Bootstrap;
using HCDN.Desktop.Bootstrap.Modding;
using HCDN.Desktop.Bootstrap.Updating;
using HCDN.Desktop.Exceptions;
using HCDN.Graphics;
using HCDN.Logging;

namespace HCDN.Desktop.Launch;

internal static class GameLauncher {
    public static int Launch(LaunchType launchType, string[] args) {
        return CommonGameLaunch(args, launchType);
    }

    private static int CommonGameLaunch(string[] args, LaunchType launchType) {
        var logName = "desktop" + GetLaunchTypeLogName(launchType);
        LogInitializer.Initialize("HoloCure.NET Desktop", logName);

        return launchType switch {
            LaunchType.UpdateDaemon => UpdateDaemonLaunch(args),
            LaunchType.CoreModsEnabled => CoreModGameLaunch(args),
            LaunchType.CoreModsDisabledImplicitly => RegularGameLaunch(args, withCoreMods: true),
            LaunchType.CoreModsDisabledExplicitly => RegularGameLaunch(args, withCoreMods: false),
            _ => throw new InvalidLaunchTypeException(launchType),
        };
    }

    private static string GetLaunchTypeLogName(LaunchType launchType) {
        return launchType switch {
            LaunchType.UpdateDaemon => "-update-daemon",
            LaunchType.CoreModsEnabled => "-main",
            LaunchType.CoreModsDisabledImplicitly => "-child",
            LaunchType.CoreModsDisabledExplicitly => "",
            _ => throw new InvalidLaunchTypeException(launchType),
        };
    }

    private static int UpdateDaemonLaunch(string[] args) {
        var logger = LogInitializer.FromType(typeof(GameLauncher));
        logger.Info("Launching as update daemon...");
        LogStartupInfo(logger, args);
        return Updater.RunFromStaging(args.ToList()) ? 0 : 1;
    }

    private static int CoreModGameLaunch(string[] args) {
        var logger = LogInitializer.FromType(typeof(GameLauncher));
        logger.Info("Launching core-mod parent process...");
        LogStartupInfo(logger, args);

        FnaBootstrapper.Bootstrap();

        var coreModLoader = new CoreModLoader();
        return coreModLoader.RunGame(args.Append("--core-mods-loaded").ToArray());
    }

    private static int RegularGameLaunch(string[] args, bool withCoreMods) {
        var logger = LogInitializer.FromType(typeof(GameLauncher));
        logger.Info(withCoreMods ? "Launching core-mod child process..." : "Launching without core-mods...");
        LogStartupInfo(logger, args);

        FnaBootstrapper.Bootstrap();

        logger.Info("Starting game!");
        logger.Debug($"Initializing {nameof(DesktopGame)} instance...");
        using var game = new DesktopGame(
            new DesktopModLoader(),
            new AssetManager(),
            Updater.MakeGameUpdater()
        );

        logger.Debug($"Running {nameof(DesktopGame)} instance...");
        game.Run();
        return 0;
    }

    private static void LogStartupInfo(Logger logger, string[] args) {
        if (args.Length > 0) {
            logger.Debug("Started process with launch arguments:");
            foreach (var arg in args)
                logger.Debug($"  {arg}");
        }
        else {
            logger.Debug("Started process without launch arguments.");
        }

        logger.Debug("Running in CWD: " + Environment.CurrentDirectory);
        logger.Debug("AppDomain directory: " + AppDomain.CurrentDomain.BaseDirectory);
    }
}
