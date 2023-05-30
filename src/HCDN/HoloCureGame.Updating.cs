using System.Threading.Tasks;
using HCDN.API.Updating;
using SDL2;

namespace HCDN; 

partial class HoloCureGame {
    public enum DownloadState {
        Unavailable,
        Available,
        Requested,
        Downloading,
        Downloaded,
        Installing,
        Installed,
        Failed,
    }

    private DownloadState state;

    private void CheckForGameUpdates() {
        // TODO: When the infrastructure is ready, this should be moved to an
        // actual in-game display provided by HCDN.HoloCure.
        Task.Run(async () => {
            if (await GameUpdater.HasUpdateAsync(CreateGameUpdateReporter()))
                state = DownloadState.Available;
            else
                state = DownloadState.Unavailable;
            
            if (state == DownloadState.Available)
                PromptUserWithUpdate();
        });
    }

    private void ApplyGameUpdate() {
        if (state == DownloadState.Requested) {
            Task.Run(async () => {
                state = DownloadState.Downloading;
                await GameUpdater.DownloadUpdateAsync(CreateGameUpdateReporter());
                state = DownloadState.Downloaded;
            });
        }

        if (state == DownloadState.Downloaded) {
            Task.Run(async () => {
                state = DownloadState.Installing;
                await GameUpdater.InstallUpdateAsync(CreateGameUpdateReporter());
                state = DownloadState.Installed;
            });
        }
    }

    private void PromptUserWithUpdate() {
        const int update_button = 1;
        const int ignore_button = 0;
        
        // Show SDL message box showing an update is available and two options:
        //  'Update' - Download and unpack the update, then close the game.
        //  'Ignore' - Continue without doing anything.
        var messageBoxData = new SDL.SDL_MessageBoxData {
            flags = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
            title = "Update Available",
            message = "An update is available for your copy of the game.\n\n"
                    + "Press Update to download and install the update, or Ignore to continue playing the current version.",
            numbuttons = 2,
            buttons = new SDL.SDL_MessageBoxButtonData[] {
                new() {
                    buttonid = update_button,
                    text = "Update",
                },
                new() {
                    buttonid = ignore_button,
                    text = "Ignore",
                },
            },
        };

        SDL.SDL_ShowMessageBox(ref messageBoxData, out var buttonId);
        
        switch (buttonId) {
            case ignore_button:
                // Ignore.
                break;

            case update_button:
                // Update.
                state = DownloadState.Requested;
                break;
        }
    }
    
    /// <summary>
    ///     Creates a new <see cref="IUpdateReporter"/> for reporting updates.
    ///     A new one is instantiated for each update step.
    /// </summary>
    /// <returns>
    ///     A new <see cref="IUpdateReporter"/> for reporting updates.
    /// </returns>
    protected abstract IUpdateReporter CreateGameUpdateReporter();
}
