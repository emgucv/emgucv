# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

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

### MAUI Demo App (MacCatalyst)

After the MacCatalyst C++ binary is built, build then launch the MAUI demo app directly on macOS:

```bash
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj -f net10.0-maccatalyst
open "Emgu.CV.Example/MAUI/MauiDemoApp/bin/Debug/net10.0-maccatalyst/maccatalyst-arm64/Emgu CV MAUI Demo.app"
```

Note: `-t:Run` does not work reliably for MacCatalyst — build and `open` separately instead.

### MAUI Demo App (iOS Simulator)

After the C++ iOS binaries are built, you can build and run the MAUI demo app on an iOS simulator.

Build then run on the default iOS simulator:
```bash
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj -f net10.0-ios
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj -f net10.0-ios -t:Run
```

The project is defined in `Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj`. The solution file for iOS is at `Solution/iOS/Emgu.CV.iOS.Maui.sln`.

### Running Tests
```bash
dotnet test Emgu.CV.Test/Emgu.CV.Test.Net/Emgu.CV.Test.Net.csproj
```

Run a single test (by name filter):
```bash
dotnet test Emgu.CV.Test/Emgu.CV.Test.Net/Emgu.CV.Test.Net.csproj --filter "TestName"
```

Test files (NUnit) are in `Emgu.CV.Test/` as `AutoTest*.cs`.

## Architecture

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
All native calls go through the static partial class `CvInvoke` in `Emgu.CV/PInvoke/`.
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
- When adding a new native C++ property/method exposed to C#: add the declaration to `Emgu.CV.Extern/<module>/<class>_property.h`, add the implementation to the corresponding `.cpp`, then re-run CMake to regenerate the `.g.cs` file. Do not write `.g.cs` files by hand.
- When adding a new `VectorOf*` collection type: use the CMake `CREATE_VECTOR_CS` macro in `Emgu.CV.Extern/CMakeLists.txt` rather than writing the wrapper manually.
