using HCDN.API.Updating;
using log4net;

namespace HCDN.Desktop;

partial class DesktopGame {
    private sealed class LoggerUpdateReporter : IUpdateReporter {
        private readonly ILog logger;

        public LoggerUpdateReporter() {
            logger = LogManager.GetLogger(typeof(DesktopGame));
        }

        public void Report(UpdateProgress progress) {
            logger.Info(progress.ToString());
        }
    }

    protected override IUpdateReporter CreateGameUpdateReporter() {
        return new LoggerUpdateReporter();
    }
}
