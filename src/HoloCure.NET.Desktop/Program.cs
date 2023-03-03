using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace HoloCure.NET.Desktop;

internal static class Program {
    private const string logger_pattern = "[%d{HH:mm:ss.fff}] [%t/%level] [%logger]: %m%n";
    private const string log_file_name = "desktop.log";

    // TODO: Decide if UTF-8 is fine for other languages.
    private static readonly Encoding logger_encoding = new UTF8Encoding(false);

    [STAThread]
    internal static void Main(string[] args) {
        ConfigureLogging();
        var logger = LogManager.GetLogger(typeof(Program));
        logger.Debug("Configured logging!");

        if (args.Length > 0) {
            logger.Debug("Started process with launch arguments:");
            foreach (var arg in args)
                logger.Debug($"  {arg}");
        }
        else {
            logger.Debug("Started process without launch arguments.");
        }

        logger.Debug("Running in CWD: " + Environment.CurrentDirectory);

        Bootstrap.BootstrapFna();

        logger.Info("Starting game...");
        RunGame(logger);
    }

    private static void RunGame(ILog logger) {
        logger.Debug($"Initialization {nameof(DesktopGame)} instance...");
        using var game = new DesktopGame();

        logger.Debug($"Running {nameof(DesktopGame)} instance...");
        game.Run();
    }

    private static void ConfigureLogging() {
        var layout = new PatternLayout {
            ConversionPattern = logger_pattern,
        };
        layout.ActivateOptions();

        var appenders = MakeAppenders(layout).ToArray();
        BasicConfigurator.Configure(appenders);
    }

    private static IEnumerable<IAppender> MakeAppenders(ILayout layout) {
        yield return new ConsoleAppender {
            Name = nameof(ConsoleAppender),
            Layout = layout,
        };

        yield return new DebugAppender {
            Name = nameof(DebugAppender),
            Layout = layout,
        };

        var archivable = new FileAppender {
            Name = "Archivable" + nameof(FileAppender),
            File = PrepareArchivableLogFile(),
            AppendToFile = false,
            Encoding = logger_encoding,
            Layout = layout,
        };
        archivable.ActivateOptions();
        yield return archivable;

        var temporary = new FileAppender {
            Name = "Temporary" + nameof(FileAppender),
            File = PrepareTemporaryLogFile(),
            AppendToFile = false,
            Encoding = logger_encoding,
            Layout = layout,
        };
        temporary.ActivateOptions();
        yield return temporary;
    }

    private static (string cwd, string logDir) EnsureLogDirectories() {
        var cwd = Path.GetFullPath(Environment.CurrentDirectory);
        var logDir = Path.Combine(cwd, "logs");

        Directory.CreateDirectory(logDir);

        return (cwd, logDir);
    }

    private static string PrepareArchivableLogFile() {
        var (_, logDir) = EnsureLogDirectories();
        var name = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + log_file_name;
        var logFile = Path.Combine(logDir, name);

        if (File.Exists(logFile)) {
            // TODO: Panic?
        }

        return logFile;
    }

    private static string PrepareTemporaryLogFile() {
        var (cwd, _) = EnsureLogDirectories();
        var logFile = Path.Combine(cwd, log_file_name);

        if (File.Exists(logFile)) {
            File.Delete(logFile);
        }

        return logFile;
    }
}
