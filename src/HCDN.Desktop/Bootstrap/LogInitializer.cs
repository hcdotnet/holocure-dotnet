using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HCDN.Logging;

namespace HCDN.Desktop.Bootstrap;

internal static class LogInitializer {
    private static Logger logger = null!;

    public static Logger Logger {
        get => logger ?? throw new InvalidOperationException("Logger has not been initialized yet.");
        set => logger = value;
    }

    public static void Initialize(string name, string logFileName) {
        var writers = LogWriter.FromMany(MakeLogWriters(logFileName).ToArray());
        Logger = new Logger(name, writers);
    }

    public static Logger FromType(Type type) {
        return Logger.MakeChildFromType(type);
    }

    private static IEnumerable<ILogWriter> MakeLogWriters(string logFileName) {
        yield return new ConsoleLogWriter();
        yield return new FileLogWriter(PrepareArchivableLogFile(logFileName));
        yield return new FileLogWriter(PrepareTemporaryLogFile(logFileName));
    }

    private static (string cwd, string logDir) EnsureLogDirectories() {
        var cwd = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
        var logDir = Path.Combine(cwd, "logs");

        Directory.CreateDirectory(logDir);

        return (cwd, logDir);
    }

    private static string PrepareArchivableLogFile(string logFileName) {
        var (_, logDir) = EnsureLogDirectories();
        var name = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + logFileName + ".log";
        var logFile = Path.Combine(logDir, name);

        if (File.Exists(logFile)) {
            // TODO: Panic?
        }

        return logFile;
    }

    private static string PrepareTemporaryLogFile(string logFileName) {
        var (cwd, _) = EnsureLogDirectories();
        var logFile = Path.Combine(cwd, logFileName + ".log");

        if (File.Exists(logFile)) {
            File.Delete(logFile);
        }

        return logFile;
    }
}
