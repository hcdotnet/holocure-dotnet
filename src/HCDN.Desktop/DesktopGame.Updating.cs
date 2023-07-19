using HCDN.API.Updating;
using HCDN.Desktop.Bootstrap;
using HCDN.Logging;

namespace HCDN.Desktop;

internal partial class DesktopGame {
    private sealed class LoggerUpdateReporter : IUpdateReporter {
        private readonly Logger logger;

        public LoggerUpdateReporter() {
            logger = LogInitializer.FromType(typeof(LoggerUpdateReporter));
        }

        public void Report(UpdateProgress progress) {
            logger.Info(progress.ToString());
        }
    }

    protected override IUpdateReporter CreateGameUpdateReporter() {
        return new LoggerUpdateReporter();
    }
}
