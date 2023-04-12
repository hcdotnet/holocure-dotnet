using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HCDN.API.Updating;
using HCDN.Extensions;
using NuGet.Versioning;

namespace HCDN.Desktop.Bootstrap.Updating;

internal sealed class DesktopGameUpdater : AbstractNuGetUpdater {
    public DesktopGameUpdater(string packageId, NuGetVersion currentVersion) : base(packageId, currentVersion) { }

    protected override Task InnerInstallUpdateAsync(IUpdateReporter reporter) {
        var progress = new UpdateProgress("Installing update...", "Installing update...", 0, 3);
        reporter.Report(progress);

        reporter.Report(progress.WithMessage("Validating installation...").WithProgress(1, 3));
        var dirInfo = new DirectoryInfo(PkgDownload!.Path);
        var root = dirInfo.GetDirectories().Single();
        var lib = root.GetDirectories("lib").Single();
        var tg = lib.GetDirectories().Single();
        if (tg.GetFiles("HCDN.Desktop.dll").Length != 1)
            throw new IOException("Expected exactly one 'HCDN.Desktop.dll' file in staging directory.");

        var path = AppDomain.CurrentDomain.BaseDirectory;
        if (path.EndsWith(Path.DirectorySeparatorChar))
            path = path[..^1];

        reporter.Report(progress.WithMessage("Starting update daemon...").WithProgress(2, 3));
        Process.Start(new ProcessStartInfo {
            FileName = "dotnet",
            Arguments = $"\"{tg.GetFiles("HCDN.Desktop.dll").Single().FullName}\" --staging \"{path}\"",
            UseShellExecute = false,
        });
        reporter.Report(progress.WithMessage("Update daemon started, exiting...").WithProgress(3, 3));

        // TODO: Close safely when the game actually furthers in development...
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}
