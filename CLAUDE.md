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
