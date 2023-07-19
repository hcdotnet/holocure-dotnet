using System;
using HCDN.Desktop.Launch;

namespace HCDN.Desktop.Exceptions;

public sealed class InvalidLaunchTypeException : Exception {
    internal InvalidLaunchTypeException(LaunchType launchType) : base(ReportLaunchType(launchType)) { }

    private static string ReportLaunchType(LaunchType launchType) {
        return $"Invalid launch type: {launchType}";
    }
}
