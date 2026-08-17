# Emscripten / WebAssembly Build

Use `cmake_configure_dotnet.sh` (Linux/macOS) or `cmake_configure_dotnet.bat` (Windows) to build the WebAssembly native library and NuGet package.

`cmake_configure.sh` / `cmake_configure.bat` use the system Emscripten toolchain and are kept for reference only — do **not** use them for the Blazor demo or .NET builds.

See the top-level [CLAUDE.md](../../CLAUDE.md) for full build instructions.

JPEG is intentionally disabled in this build (`BUILD_JPEG`/`WITH_JPEG=FALSE`
in `cmake_configure_dotnet.sh`) due to an unresolved WASM toolchain crash —
see [JPEG_WASM_CRASH.md](JPEG_WASM_CRASH.md) before attempting to re-enable it.
