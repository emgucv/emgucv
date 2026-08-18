# Emscripten / WebAssembly Build

Use `cmake_configure_dotnet.sh` (Linux/macOS) or `cmake_configure_dotnet.bat` (Windows) to build the WebAssembly native library and NuGet package.

`cmake_configure.sh` / `cmake_configure.bat` use the system Emscripten toolchain and are kept for reference only — do **not** use them for the Blazor demo or .NET builds.

See the top-level [CLAUDE.md](../../CLAUDE.md) for full build instructions.

JPEG is enabled in this build, but backed by `stb_image`/`stb_image_write`
instead of libjpeg-turbo (`opencv/3rdparty/stb`) — libjpeg-turbo's
`setjmp`-based error handling crashes `wasm-ld` under this build's
`-fwasm-exceptions` flag. See [JPEG_WASM_CRASH.md](JPEG_WASM_CRASH.md) for
the full investigation and the feature-parity tradeoffs of the stb_image
backend before touching `grfmt_jpeg.cpp` or the JPEG CMake flags.
