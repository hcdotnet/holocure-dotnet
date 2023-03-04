using System;
using System.Linq;
using HCDN.Desktop.Bootstrap;
using HCDN.Desktop.Bootstrap.Modding;
using log4net;

namespace HCDN.Desktop;

internal static class Program {
    [STAThread]
    internal static void Main(string[] args) {
        // Step 0: Run the post-download update process first.
        if (Updater.RunFromStaging(args.ToList()))
            return;
        
        // Step 1: Initialize logging, this is done before anything else. Also
        // get some basic logging done before anything else just because.
        LogBootstrapper.Bootstrap();
        var logger = LogManager.GetLogger(typeof(Program));
        LogStartupInfo(logger, args);
        
        // Step 2: Bootstrap mod loading. Core-mod support depends on this step,
        // as well as regular runtime mod loading, so this comes first. This
        // step handles some important things such as discovering, unpacking,
        // and deleting .nupkg files (present in first-launch releases and from
        // downloading packages in-game).
        ModBootstrapper.Bootstrap();
        
        // Step 3: Load core-mods. This is the first thing we do after logging.
        // It is by far the most important and complex step in the loading
        // process.
        CoreModLoader.LoadCoreMods();
        
        // Step 4: Bootstrap FNA. This is the first important thing we do after
        // loading core-mods.
        FnaBootstrapper.Bootstrap();
        
        // Step 5: Check for updates to the desktop client. This comes after
        // bootstrapping FNA since we use SDL message boxes. Returns true if the
        // user accepts an update, in which case we want to exit successfully.
        if (Updater.CheckForAndPromptUpdate())
            return;
        
        // Step 6: After loading core-mods and bootstrapping FNA, load the game.
        RunGame(logger);
    }

    private static void RunGame(ILog logger) {
        logger.Info("Starting game!");
        logger.Debug($"Initializing {nameof(DesktopGame)} instance...");
        using var game = new DesktopGame(new DesktopModLoader());

        logger.Debug($"Running {nameof(DesktopGame)} instance...");
        game.Run();
    }

    private static void LogStartupInfo(ILog logger, string[] args) {
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
