using System.Threading.Tasks;
using HCDN.API.Updating;

namespace HCDN.Desktop.Bootstrap.Updating; 

internal sealed class DummyGameUpdater : IUpdater {
    public Task<bool> HasUpdateAsync(IUpdateReporter reporter) {
        return Task.FromResult(false);
    }

    public Task DownloadUpdateAsync(IUpdateReporter reporter) {
        throw new System.NotImplementedException();
    }

    public Task InstallUpdateAsync(IUpdateReporter reporter) {
        throw new System.NotImplementedException();
    }
}
