# HoloCure.NET

> please don't sue me

---
~~~~
Free, open-source, modular, extensible HoloCure rewrite in C# with FNA. Designed
to facilitate modding with performance in mind.

## Building

The build instructions change depending on what your goal is. The following
example illustrates building in `Release` mode, but swapping `Release` for
`Debug` works fine. If your goal is to build a production-ready NuGet package
of the Desktop implementation, then a different project should be built. The
separation exists due to special steps for building a NuGet package that would
interfere with the regular testing environment.

```bash
# Clone the project and move into the created directory.
$ git clone https://github.com/hcdotnet/holocure && cd holocure

# Restore submodules.
$ git clone submodule update --init --recursive

# Build the project. "Release" for a release build. This creates a build for
# each project.
$ dotnet build "./src/HoloCure.NET.sln" -c "Release"

# If you only want to build the necessities to run the desktop implementation:
$ dotnet build "./src/HCDN.Desktop/HCDN.Desktop.csproj" -c "Release"

# ...or if you want to build a production-ready NuGet release:
$ dotnet build "./src/HCDN.Desktop.NuGet/HCDN.Desktop.NuGet.csproj" -c "Release"
```
