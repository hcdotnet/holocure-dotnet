using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace HCDN.Desktop;

internal static class NuGetUtil {
    private class Log4NetLogger : ILogger {
        private readonly ILog logger;

        public Log4NetLogger(ILog logger) {
            this.logger = logger;
        }

        public void LogDebug(string data) {
            logger.Debug(data);
        }

        public void LogVerbose(string data) {
            logger.Debug(data);
        }

        public void LogInformation(string data) {
            logger.Info(data);
        }

        public void LogMinimal(string data) {
            logger.Info(data);
        }

        public void LogWarning(string data) {
            logger.Warn(data);
        }

        public void LogError(string data) {
            logger.Error(data);
        }

        public void LogInformationSummary(string data) {
            logger.Info(data);
        }

        public void LogErrorSummary(string data) {
            logger.Error(data);
        }

        public void Log(LogLevel level, string data) {
            switch (level) {
                case LogLevel.Debug:
                    logger.Debug(data);
                    break;

                case LogLevel.Verbose:
                    logger.Debug(data);
                    break;

                case LogLevel.Information:
                    logger.Info(data);
                    break;

                case LogLevel.Minimal:
                    logger.Info(data);
                    break;

                case LogLevel.Warning:
                    logger.Warn(data);
                    break;

                case LogLevel.Error:
                    logger.Error(data);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public Task LogAsync(LogLevel level, string data) {
            Log(level, data);
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message) {
            Log(message.Level, message.Message);
        }

        public Task LogAsync(ILogMessage message) {
            Log(message);
            return Task.CompletedTask;
        }
    }

    public static ILogger GetLogger(ILog logger) {
        return new Log4NetLogger(logger);
    }

    public static async Task<(SourceRepository?, IPackageSearchMetadata?)> GetNewerPackageVersion(
        string packageId,
        string packageVersion,
        ILog logger
    ) {
        logger.Debug("Fetching package information for package: " + packageId);

        var srcProv = new PackageSourceProvider(Settings.LoadDefaultSettings(null));
        var sources = srcProv.LoadPackageSources().ToList();
        var repoProv = new SourceRepositoryProvider(srcProv, Repository.Provider.GetCoreV3());
        var version = new NuGet.Versioning.NuGetVersion(packageVersion);

        logger.Debug("Found " + sources.Count + " package sources.");

        foreach (var source in sources) {
            if (!source.IsEnabled) {
                logger.Debug("Skipping disabled package source: " + source.Name);
                continue;
            }

            if (!source.IsHttp && !source.IsHttps && !source.IsLocal) {
                logger.Debug("Skipping non-HTTP/HTTPS/Local package source: " + source.Name);
                continue;
            }

            logger.Debug("Found enabled package source: " + source.Name);
            var srcRepo = repoProv.CreateRepository(source);
            var metadataR = await srcRepo.GetResourceAsync<PackageMetadataResource>();
            var metaMetadata = (await metadataR.GetMetadataAsync(
                packageId,
                true,
                false,
                new SourceCacheContext(),
                GetLogger(logger),
                new CancellationToken()
            )).ToArray();

            if (metaMetadata.Length == 0) {
                logger.Debug("No metadata found for package: " + packageId);
                continue;
            }

            logger.Debug("Found " + metaMetadata.Length + " metadata entries for package: " + packageId);
            var greaterMetadata = metaMetadata.Where(m => m.Identity.Version > version).ToArray();
            
            if (greaterMetadata.Length == 0) {
                logger.Debug("No newer versions found for package: " + packageId);
                continue;
            }
            
            logger.Debug("Found " + greaterMetadata.Length + " newer versions for package: " + packageId);
            var newestVersion = greaterMetadata.MaxBy(m => m.Identity.Version);
            logger.Info("Newer version found for package: " + packageId + " (" + newestVersion + ")");
            return (srcRepo, newestVersion);
        }

        return (null, null);
    }
}
