# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Windows Shell Note

On this machine, `cmd.exe` in PATH may resolve to the msys64 version, which cannot run Windows batch files (`.bat`) correctly. **Always use the full path `C:\Windows\System32\cmd.exe`** when invoking cmd.exe from PowerShell or Bash tools. Example (for genuine remaining `.bat` files, e.g. `platforms/windows/docker/Docker_Build_Helper.bat`):

```powershell
& "C:\Windows\System32\cmd.exe" /c "cd /d E:\repo\emgucv_5.0\platforms\windows\docker && Docker_Build_Helper.bat"
```

Note: from **Git Bash**, avoid `cmd.exe /c "..."` entirely — Bash's MSYS path-mangling layer rewrites the bare `/c` flag into a Windows drive path (`C:\`), silently corrupting the invocation into an interactive shell that immediately exits on EOF with no output or error. Use the PowerShell tool for `cmd.exe`/`.bat` invocations instead.

All Windows native-build scripts (`platforms/windows/Build_Binary*.ps1`) are now PowerShell — invoke them directly (`& .\Build_Binary.ps1 -Arch x86_64 ...`), no `cmd.exe` wrapper needed. The legacy `.bat` equivalents have been removed.

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

To configure from scratch on Windows, run from **`platforms/android/scripts/`** (PowerShell; the legacy `build.cmd`/`wincfg.cmd`/etc. `.bat`/`.cmd` files have been removed):
```powershell
cd platforms/android/scripts
.\build.ps1 -Abi <abi> -Variant <variant> -AndroidToolchain <toolchain>
```
(positional args also work: `.\build.ps1 arm64-v8a mini`). On macOS/Linux, use `./build.sh [abi] [variant] [toolchain]` instead.

**Abi:** `arm64-v8a`, `x86_64`, `x86`, or `armeabi-v7a`

**Variant:**
- *(empty)* — full build with `opencv_contrib` + Tesseract
- `core` — core OpenCV only, no Tesseract/Freetype
- `mini` — minimal subset (strips dnn, ml, calib, video, etc.)

Requires environment variables `ANDROID_NDK`, `CMAKE`, and `MAKE` to be set — `build.ps1` auto-detects these via `wincfg.ps1` (dot-sourced) if not already set. Output lands in `android_<abi>/`, and binaries are copied to `libs/android/<abi>/`.

Other scripts in `platforms/android/scripts/` (all PowerShell): `build_all.ps1` / `rebuild_all.ps1` rebuild all four ABIs and merge them into a single `libemgucv-android.zip` (legacy jni/ant-era packaging, predates the MAUI runtime nuget flow above); `remove_from_path.ps1` is a small PATH-cleanup helper used by `rebuild_all.ps1`.

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

### Native C++ (Unity WebGL)

