using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using static SDL2.SDL;

namespace HCDN.Desktop.Bootstrap;

internal readonly record struct InfoAsmData(
    string PackageId,
    string PackageVersion
) {
    public static InfoAsmData FromAssembly() {
        var assembly = typeof(Updater).Assembly;
        var info = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (info is null)
            throw new Exception("AssemblyInformationalVersionAttribute not found on assembly.");

        var infoParts = info.InformationalVersion.Split('|');
        if (infoParts.Length != 2)
            throw new Exception("AssemblyInformationalVersionAttribute unexpected length: " + infoParts.Length);

        var packageId = infoParts[0];
        var packageVersion = infoParts[1];

        return new InfoAsmData(packageId, packageVersion);
    }
}

internal static class Updater {
    private const string staging_dir = "staging";

    private static readonly string[] dirs_to_delete = {
        "lib64",
        "osx",
        "vulkan",
        "x64",
        "x86",
        "runtimes",
    };

    public static bool CheckForAndPromptUpdate() {
        var logger = LogManager.GetLogger(typeof(Updater));

        try {
            var info = InfoAsmData.FromAssembly();
            return CheckNuGet(info, logger).GetAwaiter().GetResult();
        }
        catch (Exception e) {
            logger.Error("Failed to get assembly information.", e);
            return DisplayPanicMessageBox(e);
        }
    }

    public static bool RunFromStaging(List<string> args) {
        var index = args.IndexOf("--staging");
        if (index == -1)
            return false;

        if (args.Count <= index + 1)
            throw new Exception("Expected path to install directory after --staging.");

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

    private static async Task<bool> CheckNuGet(InfoAsmData info, ILog logger) {
        var stagingDir = Path.Combine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory), staging_dir);
        if (Directory.Exists(stagingDir))
            Directory.Delete(stagingDir, true);

        var (sourceRepo, package) = await NuGetUtil.GetNewerPackageVersion(
            info.PackageId,
            info.PackageVersion,
            logger
        );

        if (package is null || sourceRepo is null)
            return false;

        Directory.CreateDirectory(staging_dir);

        // Show SDL message box showing an update is available and two options:
        //  'Update' - Download and unpack the update, then close the game.
        //  'Ignore' - Continue without doing anything.
        var messageBoxData = new SDL_MessageBoxData {
            flags = SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
            title = "Update Available",
            message = "An update is available for your copy of the game.\n\n"
                    + "Press Update to download and install the update, or Ignore to continue playing the current version.",
            numbuttons = 2,
            buttons = new SDL_MessageBoxButtonData[] {
                new()  {
                    buttonid = 0,
                    text = "Update",
                },
                new()  {
                    buttonid = 1,
                    text = "Ignore",
                },
            },
        };

        SDL_ShowMessageBox(ref messageBoxData, out var buttonId);

        switch (buttonId) {
            case 1:
                // Ignore.
                return false;

            case 0:
                // Update
                var downloadR = await sourceRepo.GetResourceAsync<DownloadResource>();
                var download = await downloadR.GetDownloadResourceResultAsync(
                    package.Identity,
                    new PackageDownloadContext(
                        new SourceCacheContext(),
                        stagingDir,
                        true
                    ),
                    SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null)),
                    NuGetUtil.GetLogger(logger),
                    new CancellationToken()
                );

                await PackageExtractor.ExtractPackageAsync(
                    download.PackageSource,
                    download.PackageStream,
                    new PackagePathResolver(stagingDir),
                    new PackageExtractionContext(
                        PackageSaveMode.Defaultv3,
                        XmlDocFileSaveMode.None,
                        ClientPolicyContext.GetClientPolicy(Settings.LoadDefaultSettings(null), NuGetUtil.GetLogger(logger)),
                        NuGetUtil.GetLogger(logger)
                    ),
                    new CancellationToken()
                );

                var stagingDirInfo = new DirectoryInfo(stagingDir);
                var folder = stagingDirInfo.GetDirectories().Single();
                var libFolder = folder.GetDirectories("lib").Single();
                var tgFolder = libFolder.GetDirectories().Single();
                if (tgFolder.GetFiles("HCDN.Desktop.dll").Length != 1)
                    throw new Exception("Expected exactly one HCDN.Desktop.dll in staging directory.");

                var path = AppDomain.CurrentDomain.BaseDirectory;
                if (path.EndsWith(Path.DirectorySeparatorChar))
                    path = path[..^1];
                Process.Start(new ProcessStartInfo {
                    FileName = "dotnet",
                    Arguments = $"\"{tgFolder.GetFiles("HCDN.Desktop.dll").Single().FullName}\" --staging \"{path}\"",
                    UseShellExecute = false,
                });
                return true;
        }

        return false;
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
