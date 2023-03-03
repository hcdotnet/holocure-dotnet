using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using static SDL2.SDL;

namespace HCDN.Desktop;

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
    private const string temp_path = "temp.nupkg";

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

    private static async Task<bool> CheckNuGet(InfoAsmData info, ILog logger) {
        var (sourceRepo, package) = await NuGetUtil.GetNewerPackageVersion(
            info.PackageId,
            info.PackageVersion,
            logger
        );

        if (package is null || sourceRepo is null)
            return false;

        var stagingDir = Path.Combine(Path.GetFullPath(Environment.CurrentDirectory), staging_dir);
        if (Directory.Exists(stagingDir))
            Directory.Delete(stagingDir, true);
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