Unity's WebGL export uses its **own bundled Emscripten toolchain** (a specific
version pinned per Unity Editor release, found under
`<Editor>/PlaybackEngines/WebGLSupport/BuildTools/Emscripten`) -- this is a
**different, ABI-incompatible toolchain** from the .NET SDK's Emscripten
workload used above for the Blazor build (e.g. Unity 6000.3 bundles
Emscripten 3.1.39 / LLVM 17, vs. the .NET SDK's 3.1.56 / LLVM 19). The two
`cvextern` builds are not interchangeable and must never share an output
file. Whenever the target Unity version's bundled Emscripten version changes,
`cvextern.a` must be rebuilt against it.

The [Unity CLI](https://docs.unity.com/en-us/unity-cli/use-unity-cli) (`brew
install --cask unity-cli`, or the platform installers linked from that page)
manages Unity Editor installs and drives headless builds:
```bash
unity install lts                              # install the target Editor if needed
unity install-modules -e <version> -m webgl     # WebGL Build Support module
```

**Build `cvextern.a`** -- defaults to the full variant (opencv_contrib + the
full module set, matching the desktop/mobile default; this is the variant
expected at `Emgu.CV.Unity/Assets/Emgu.CV/Plugins/WebGL/cvextern.a`, though
like every other platform binary under `Plugins/`, only the `.meta` is
checked into git -- the binary itself is built locally and copied in by
hand, see `Emgu.CV.Unity/README.md`). Pass `mini` for a smaller build (no
opencv_contrib, no dnn/calib/photo/features/video) if WebGL's download-size
budget matters more than module coverage:
```bash
cd platforms/unity/webgl
./cmake_configure_unity.sh [editor-version]         # full (default), defaults to the newest installed Editor
./cmake_configure_unity.sh [editor-version] mini     # mini, no opencv_contrib
```
Output: `libs/unity-webgl/cvextern.a` (full) or `cvextern_mini.a` (mini) --
kept separate from `libs/webgl/cvextern.a`, the Blazor artifact. The script
locates the Editor's bundled `emcc`/`llvm-ar` directly (no LLVM/Node shim
juggling is needed here -- unlike the .NET SDK path, Unity ships `llvm-ar`
and `llc` directly alongside `emcc`, and its Emscripten cache/sysroot is
prebuilt and frozen, so no warmup compile is required).

The full variant (~131 MB vs. ~29 MB for mini) builds and links cleanly
against Unity's toolchain with no further cvextern-side changes, but its
larger static data segment (~81 MB) exceeds Unity's default WebGL initial
heap. The **consuming Unity project**, not cvextern.a, needs a Player
Settings change: `PlayerSettings.WebGL.initialMemorySize = 256;` (MB) in an
Editor build script -- note this is `initialMemorySize`
(`webGLInitialMemorySize` in `ProjectSettings.asset`), not the older
`PlayerSettings.WebGL.memorySize` (`webGLMemorySize`), which no longer
controls the linker's `-sINITIAL_MEMORY` in Unity 6 and silently no-ops if
you set it instead. Symptom if this is missed:
`wasm-ld: error: initial memory too small, N bytes needed`. This is applied
automatically for WebGL builds by
`Emgu.CV.Unity/Assets/Emgu.CV/Editor/WebGLBuildSettings.cs`
(`IPreprocessBuildWithReport`), which only raises the value if it's below
256MB -- the mini variant doesn't need this, so it's a no-op if you swap
back to mini.

The same script also raises `PlayerSettings.WebGL.exceptionSupport` from
`None` to `FullWithStacktrace` if it's still at the Unity default of `None`.
This one has nothing to do with the module variant -- it bites both mini and
full. `cvextern.a` is compiled without `-fwasm-exceptions` for Unity (see
below), matching Unity's own legacy JS-exception model at final link, but
that model only actually *catches* exceptions when `exceptionSupport` is
above `None`; at `None` (`-sDISABLE_EXCEPTION_CATCHING=1`), any exception
thrown inside `cvextern.a` -- including ones OpenCV throws and catches
internally via `CVAPI_CATCH_CV_ERRORS` -- surfaces instead as an uncaught,
message-less wasm trap (`Uncaught undefined`, no readable stack past the
JS/wasm boundary) on the very first native call made, even a trivial `new
Mat(64, 64, DepthType.Cv8U, 3)`. This is easy to misdiagnose as a memory,
PNG, or module-availability problem since the failure carries no message and
is fully deterministic (identical wasm frame numbers on every reload) --
bisecting down to the first statement of a `MonoBehaviour.Start()` and
testing an isolated bare-Mat scene against both a from-scratch spike project
(works) and the real project (doesn't) is what actually separates this from
those other candidates.

Two flags differ from the Blazor mini build, both required to link
successfully against Unity's own WebGL runtime rather than a bug in cvextern
itself:
- PNG -- Unity's own WebGL player runtime statically links its own libpng
  (for `Texture2D.LoadImage`); a second copy from cvextern.a naively linked
  in causes `wasm-ld: error: duplicate symbol: png_get_uint_32` etc. at
  Unity's final link step. The **full** variant avoids this via
  `EMGU_CV_EMSCRIPTEN_PNG_PREFIX:BOOL=TRUE` (alongside
  `BUILD_PNG`/`WITH_PNG:BOOL=TRUE`): `Emgu.CV.Extern/cmake/generate_png_prefix.py`
  parses `png.h`'s `PNG_EXPORT`/`PNG_EXPORTA` declarations and generates
  `pngprefix.h`, defining `#define png_foo emgu_png_foo` for all 218 public
  libpng symbols; this is force-included (`-DPNG_PREFIX=emgu_ -include
  pngprefix.h`) on just the `libpng` and `opencv_imgcodecs` targets (see
  `cmake/EmscriptenBuildFlags.cmake` for generation,
  `Emgu.CV.Extern/cmake/BuildCvExternTarget.cmake` for the
  `target_compile_options` application -- it must be scoped to those two
  targets specifically, since `SET_PROPERTY(DIRECTORY ...)` on a
  not-yet-`add_subdirectory()`'d path fails in this CMake version). This
  relies on libpng's own `PNG_PREFIX` mechanism (`pngpriv.h`:
  `#if defined(PNG_PREFIX) ... #include "pngprefix.h"`), not on
  `llvm-objcopy --redefine-sym` (confirmed unsupported on WASM objects by
  Unity's bundled LLVM). Verified end-to-end in a real Unity Editor WebGL
  build (Safari and Chrome): `Imencode`/`Imdecode` PNG round-trips with an
  exact pixel match, no link or runtime errors. The **mini** variant still
  passes `BUILD_PNG:BOOL=FALSE -DWITH_PNG:BOOL=FALSE` (no prefix flag) --
  the identical PNG_PREFIX change hit an unexplained runtime crash
  (`Uncaught exception from main loop`, a raw pointer with no message) when
  tried there, bisected to somewhere at/after the ORB feature-detection
  step, not the PNG code path itself -- so mini keeps PNG disabled and
  decodes images via `Texture2D` + `TextureConvert` instead of
  `CvInvoke.Imread`/`Imwrite`.
- `EMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS:BOOL=FALSE` -- Unity's own final link
  (its `GameAssembly.a` plus bundled runtime modules) uses Emscripten's
  legacy JS-exception model (`-sDISABLE_EXCEPTION_CATCHING=0`, no
  `-fwasm-exceptions` anywhere in Unity's own `emcc` invocation). Compiling
  cvextern.a with `-fwasm-exceptions` (the default for the Blazor build, see
  `cmake/EmscriptenBuildFlags.cmake`) mismatches that ABI and crashes
  Unity's bundled LLVM 17 `wasm-ld` with a SIGSEGV inside its "Expand
  indirectbr instructions" pass -- seen deep in core OpenCV code (e.g.
  `cv::utils::getConfigurationParameterBool`), not anything module-specific,
  so this isn't fixable by trimming modules.

**Consuming the library in a Unity project:** drop the built `cvextern.a`
into `Assets/Plugins/WebGL/cvextern.a` (see
`Emgu.CV.Unity/Assets/Emgu.CV/Plugins/WebGL/` for the reference copy and its
`.meta`). Unity recognizes bare `.a` files as native plugins automatically by
folder convention; the `.meta`'s `PluginImporter` block should still
explicitly restrict it to the WebGL platform (`SetCompatibleWithPlatform`)
so it isn't accidentally pulled into Standalone/Android/iOS builds of the
same project. No C# code changes are needed to consume it: `CvInvoke.cs`
already special-cases `UNITY_WEBGL` (treats the native library as always
loaded, no dynamic `dlopen`), and the generated `CvInvokeEntryPoints.cs`
already emits `ExternLibrary = "__Internal"` for
`(__IOS__ || UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR` -- exactly
Unity WebGL's static-linking P/Invoke model.

**Populating the managed C# source:** the checked-in `Emgu.CV.Unity` project
ships **no** Emgu.CV managed source at all -- only native-plugin placeholders
(per the `.meta`-only convention above) and the demo `.unity` scenes /
`Demo/*.cs` scripts that consume it. Opening the project or building any
scene fails immediately with `CS0234` errors (`The type or namespace name
'CvEnum' does not exist in the namespace 'Emgu.CV'`, etc.) until the source
is copied in. Two shell scripts at the project root do this (run from
`Emgu.CV.Unity/` on macOS/Linux; `copy_unity_assets.bat`/
`copy_demo_assets.bat` are the legacy Windows equivalents):
```bash
cd Emgu.CV.Unity
./copy_unity_assets   # Emgu.CV / Emgu.CV.Contrib / Emgu.Util / Emgu.CV.OCR / Emgu.CV.Models source
./copy_demo_assets    # sample images + haarcascade + DrawMatches.cs for the other demo scenes
```
`copy_unity_assets` copies a **curated** subset (not a straight directory
copy) into `Assets/Emgu.CV/Assets/Scripts/<Module>/` -- notably excluding
`Emgu.CV/Cvb`, `Emgu.CV/PInvoke/{iOS,Android,Windows.Store}`, and a handful
of Contrib modules (Aruco, Mcc, Barcode, DepthAI) that are commented out in
the script itself; edit the script if a build needs one of those.
`copy_demo_assets` requires ImageMagick's `convert` on `PATH` (`brew install
imagemagick`) to convert the sample JPEGs to PNGs, and pulls
`box.png`/`box_in_scene.png` from
`Emgu.CV.Example/MAUI/MauiDemoApp/Resources/Raw/` and a haarcascade from
`opencv_contrib/`; it's only needed for `FeatureMatchingScene`/
`StitchScene`/`FaceDetectionScene`, not `HelloWorldScene`. Neither script's
output is meant to be committed -- like the native binaries, this is local
scaffolding regenerated from the scripts, not tracked in git. Two other gaps
to know about in this managed source, both already fixed in the checked-in
`Emgu.CV.Models` source but worth remembering if a future model file adds
new code the same way: `Clip.cs`/`MultilingualE5.cs`/`Siglip.cs`/
`SmolVlm2.cs`'s private tokenizer classes use `System.Text.Json` (not
available under Unity's scripting runtime), and `Yolo.cs` had one unguarded
`Emgu.CV.Cuda.CudaInvoke.HasCuda` reference (Unity projects never compile in
`Emgu.CV.Cuda`) sitting between two otherwise-correctly-guarded blocks --
both need `#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID ||
UNITY_STANDALONE || UNITY_WEBGL` guards, or they break compilation of *any*
Unity build (all platforms, not just WebGL), not a WebGL-specific issue.
The tokenizers currently just throw `PlatformNotSupportedException` on
Unity rather than actually working there. **Future work**: the specific
`JsonDocument`/`JsonElement` pull-parsing API these files use is
reflection-free and IL2CPP/AOT-safe by design (unlike `JsonSerializer<T>`,
which needs source-generated contexts under AOT); the real gap is that
Unity doesn't ship `System.Text.Json` at all under any scripting backend.
Making it work would mean vendoring the compiled `System.Text.Json.dll`
(from NuGet) into `Assets/Plugins/`, plus the small transitive dependency
DLLs older/netstandard2.0-targeting builds need (`System.Buffers`,
`System.Memory`, `System.Numerics.Vectors`,
`System.Runtime.CompilerServices.Unsafe`,
`System.Threading.Tasks.Extensions`) -- none of which are vendored anywhere
in this Unity integration today -- and smoke-testing specifically on WebGL,
which is more restrictive about reflection/threading than desktop/mobile
IL2CPP targets.

**Building and verifying `HelloWorldScene` for WebGL end-to-end**, once the
source is copied in and `cvextern.a` is dropped into `Plugins/WebGL/`:
1. Open the project in the target Unity Editor version once (via Unity Hub
   or `unity-cli`) so it compiles the newly-copied scripts and resolves
   `Packages/manifest.json` -- a plain `-batchmode -quit` launch also works
   and is faster for CI, but a first-ever open is more reliable for surfacing
   compile errors from the copy scripts' output before spending time on a
   full WebGL build.
2. Build `HelloWorldScene.unity` for the `WebGL` target (via `unity-cli`,
   see below, or a small `BuildPlayerOptions` script targeting
   `Assets/Emgu.CV/Demo/HelloWorldScene.unity`). `WebGLBuildSettings.cs`
   (see above) fires automatically during this build and needs no manual
   `PlayerSettings` changes.
3. Serve the output locally (`python3 -m http.server <port>` from the build
   output directory) and open it in a browser. **Use a fresh, distinctive
   port for every rebuild** -- Unity's `[UnityCache]` IndexedDB layer and
   browser service-worker state persist per-origin across reloads, and a
   reused port (even one that feels "fresh" for the session) can silently
   serve a stale prior build's `.data`/`.wasm` files instead of the one just
   produced, with no error indicating this happened.
4. A clean run logs `EmguCV_SelfTest:...PASS` lines (from `HelloTexture.cs`'s
   `SelfTest`) to the browser console and renders the text overlay via the
   scene's `Canvas`/`Image`. An uncaught, message-less `Uncaught undefined`
   wasm trap instead most likely means `PlayerSettings.WebGL.exceptionSupport`
   reverted to `None` somehow (see above) -- check for that before suspecting
   memory or PNG.

**Headless build/verify** via `unity-cli`:
```bash
unity build --target WebGL --execute-method <YourBuildScript.Method> \
  --output-path ./Build/WebGL --editor-version <version> --no-tail
```
The build log streams to `<project>/Logs/build-WebGL-*.log` by default (also
written to `<project>/Logs/build-WebGL-*.log` when `--no-tail` is passed).
`unity build`'s own process exit code is not reliable on its own -- a C#
compile error can leave it at 0 even though `-executeMethod` never ran -- so
always grep the log for `error CS` and check for the literal line
`Build Finished, Result: Success.` (or `Failure.`) rather than trusting the
wrapper's exit code alone.

For a *minimal* native-only smoke test (no C# project involved), export a
trivial function with `EMSCRIPTEN_KEEPALIVE`, drop the resulting `.a` into
`Assets/Plugins/WebGL/`, and disassemble the built wasm with the Editor's
bundled `wasm-dis`
(`<Editor>/PlaybackEngines/WebGLSupport/BuildTools/Emscripten/binaryen/bin/wasm-dis`)
to confirm it appears under `(export "...")` with a real function body, not
as an unresolved import. This does **not** apply to a real cvextern.a build:
`cve*` functions are called directly from IL2CPP-generated C++ (not
JS-exported), and release builds pass `-g0`, so their names are stripped
from the wasm binary entirely -- a clean `Build Finished, Result: Success.`
with no `error CS`/`wasm-ld` lines in the log is the correct signal there.

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
- When adding a new native C++ property/method exposed to C#: add the declaration to `Emgu.CV.Extern/<module>/<class>_property.h`, add the implementation to the corresponding `.cpp`, add a `CREATE_OCV_CLASS_PROPERTY` call in `Emgu.CV.Extern/cmake/PropertyInvocations.cmake` (included from `Emgu.CV.Extern/CMakeLists.txt`) pointing to the correct module folder and include header, then re-run CMake to regenerate the `.g.cs` file. Do not write `.g.cs` files by hand. The macro itself is defined in `Emgu.CV.Extern/cmake/CodeGenMacros.cmake`.
- When adding a new `VectorOf*` collection type: use the CMake `CREATE_VECTOR_CS` macro (defined in `Emgu.CV.Extern/cmake/CodeGenMacros.cmake`, most invocations colocated there; a few module-specific ones live in `Emgu.CV.Extern/cmake/PropertyInvocations.cmake`) rather than writing the wrapper manually.
- When adding a new OpenCV module wrapper: create `Emgu.CV.Extern/<module>/<module>_c.h` and `<module>_c.cpp` for the C++ side (guard all implementations with `#ifdef HAVE_OPENCV_<MODULE>` / `throw_no_<module>()` fallback); create a matching `Emgu.CV/<Module>/` folder for the C# wrappers; add `<Compile Include="$(MSBuildThisFileDirectory)<Module>\*.cs" />` to `Emgu.CV/Emgu.CV.Shared.projitems`.
- Native functions reachable from C# that can hit `CV_Error(...)` (directly or via any OpenCV call) must wrap their body in `try { ... } CVAPI_CATCH_CV_ERRORS(returnValueOnError)` (or `CVAPI_CATCH_CV_ERRORS_VOID` for `void` functions) from `Emgu.CV.Extern/emgu_error.h`. A managed exception thrown from inside the `redirectError` callback would need to unwind across the native call frame that invoked it, which the .NET runtime treats as fatal — so the native exception must be caught locally instead, and the corresponding C# wrapper must call `CvInvoke.CheckError()` right after the P/Invoke call to raise the equivalent `CvException` safely, from managed code. This pattern has been rolled out across essentially every module (core, imgproc, video, objdetect, dnn, calib, features, face, photo, stitching, ximgproc, videoio, imgcodecs, ml, xfeatures2d, tracking, tesseract, flann, xphoto, quality, cuda, and more) — new native entry points should follow the same pattern rather than being treated as an exception. Set `EMGU_CV_DISABLE_ERROR_HANDLER=1` to exercise the fallback path used on platforms (iOS, MacCatalyst, Blazor/WASM, Unity WebGL) that never register a `redirectError` callback.
- Root `CMakeLists.txt` and `Emgu.CV.Extern/CMakeLists.txt` are thin entry points: most of their logic lives in included `cmake/*.cmake` files (e.g. `cmake/PlatformDetection.cmake`, `cmake/NugetIds.cmake` at the root; `Emgu.CV.Extern/cmake/PropertyInvocations.cmake`, `Emgu.CV.Extern/cmake/BuildCvExternTarget.cmake` under Extern). `include()` doesn't create a new variable scope or change `CMAKE_CURRENT_SOURCE_DIR`, so these behave identically to inline code — look there first before assuming logic is missing.
