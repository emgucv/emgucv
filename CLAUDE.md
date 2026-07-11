# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Windows Shell Note

On this machine, `cmd.exe` in PATH may resolve to the msys64 version, which cannot run Windows batch files (`.bat`) correctly. **Always use the full path `C:\Windows\System32\cmd.exe`** when invoking cmd.exe from PowerShell or Bash tools. Example:

```powershell
& "C:\Windows\System32\cmd.exe" /c "E:\repo\emgucv_5.0\platforms\windows\Build_Binary_x86-64_inf_doc.bat"
```

## Project Overview

Emgu CV is a .NET wrapper for OpenCV. It has two distinct layers:

1. **C++ native layer** (`Emgu.CV.Extern/`): Builds into `cvextern.dll` via CMake. Wraps OpenCV C++ APIs with a C-style API (all functions prefixed `cve`).
2. **C# managed layer** (`Emgu.CV/`, `Emgu.CV.Contrib/`, etc.): P/Invoke wrappers that call into `cvextern`. Uses shared `.projitems` files so one set of source files builds for multiple target frameworks/platforms.

## Build Commands

### Native C++ (CMake)
The CMake build has already been configured in `build_x86_64/`. To rebuild the native library:
```bash
cd build_x86_64
cmake --build . --config Release --target cvextern
```

To reconfigure from scratch (Windows x64):
```bash
mkdir build_x86_64 && cd build_x86_64
cmake .. -G "Visual Studio 17 2022" -A x64
```

