# JPEG codec in WASM builds — the wasm-ld crash and its fix

**Status as of 2026-08-18: resolved.** JPEG is enabled in WASM builds, backed
by `stb_image`/`stb_image_write` (`opencv/3rdparty/stb`) instead of
libjpeg-turbo — see "The fix" below. The two earlier libjpeg-turbo-side fix
attempts (documented below for reference) both failed; don't repeat them.

## Symptom

With JPEG built via libjpeg-turbo and `-fwasm-exceptions` on (see
`cmake/EmscriptenBuildFlags.cmake`), the **final link** of any project that
consumes `cvextern.a` (e.g. `HelloWorld.Blazor.csproj`'s `dotnet build`)
crashed `wasm-ld` with a SIGSEGV inside LLVM's SelectionDAG instruction
selector:

```
Running pass 'WebAssembly Instruction Selection' on function '@_ZN2cv11JpegEncoder5writeERKNS_3MatERKNSt3__26vectorIiNS4_9allocatorIiEEEE'
... SIGSEGV ...
llvm::SelectionDAGBuilder::HandlePHINodesInSuccessorBlocks(...)
llvm::SelectionDAGISel::SelectBasicBlock(...)
...
lld::wasm::BitcodeCompiler::compile()
```

The crashing symbol was always `cv::JpegEncoder::write(cv::Mat const&, std::vector<int> const&)`
in libjpeg-turbo's implementation of `opencv/modules/imgcodecs/src/grfmt_jpeg.cpp`.

**Important**: `cvextern.a` itself builds fine even when this bug is present.
Its own build step archives LLVM bitcode via `llvm-ar rcs` — it never runs
real WASM codegen/instruction-selection. Only the *final* link of a consuming
project (a real `dotnet build` of the Blazor demo, not just the native
`cmake --build`) exercises this path. Treating "cvextern.a built cleanly" as
a green light is a trap — always verify with a full Blazor rebuild.

## Root cause hypothesis

`JpegEncoder::write` (libjpeg-turbo path) was the only imgcodecs encoder
function that mixed, in the same function, **both**:
- a `setjmp(jerr.setjmp_buffer)` call (libjpeg's C error-handling idiom), which
  requires WASM SjLj lowering, and
- a reachable C++ throw/RAII-unwind path (`CV_Error`, `CV_Check`, and the
  `fileWrapper` destructor), which requires WASM-native EH landing pads
  (`-fwasm-exceptions`).

`PngEncoder::write` (`grfmt_png.cpp`) uses the identical
`setjmp(png_jmpbuf(png_ptr))` pattern and RAII (`AutoBuffer`), but has **zero**
throw sites in its body — and it does *not* crash. This asymmetry was the
strongest evidence for the hypothesis: LLVM19's `wasm-ld` (Emscripten SDK
3.1.56 toolchain, bundled with .NET 10 wasm-tools workload) appears to have a
real bug lowering SjLj and WASM-EH together within a single function during
LTO/bitcode-to-WASM codegen. The fix (below) sidesteps this by using a JPEG
backend with no `setjmp` at all, rather than trying to out-position the
`setjmp`/throw interaction within libjpeg-turbo's control flow.

## The fix: stb_image backend on Emscripten

`grfmt_jpeg.cpp` now has two implementations of `JpegDecoder`/`JpegEncoder`,
selected by `#if defined(__EMSCRIPTEN__)`:
- **Emscripten**: backed by the vendored, header-only `stb_image.h` /
  `stb_image_write.h` (`opencv/3rdparty/stb`, public domain, from
  https://github.com/nothings/stb). stb_image's decoder/encoder use plain
  return-code error handling — **no `setjmp`/`longjmp` anywhere** — so they
  never hit the SjLj+WASM-EH interaction above.
- **Every other platform**: unchanged, still the full libjpeg-turbo
  implementation (the `#else` branch, verbatim).

Wiring:
- `opencv/cmake/OpenCVFindLibsGrfmt.cmake` — on
  `CMAKE_SYSTEM_NAME STREQUAL "Emscripten"`, the `WITH_JPEG` block skips
  `add_subdirectory(3rdparty/libjpeg-turbo)` entirely and just points
  `JPEG_INCLUDE_DIR` at `opencv/3rdparty/stb` with empty `JPEG_LIBRARIES`
  (header-only, nothing to link). `HAVE_JPEG` is still set, so
  `JpegDecoder`/`JpegEncoder` still get compiled and registered in
  `loadsave.cpp` exactly as on every other platform — the swap is fully
  transparent to callers (`cv::imread`/`imdecode`/`imwrite`/`imencode`,
  `CvInvoke.Imread` etc. never know which backend handled a `.jpg`).
- `platforms/emscripten/cmake_configure_dotnet.sh` — `BUILD_JPEG`/`WITH_JPEG`
  are back to `TRUE` (they just need to be true for `grfmt_jpeg.cpp` to
  compile at all; no libjpeg-turbo source is actually built on this
  platform).

**Feature parity note**: the stb_image path is reduced-feature versus
libjpeg-turbo. On encode, only `IMWRITE_JPEG_QUALITY` is honored (no
progressive, optimize, RST interval, luma/chroma quality, or custom sampling
factor — stb_image_write's JPEG writer doesn't support them). There's no
EXIF/XMP/ICC metadata read/write support either. Every other platform is
unaffected — full libjpeg-turbo feature set there.

**Verified** (2026-08-18, via a real `HelloWorld.Blazor` build + Playwright):
- Full link succeeds — no `wasm-ld` crash.
- Decoding a real `.jpg` (`stop-sign.jpg`) produces pixel-exact output,
  cross-checked against a native PIL decode (both corner and center pixels
  matched exactly, BGR vs RGB order accounted for).
- Encoding back to JPEG produces a valid `FF D8 FF`-signed file that
  re-decodes successfully (round-trip).
- The original DNN forward-pass regression check (real YOLO12N model,
  `SetInput`/`Forward`) still passes with the correct `1x84x8400` output
  shape — confirming the codec swap didn't disturb the unrelated
  `-fwasm-exceptions` DNN path.

## Fix attempts tried against libjpeg-turbo itself (both failed — don't repeat)

Before landing on the stb_image swap above, two attempts were made to fix
the crash *within* the libjpeg-turbo-backed implementation:

1. **Hoist the `CV_Error` before `setjmp()`.** Moved the channel-count
   validation (`CV_Error(cv::Error::StsError, ...)` for the `default:` case
   in the channels switch) to occur before the `setjmp` call, keeping
   everything else in one function. Rebuilt `cvextern.a` from scratch,
   cleared `build_dotnet/Emgu.CV.Extern`, and re-ran a full `HelloWorld.Blazor`
   build. **Same crash, same symbol.** Root cause: a second throw site
   (`CV_Check` further down, inside the `else` branch for indirect writes)
   was still reachable inside the setjmp-guarded region and was missed on
   the first pass — but even after addressing that too (see attempt 2), the
   crash persisted, so throw-site positioning alone isn't the fix.

2. **Full function split.** Extracted everything between `setjmp()` and
   `jpeg_finish_compress()` into a new anonymous-namespace helper
   (`jpegEncoderCompress(...)`, all libjpeg calls + no throws — the
   `CV_Check` was provably dead code since `doDirectWrite` is only false when
   `_channels` is already validated to be 3 or 4) marked
   `__attribute__((noinline))`, called from `write()`. `write()` itself kept
   the RAII (`fileWrapper`), the `CV_Error` validation, and the `setjmp`
   dispatch. Verified via `llvm-nm` on the compiled `.o` that the two
   functions really were distinct symbols before the final link (ruling out
   simple inlining). **Still crashed, still anchored to `write()`'s own
   symbol** — i.e. splitting into a genuinely separate, `noinline` function
   was not enough to keep the setjmp+EH interaction out of the top-level
   frame that still has the RAII destructor to run.

Both attempts were fully reverted; the libjpeg-turbo path in
`grfmt_jpeg.cpp` (the `#else` branch today) is untouched from upstream.

## If the stb_image feature gaps ever become a blocker

- Building with a **newer LLVM/Emscripten toolchain** once .NET's wasm-tools
  workload picks one up might fix the underlying libjpeg-turbo crash
  directly, making the stb_image swap unnecessary. Worth periodically
  re-testing the libjpeg-turbo path (flip the `#if defined(__EMSCRIPTEN__)`
  off temporarily, or just try a newer SDK) if full libjpeg-turbo feature
  parity (progressive JPEG, EXIF, etc.) is needed on WASM.
- Filing/searching for an upstream `wasm-ld`/LLVM bug report matching
  "SjLj + wasm-eh in the same function crashes SelectionDAGBuilder" — not
  done; would help confirm whether a newer toolchain actually fixes it.

## How to re-verify after touching this area

1. Change `grfmt_jpeg.cpp` (either branch) and/or CMake JPEG flags.
2. `rm -rf platforms/emscripten/build_dotnet/Emgu.CV.Extern` — CMake's
   dependency tracking for the `cvextern` custom target does **not** see
   changes in its linked OpenCV `.bc` libraries as a reason to re-run its own
   archive/merge step, so without this the rebuild silently reuses a stale
   `cvextern.a`.
3. Re-run `./cmake_configure_dotnet.sh` from `platforms/emscripten/`.
4. `rm -rf Emgu.CV.Example/HelloWorld.Blazor/{obj,bin}` and
   `rm -rf ~/.nuget/packages/emgu.cv.runtime.webassembly/<version>` — the
   globally cached NuGet package can go stale across rebuilds that don't
   bump the package version, silently masking a fresh `cvextern.a`.
5. `dotnet build Emgu.CV.Example/HelloWorld.Blazor/HelloWorld.Blazor.csproj`
   — this is the step that actually exercises real WASM codegen/link; a
   clean `cvextern.a` build alone proves nothing (see "Important" note
   above).
6. Don't stop at "it links" — decode a real `.jpg` and cross-check pixel
   values against a non-WASM decode (e.g. PIL), and re-run a DNN
   `SetInput`/`Forward` pass, before trusting a change in this area.
