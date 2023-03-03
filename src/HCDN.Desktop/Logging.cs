using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace HCDN.Desktop;

internal static class Logging {
    private const string logger_pattern = "[%d{HH:mm:ss.fff}] [%t/%level] [%logger]: %m%n";
    private const string log_file_name = "desktop.log";

    // TODO: Decide if UTF-8 is fine for other languages.
    private static readonly Encoding logger_encoding = new UTF8Encoding(false);

    public static void Initialize() {
        ConfigureLogging();
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
