using System;
using System.IO;
using System.Runtime.InteropServices;
using log4net;

namespace HCDN.Desktop.Bootstrap;

internal static class FnaBootstrapper {
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetDefaultDllDirectories(int directoryFlags);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern void AddDllDirectory(string lpPathName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetDllDirectory(string lpPathName);

    private const int load_library_search_default_dirs = 0x00001000;

    public static void Bootstrap() {
        var logger = LogManager.GetLogger(typeof(FnaBootstrapper));
        var platform = Environment.OSVersion.Platform;
        var is64Bit = Environment.Is64BitProcess;
        var fnalibsDir = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            is64Bit ? "x64" : "x86"
        );

        logger.Debug("Bootstrapping FNA...");
        logger.Debug("OS Platform: " + platform);
        logger.Debug("Is 64-bit: " + is64Bit);

        if (platform != PlatformID.Win32NT) {
            logger.Debug("Not on Win32NT, skipping FNA bootstrap.");
            return;
        }

        logger.Debug("fnalibs directory: " + fnalibsDir);

        try {
            logger.Debug("Attempting Windows 7 KB2533623+ bootstrap...");
            SetDefaultDllDirectories(load_library_search_default_dirs);
            AddDllDirectory(fnalibsDir);
        }
        catch {
            logger.Debug("Windows 7+ bootstrap failed, ");
            SetDllDirectory(fnalibsDir);
        }
    }
}
