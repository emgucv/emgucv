# JPEG codec disabled in WASM builds — investigation notes

**Status as of 2026-08-17: unresolved, JPEG intentionally left disabled.** Read
this before attempting to re-enable `BUILD_JPEG`/`WITH_JPEG` in
`cmake_configure_dotnet.sh` — two independent source-level fixes have already
been tried and both failed identically (details below), so re-run them only
if you have a genuinely new angle, not a variation on the same idea.

## Symptom

With JPEG enabled and `-fwasm-exceptions` on (see
`cmake/EmscriptenBuildFlags.cmake`), the **final link** of any project that
consumes `cvextern.a` (e.g. `HelloWorld.Blazor.csproj`'s `dotnet build`)
crashes `wasm-ld` with a SIGSEGV inside LLVM's SelectionDAG instruction
selector:

```
Running pass 'WebAssembly Instruction Selection' on function '@_ZN2cv11JpegEncoder5writeERKNS_3MatERKNSt3__26vectorIiNS4_9allocatorIiEEEE'
... SIGSEGV ...
llvm::SelectionDAGBuilder::HandlePHINodesInSuccessorBlocks(...)
llvm::SelectionDAGISel::SelectBasicBlock(...)
...
lld::wasm::BitcodeCompiler::compile()
```

The crashing symbol is always `cv::JpegEncoder::write(cv::Mat const&, std::vector<int> const&)`
in `opencv/modules/imgcodecs/src/grfmt_jpeg.cpp`.

**Important**: `cvextern.a` itself builds fine even when this bug is present.
Its own build step archives LLVM bitcode via `llvm-ar rcs` — it never runs
real WASM codegen/instruction-selection. Only the *final* link of a consuming
project (a real `dotnet build` of the Blazor demo, not just the native
`cmake --build`) exercises this path. Treating "cvextern.a built cleanly" as
a green light is a trap — always verify with a full Blazor rebuild.

## Root cause hypothesis

`JpegEncoder::write` is the only imgcodecs encoder function that mixes, in
the same function, **both**:
- a `setjmp(jerr.setjmp_buffer)` call (libjpeg's C error-handling idiom), which
  requires WASM SjLj lowering, and
- a reachable C++ throw/RAII-unwind path (`CV_Error`, `CV_Check`, and the
  `fileWrapper` destructor), which requires WASM-native EH landing pads
  (`-fwasm-exceptions`).

`PngEncoder::write` (`grfmt_png.cpp`) uses the identical
`setjmp(png_jmpbuf(png_ptr))` pattern and RAII (`AutoBuffer`), but has **zero**
throw sites in its body — and it does *not* crash. This asymmetry is the
strongest evidence for the hypothesis: LLVM19's `wasm-ld` (Emscripten SDK
3.1.56 toolchain, bundled with .NET 10 wasm-tools workload) appears to have a
real bug lowering SjLj and WASM-EH together within a single function during
LTO/bitcode-to-WASM codegen. PNG stays enabled; JPEG stays disabled.

## Fix attempts tried (both failed — do not just repeat these)

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

Both attempts were on a scratch branch in the `opencv` submodule
(`fix/wasm-jpeg-encoder-eh-sjlj-crash`, since deleted — held zero unique
commits, it was just checked out at the same commit as `main`/pinned SHA and
never actually committed to) and were fully reverted via
`git checkout -- modules/imgcodecs/src/grfmt_jpeg.cpp`. Do not assume that
branch still exists or has any useful diff.

## What might actually work (untried)

- Building with a **newer LLVM/Emscripten toolchain** once .NET's wasm-tools
  workload picks one up — this smells like a genuine, fixable upstream
  compiler bug rather than an application-level issue.
- Filing/searching for an upstream `wasm-ld`/LLVM bug report matching
  "SjLj + wasm-eh in the same function crashes SelectionDAGBuilder"; if one
  exists, check whether it's fixed in a version newer than 19.
- Trying `-fno-exceptions` scoped narrowly to just `grfmt_jpeg.cpp` (would
  need `CV_Error`/`CV_Check` in that TU to fall back to `abort()`/longjmp
  instead of C++ throw) — not attempted; would change error-reporting
  behavior for JPEG-specific errors and needs careful evaluation before
  trying.
- Reproducing the crash in a **minimal standalone repro** (a tiny .cpp with
  just a setjmp + a throw in one function, compiled through the same
  Emscripten toolchain) to file a precise upstream bug report — not
  attempted; the investigation so far worked directly against the full
  `cvextern.a` build, which is slow to iterate on (~20-25 min per cycle).

## Current state (known-good, what's actually shipped)

`platforms/emscripten/cmake_configure_dotnet.sh`:
```
-DBUILD_JPEG:BOOL=FALSE
-DWITH_JPEG:BOOL=FALSE
-DBUILD_PNG:BOOL=TRUE
-DWITH_PNG:BOOL=TRUE
-DBUILD_TIFF:BOOL=OFF
```
BMP and PNG codecs work and are tested (decode+encode round-trip, and real
YOLO12N DNN forward-pass end-to-end via the `HelloWorld.Blazor` Yolo demo,
output shape `1x84x8400` as expected). TIFF is off but untested/unrelated to
this issue. This matches what's deployed at
`https://emgucv.github.io/emgucv/`.

## How to re-verify after touching this area

1. Change `grfmt_jpeg.cpp` and/or flip `BUILD_JPEG`/`WITH_JPEG` to `TRUE` in
   `cmake_configure_dotnet.sh`.
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
