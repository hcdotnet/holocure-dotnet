using HCDN.API.Updating;

namespace HCDN.Extensions;

partial class Extensions {
    public static void UpdateProgress(ref this UpdateProgress progress, int current, int total) {
        progress = progress with {
            Progress = current,
            Total = total,
        };
    }

    public static void UpdateProgress(ref this UpdateProgress progress, int current) {
        progress = progress with {
            Progress = current,
        };
    }

    public static void UpdateProgress(ref this UpdateProgress progress, string? title = null, string? message = null, int? current = null, int? total = null) {
        progress = new UpdateProgress(title ?? progress.Title, message ?? progress.Message, current ?? progress.Progress, total ?? progress.Total);
    }

    public static UpdateProgress WithTitle(this UpdateProgress progress, string? title) {
        return progress with {
            Title = title,
        };
    }

    public static UpdateProgress WithMessage(this UpdateProgress progress, string? message) {
        return progress with {
            Message = message,
        };
    }

    public static UpdateProgress WithProgress(this UpdateProgress progress, int current, int total) {
        return progress with {
            Progress = current,
            Total = total,
        };
    }

    public static UpdateProgress WithProgress(this UpdateProgress progress, int current) {
        return progress with {
            Progress = current,
        };
    }
}
