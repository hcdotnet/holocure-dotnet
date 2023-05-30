using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HCDN.Logging;

namespace HCDN.Desktop.Bootstrap;

internal static class LogInitializer {
    private const string log_file_name = "desktop.log";

    private static Logger logger = null!;

    public static Logger Logger {
        get => logger ?? throw new InvalidOperationException("Logger has not been initialized yet.");
        set => logger = value;
    }

    public static void Initialize(string name) {
        logger = new Logger(name, LogWriter.FromMany(MakeLogWriters().ToArray()));
    }

    public static Logger FromType(Type type) {
        return logger.MakeChildFromType(type);
    }

    private static IEnumerable<ILogWriter> MakeLogWriters() {
        yield return new ConsoleLogWriter();
        yield return new FileLogWriter(PrepareArchivableLogFile());
        yield return new FileLogWriter(PrepareTemporaryLogFile());
    }

    private static (string cwd, string logDir) EnsureLogDirectories() {
        var cwd = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
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