### C# (MSBuild / dotnet)
Open the main solution in Visual Studio:
- `build_x86_64/emgucv.sln` — full solution (C++ + C#)
- `Solution/Windows.Desktop/Emgu.CV.sln` — C# only, desktop

Build from command line:
```bash
dotnet build Emgu.CV/NetStandard/Emgu.CV.csproj
dotnet build Solution/Windows.Desktop/Emgu.CV.NetStandard.sln
```

### Native C++ (Android)
The Android build is pre-configured per-ABI in `android_<abi>/` (e.g. `android_arm64-v8a/`, `android_x86_64/`, `android_x86/`, `android_armeabi-v7a/`). To rebuild the native library and the MAUI runtime nuget package for an ABI:
```bash
cd android_arm64-v8a
cmake --build . --config Release --target Emgu.CV.runtime.maui
```

If CMakeLists.txt files changed, reconfigure first:
```bash
cd android_arm64-v8a
cmake .
```

To configure from scratch, run from **`platforms/android/scripts/`**:
```bash
cd platforms/android/scripts
build.cmd [abi] [variant] [toolchain]
```

**ABI (`%1`):** `arm64-v8a`, `x86_64`, `x86`, or `armeabi-v7a`

**Variant (`%2`):**
- *(empty)* — full build with `opencv_contrib` + Tesseract
- `core` — core OpenCV only, no Tesseract/Freetype
- `mini` — minimal subset (strips dnn, ml, calib, video, etc.)

Requires environment variables `ANDROID_NDK`, `CMAKE`, and `MAKE` to be set. Output lands in `android_<abi>/`, and binaries are copied to `libs/android/<abi>/`.

### Native C++ (iOS / Xcode)
The iOS build uses shell scripts in `platforms/ios/`. Run from within **`platforms/ios/`**:

```bash
cd platforms/ios
./build.sh [target] [variant]
```

**Targets (`$1`):**
- *(empty or `all`)* — build all three targets
- `device_arm64` — physical iOS device (arm64)
- `simulator_arm64` — Apple Silicon simulator
- `simulator_x86_64` — Intel simulator

**Variants (`$2`):**
- *(empty)* — full build with `opencv_contrib` + Tesseract
- `core` — core OpenCV only, no contrib/Freetype/Tesseract
- `mini` — minimal subset (strips dnn, ml, calib, video, etc.)

Example — device only, core variant:
```bash
cd platforms/ios
./build.sh device_arm64 core
```

Output lands in `platforms/ios/{iphoneos_arm64,simulator_arm64,simulator_x86_64}/`.

Prerequisites:
- Xcode installed at `/Applications/Xcode.app`
- `eigen` submodule present (built automatically as a dependency)
- For full/default builds: `opencv_contrib/` submodule present

The script calls `configure_xcode.sh` which invokes `cmake -GXcode` with the appropriate iOS toolchain file from `opencv/platforms/ios/cmake/Toolchains/`, then builds with `xcodebuild`. Deployment target is iOS 14.2 for device and simulator.

### Native C++ (MacCatalyst / Xcode)
Similar to iOS, run from within **`platforms/ios/`**:

```bash
cd platforms/ios
./build_catalyst.sh [arch] [variant]
```

**Architectures (`$1`):**
- `arm64` — Apple Silicon
- `x86_64` — Intel

**Variants (`$2`):** same as iOS — *(empty)* for full, `core`, or `mini`.

Example — Apple Silicon, full variant:
```bash
cd platforms/ios
./build_catalyst.sh arm64
```

Output lands in `platforms/ios/catalyst_<arch>/`.

### MAUI Demo App (Windows)

**Prerequisites:** Windows App Runtime 1.8 must be installed. Download from
https://aka.ms/windowsappruntimeinstall or install via `winget install Microsoft.WindowsAppRuntime.1.8`.

Pass `WindowsPackageType=None` on the command line (unpackaged mode) so the app
runs directly as an `.exe` without MSIX packaging. It uses bootstrap mode to
locate the installed Windows App Runtime 1.8 framework package at startup.

**Build:**
```bash
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj -f net10.0-windows10.0.19041.0 -p:WindowsPackageType=None
```

**Run:**
```bash
Emgu.CV.Example/MAUI/MauiDemoApp/bin/Debug/net10.0-windows10.0.19041.0/win-x64/MauiDemoApp.exe
```

### MAUI Demo App (MacCatalyst)

After the MacCatalyst C++ binary is built, build then launch the MAUI demo app directly on macOS:

```bash
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj -f net10.0-maccatalyst
open "Emgu.CV.Example/MAUI/MauiDemoApp/bin/Debug/net10.0-maccatalyst/maccatalyst-arm64/Emgu.app"
```

Note: `-t:Run` does not work reliably for MacCatalyst — build and `open` separately instead.

If the build fails with "This version of .NET for MacCatalyst requires Xcode
X.Y. The current version of Xcode is X.Z", the installed Xcode is newer than
the version the .NET MacCatalyst SDK expects (the check is an exact
major.minor match). Skip the check by adding `-p:ValidateXcodeVersion=false`
to the build command — this is the SDK's own opt-out for that validation and
does not require changing workloads or Xcode.

### MAUI Demo App (iOS Simulator)

After the C++ iOS binaries are built, you can build and run the MAUI demo app on an iOS simulator.

Build then run on the default iOS simulator:
```bash
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj -f net10.0-ios
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj -f net10.0-ios -t:Run
```

The project is defined in `Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj`. The solution file for iOS is at `Solution/iOS/Emgu.CV.iOS.Maui.sln`.

### Native C++ (WebAssembly / Emscripten)

The script in `platforms/emscripten/` configures and builds `libs/webgl/cvextern.a`
using the Emscripten toolchain bundled with the .NET SDK WASM workload
(`Microsoft.NET.Runtime.Emscripten.*.Sdk.*` under `~/.dotnet/packs`).
If the workload is not installed, run `dotnet workload install wasm-tools` first.

**Full build** (all OpenCV modules + contrib, outputs `libs/webgl/cvextern.a`
and `platforms/nuget/Emgu.CV.runtime.webassembly.*.nupkg`,
build tree in `platforms/emscripten/build_dotnet/`):
```bash
cd platforms/emscripten
./cmake_configure_dotnet.sh
```

**Mini build** (core modules only — no contrib, no dnn/calib/photo/features/video,
also outputs `libs/webgl/cvextern.a` and the NuGet package,
build tree in `platforms/emscripten/build_dotnet_mini/`):
```bash
cd platforms/emscripten
./cmake_configure_dotnet.sh mini
```

Both variants build `cvextern.a` and then the `Emgu.CV.runtime.webassembly`
NuGet package in one step. To rebuild just the NuGet package after the native
library is already up to date:
```bash
cd platforms/emscripten/build_dotnet      # or build_dotnet_mini for mini
make Emgu.CV.runtime.webassembly
```

The older `cmake_configure.sh` uses the system Emscripten instead and outputs
to `build/` — do not use it for the Blazor demo.

### Blazor WebAssembly Demo

The demo project is `Emgu.CV.Example/HelloWorld.Blazor/`.  It links
`cvextern.a` into `dotnet.native.wasm` via `NativeFileReference` (supplied by
the `Emgu.CV.runtime.webassembly` NuGet package from the local feed at
`platforms/nuget/`).

**Prerequisites (one-time, automated)**

`WasmCachePath` in the csproj points to `platforms/emscripten/wasm-cache/`.
Because this differs from the SDK's default cache directory, the build sets
`EM_FROZEN_CACHE=0` automatically and rebuilds any missing Emscripten system
libraries (e.g. `libbulkmemory.a`) on the first run.  No manual setup is
needed after a fresh checkout.

**Build:**
```bash
dotnet build Emgu.CV.Example/HelloWorld.Blazor/HelloWorld.Blazor.csproj
```

The first build takes ~4 minutes (Emscripten link step).  Subsequent builds
are incremental and much faster.

**Run:**
```bash
dotnet run --project Emgu.CV.Example/HelloWorld.Blazor/HelloWorld.Blazor.csproj --no-build
```

Then open **http://localhost:5000** and click **Run OpenCV Demo**.

**Key design notes:**
- `BLAZORWASM` define → `ExternLibrary = "cvextern"`, matching the
  `NativeFileReference` filename and the Mono WASM P/Invoke table key.
- `WasmInitialHeapSize` is set to 128 MB because the full statically-linked
  cvextern data segment requires ~80 MB.
- `-O1` link optimisation prevents the WASM local-count browser limit from
  being hit by xfeatures2d functions at the default `-O0`.
- `-flto` is intentionally **not** used even though `cvextern.a` contains
  LLVM IR bitcode (Emscripten's default object format). wasm-ld compiles
  bitcode to WASM at link time without `-flto`; the flag only adds
  cross-module optimisation. With `-flto`, emcc switches all system libraries
  to the LTO sysroot (bitcode), which causes `"attempt to add bitcode file
  after LTO"` when Blazor's regular-WASM objects (pinvoke.o, libmono*.a, etc.)
  lazily pull in `libc.a(htonl.o)` after the LTO pass has concluded.
- `cvextern.a` is shipped via the `Emgu.CV.runtime.webassembly` NuGet package
  (in `build/native/`). The package's `.targets` file adds it to
  `NativeFileReference` when `WasmBuildNative=true`. The local feed is at
  `platforms/nuget/` (configured in `nuget.config`).
- `-sSUPPORT_LONGJMP=wasm` enables WASM-native setjmp/longjmp for OpenCV's
  JPEG codec and others. The SjLj-lowering pass runs via the
  `-mllvm -wasm-enable-sjlj` flag that this Emscripten setting injects.
- `wasm_sjlj.lib.js` provides JS stubs for `__wasm_setjmp` / `__wasm_setjmp_test`
  symbols generated by the LLVM SjLj-lowering pass when bitcode compiled
  without `-fwasm-exceptions` is linked with `-fwasm-exceptions`.

### Ubuntu Release Package

The CMake build is pre-configured in `platforms/ubuntu/<version>/build/` (e.g. `platforms/ubuntu/22.04/build/`). To build all NuGet packages and CPack archives in one go:

```bash
cd platforms/ubuntu/<version>/build
cmake --build . --config Release --parallel $(nproc)
cpack
```

This produces:
- **NuGet packages** in `platforms/nuget/` — `Emgu.CV.*.nupkg`, `Emgu.CV.Bitmap.*.nupkg`, `Emgu.CV.Models.*.nupkg`, `Emgu.CV.runtime.ubuntu-<version>-x64.*.nupkg`
- **CPack archives** in `platforms/ubuntu/<version>/build/` — `libemgucv-ipp-<version>.sh`, `.tar.gz`, `.tar.Z`

To reconfigure from scratch:
```bash
cd platforms/ubuntu/<version>
./cmake_configure
```

### Running Tests
```bash
dotnet test Emgu.CV.Test/Emgu.CV.Test.Net/Emgu.CV.Test.Net.csproj
```

Run a single test (by name filter):
```bash
dotnet test Emgu.CV.Test/Emgu.CV.Test.Net/Emgu.CV.Test.Net.csproj --filter "TestName"
```

Test files (NUnit) are in `Emgu.CV.Test/` as `AutoTest*.cs`.

### Cleaning the Repository

To remove all build outputs and generated files from the repository and every
submodule in one go:

```bash
./miscellaneous/git-clean
```

The script runs `git clean -d -f -x` in each submodule (opencv, opencv_contrib,
opencv_extra, eigen, vtk, tesseract, leptonica, depthai-core, freetype2,
harfbuzz, hdf5, openvino) and finally in the repository root. It deletes
**every untracked file**, including:

- CMake build trees (`build_x86_64/`, `platforms/ubuntu/*/build/`,
  `platforms/emscripten/build_dotnet/`, etc.)
- Native binaries under `libs/`
- Generated sources (`*.g.cs`, `Util/VectorOf*.cs`, `Directory.Build.props`,
  `CvInvokeEntryPoints.cs`) — these are recreated by the next CMake configure
- NuGet packages in `platforms/nuget/`
- All `bin/` and `obj/` directories

Before running it, commit (or move out of the tree) any untracked file you
want to keep — uncommitted patches, notes, or downloaded models are deleted
without confirmation. Modified *tracked* files are not affected. After a
clean, the relevant platform `cmake_configure` script must be re-run before
anything can be built.

### C# Project Structure
Projects share source via `.projitems` files (MSBuild Shared Projects):
- `Emgu.CV/Emgu.CV.Shared.projitems` — core OpenCV wrapper
- `Emgu.CV.Contrib/Emgu.CV.Contrib.projitems` — contrib modules (aruco, text, MCC, etc.)
- `Emgu.CV.Cuda/Emgu.CV.Cuda.projitems` — CUDA/GPU support
- `Emgu.CV.OCR/Emgu.CV.OCR.projitems` — Tesseract OCR

Platform runtime libraries (which native DLLs to load) are in `Emgu.CV.Runtime/`:
- `Windows/`, `Mac/`, `Debian/`, `Ubuntu/`, `RHEL/`, `UWP/`, `Maui/`

The entry point project for the main managed assembly is `Emgu.CV/NetStandard/Emgu.CV.csproj`.

### P/Invoke Pattern
All native calls go through the static partial class `CvInvoke` (in `namespace Emgu.CV`). Core wrappers live in `Emgu.CV/PInvoke/`; module-specific wrappers live in their own folder alongside the corresponding C# classes (e.g., `Emgu.CV/Geometry/GeometryInvoke.cs`, `Emgu.CV/Photo/CvInvokePhoto.cs`). All files contribute to the same `partial class CvInvoke`.
- Native library constant: `CvInvoke.ExternLibrary = "cvextern"`
- Calling convention: `CallingConvention.Cdecl`
- Bool marshaling: `UnmanagedType.U1`
- Native function naming convention: `cve` prefix (e.g., `cveMatIsContinuous`)

### Code Generation (Do Not Edit Generated Files)
CMake generates several files automatically — **never edit these manually**:

- `Emgu.CV/PInvoke/CvInvokeEntryPoints.cs` — library name constants
- `Emgu.CV/**/*.g.cs` — P/Invoke stubs generated from `Emgu.CV.Extern/**/*_property.h` files
- `Emgu.CV/Util/VectorOf*.cs` — vector wrappers generated from templates in `Emgu.CV.Extern/cmake/`
- Same pattern applies under `Emgu.CV.Contrib/`, `Emgu.CV.Cuda/`, etc.

The C headers (`*_property.h`) and the CMake templates (`Emgu.CV.Extern/cmake/*.cs.in`, `*.cpp.in`, `*.h.in`) are the sources of truth for generated code.

### Native Library Layout
After building, native binaries land in `libs/runtimes/<rid>/native/` and are referenced by the NuGet packages in `platforms/nuget/`.

### Third-party Dependencies
- `opencv/` — OpenCV source (built as part of CMake)
- `Emgu.CV.Extern/tesseract/` — Tesseract OCR + Leptonica
- `3rdParty/openvino/` — Intel OpenVINO (optional)
- `3rdParty/freetype2/`, `harfbuzz/`, `hdf5/`, `eigen/`, `vtk/` — optional components
- `build_x86_64/` — CMake build directory (generated, do not add to git)

### Platform Solutions
- `Solution/Windows.Desktop/` — Windows desktop
- `Solution/Android/`, `Solution/iOS/`, `Solution/Mac/` — mobile/macOS
- `Solution/Ubuntu/` — Linux
- `Solution/CrossPlatform/` — multi-target

## Key Conventions

- The assembly is strong-named using `Emgu.CV.snk`.
- `UNSAFE_ALLOWED` and `AllowUnsafeBlocks` are enabled project-wide.
- `NETSTANDARD` is defined in Debug builds.
- When adding a new native C++ property/method exposed to C#: add the declaration to `Emgu.CV.Extern/<module>/<class>_property.h`, add the implementation to the corresponding `.cpp`, update the `CREATE_OCV_CLASS_PROPERTY` call in `Emgu.CV.Extern/CMakeLists.txt` to point to the correct module folder and include header, then re-run CMake to regenerate the `.g.cs` file. Do not write `.g.cs` files by hand.
- When adding a new `VectorOf*` collection type: use the CMake `CREATE_VECTOR_CS` macro in `Emgu.CV.Extern/CMakeLists.txt` rather than writing the wrapper manually.
- When adding a new OpenCV module wrapper: create `Emgu.CV.Extern/<module>/<module>_c.h` and `<module>_c.cpp` for the C++ side (guard all implementations with `#ifdef HAVE_OPENCV_<MODULE>` / `throw_no_<module>()` fallback); create a matching `Emgu.CV/<Module>/` folder for the C# wrappers; add `<Compile Include="$(MSBuildThisFileDirectory)<Module>\*.cs" />` to `Emgu.CV/Emgu.CV.Shared.projitems`.
