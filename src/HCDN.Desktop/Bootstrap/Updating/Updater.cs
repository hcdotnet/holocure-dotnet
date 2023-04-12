using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using HCDN.API.Updating;
using log4net;
using NuGet.Versioning;
using static SDL2.SDL;

namespace HCDN.Desktop.Bootstrap.Updating;

internal readonly record struct AssemblyInformationData(string PackageId, string PackageVersion) {
    public static AssemblyInformationData FromAssembly() {
        var assembly = typeof(Updater).Assembly;
        var info = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (info is null)
            throw new Exception("AssemblyInformationalVersionAttribute not found on assembly.");

        var infoParts = info.InformationalVersion.Split('|');
        if (infoParts.Length != 2)
            throw new Exception("AssemblyInformationalVersionAttribute unexpected length: " + infoParts.Length);

        var packageId = infoParts[0];
        var packageVersion = infoParts[1];

        return new AssemblyInformationData(packageId, packageVersion);
    }
}

internal static class Updater {
    private static readonly string[] dirs_to_delete = { "lib64", "osx", "vulkan", "x64", "x86", "runtimes", };

    public static bool RunFromStaging(List<string> args) {
        var index = args.IndexOf("--staging");
        if (index == -1)
            return false;

        if (args.Count <= index + 1)
            throw new Exception("Expected path to install directory after --staging.");

        // Sleep for a second to give the original process time to close.
        Thread.Sleep(1000);

        var installDir = args[index + 1];
        var stagingDir = AppDomain.CurrentDomain.BaseDirectory;
        if (!stagingDir.EndsWith(Path.DirectorySeparatorChar))
            stagingDir += Path.DirectorySeparatorChar;

        // Delete all files in the install directory.
        foreach (var file in Directory.EnumerateFiles(installDir))
            File.Delete(file);

        // Delete all directories in the install directory that should be
        // deleted.
        foreach (var dir in dirs_to_delete.Select(x => Path.Combine(installDir, x)))
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);

        // Copy staging directory contents to install directory, preserving
        // directory structure.
        foreach (var file in Directory.EnumerateFiles(stagingDir, "*", SearchOption.AllDirectories)) {
            var relativePath = file[stagingDir.Length..];
            var destPath = Path.Combine(installDir, relativePath);
            var destDir = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir!);

            File.Copy(file, destPath);
        }

        return true;
    }

    internal static IUpdater MakeGameUpdater() {
        var logger = LogManager.GetLogger(typeof(Updater));

        try {
            var info = AssemblyInformationData.FromAssembly();
            logger.Debug("Package ID: " + info.PackageId);
            logger.Debug("Package version: " + info.PackageVersion);
            return new DesktopGameUpdater(info.PackageId, NuGetVersion.Parse(info.PackageVersion));
        }
        catch (Exception e) {
            logger.Error("Failed to get assembly information.", e);
            if (DisplayPanicMessageBox(e))
                Environment.Exit(1);
            return new DummyGameUpdater();
        }
    }

    private static bool DisplayPanicMessageBox(Exception e) {
        // Show SDL message box displaying the exception and two options:
        //  'OK' - Continue without doing anything.
        //  'Exit' - Exit the game.
        var messageBoxData = new SDL_MessageBoxData {
            flags = SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
            title = "Error",
            message = "An exception occurred processing version data. Your copy of the game is likely not auto-updateable.\n\n"
                    + "Please report this issue to the developers.\n\n"
                    + "Exception: "
                    + e
                    + "\n\n"
                    + "Press OK to continue, or Exit to exit the game",
            numbuttons = 2,
            buttons = new SDL_MessageBoxButtonData[] {
                new()  {
                    buttonid = 0,
                    text = "OK",
                },
                new()  {
                    buttonid = 1,
                    text = "Exit",
                },
            },
        };

        SDL_ShowMessageBox(ref messageBoxData, out var buttonId);
        return buttonId == 1;
    }
}
