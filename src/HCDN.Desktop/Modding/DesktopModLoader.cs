using System.Collections.Generic;
using HCDN.API;

namespace HCDN.Desktop.Modding; 

public class DesktopModLoader : IModLoader {
    public IDictionary<string, IMod> Mods { get; } = new Dictionary<string, IMod>();
}
