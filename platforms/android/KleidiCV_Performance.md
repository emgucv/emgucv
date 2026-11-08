# KleidiCV on Android: build configuration and performance

`scripts/build.sh` builds `libcvextern.so` with **KleidiCV on, its SVE2 backend on, and
LTO off**. This is the same configuration as OpenCV's official Android SDK
(`opencv/platforms/android/build_sdk.py`). This page records why, and the benchmark behind
the decision.

## Why this configuration

KleidiCV is Arm's optimized image-processing library. OpenCV uses it as a HAL (hardware
acceleration layer), so it takes over the OpenCV functions it supports, and OpenCV's own
code handles everything else.

| Configuration | KleidiCV | SVE2 paths | LTO | Status |
|---|---|---|---|---|
| Before | Off | — | On | Slowest in KleidiCV-covered functions |
| Tested | On | Off | On | Safe, same speed as the current configuration |
| **Current** | **On** | **On** | **Off** | Matches the official OpenCV build |

KleidiCV compiles its SVE2 code into separate objects (`-march=armv8-a+sve2`) and only calls
them after a runtime CPU check. Devices without SVE2 use its NEON paths, which includes the
emulator and most phones before Armv9.

**LTO must stay off while SVE2 is on.** KleidiCV used to be turned off because, under LTO,
whole-program optimization pulled its SVE2 code into unrelated code (libwebp's
`WebPGetColorPalette`), outside KleidiCV's runtime check. On devices without SVE that code
crashed with SIGILL. Setting `CPU_BASELINE_DISABLE=SVE` doesn't help, because the SVE code comes
from KleidiCV and LTO, not from OpenCV's own CPU baseline or dispatch.

In the current build, a disassembly shows all 36,874 SVE instructions in one block of
KleidiCV-internal code. None are inside a named Emgu CV or OpenCV function. The benchmark ran
every test on a device without SVE2 with no SIGILL.

## Benchmark

**Device:** Samsung Galaxy S10 (SM-G973W), Snapdragon 855, Android 12. The CPU has NEON, FP16
and dot-product, and no SVE or SVE2.

**Method:** `benchmark/kcvbench.c` loads `libcvextern.so` and times each function through
Emgu CV's C API.
- **Images:** 1920×1080 random 8-bit data.
- **Threads:** single-threaded (`cveSetNumThreads(1)`), pinned to one 2.42 GHz Cortex-A76 core
  (`taskset 20`; the shell can't use the 2.84 GHz prime core).
- **Timing:** each function runs 5 warm-up calls, then reports the median of 41 timed calls.
  It reuses one output image throughout, as a camera pipeline would.
- **Rounds:** two rounds per build, alternating builds between rounds. The figures below are
  the better of the two rounds.
- **Error checks:** after the first call, the program checks the library's pending error and
  the output size, so a failing call can't pass for a fast one.

All times are in milliseconds (lower is better). Measured 2026-11-07 on EMGUCV_5_x (OpenCV
5.1.0-based, KleidiCV 26.09, NDK 28.0.12916984).

| Function | KleidiCV off, LTO | KleidiCV on, LTO, no SVE2 | **KleidiCV on, SVE2, no LTO** | Speedup vs off |
|---|---|---|---|---|
| `resize` to 1/2, linear (8UC3) | 3.090 | 0.553 | **0.560** | 5.5× |
| `pyrDown` (8UC1) | 1.592 | 0.577¹ | **0.504** | 3.2× |
| `flip` horizontal (8UC3) | 1.533 | 0.560 | **0.548** | 2.8× |
| `GaussianBlur` 5×5 (8UC1) | 2.685 | 1.023 | **1.014** | 2.6× |
| `resize` to 2×, linear (8UC1) | 4.917 | 2.117 | **2.117** | 2.3× |
| `transpose` (8UC1) | 0.623 | 0.426 | **0.444** | 1.4× |
| `cvtColor` NV21→BGR | 2.906 | 2.187 | **2.187** | 1.3× |
| `erode` 3×3 (8UC1) | 0.852 | 0.863 | **0.679** | 1.3× |
| `medianBlur` 5 (8UC1) | 6.930 | 5.869 | **5.899** | 1.2× |
| `cvtColor` BGR→RGB (8UC3) | 0.564 | 0.486 | **0.479** | 1.2× |
| `cvtColor` BGR→GRAY (8UC3) | 1.237 | 1.235 | **1.236** | 1.0× |
| `Sobel` dx 3×3 → 16S (8UC1) | 1.631 | 1.565 | **1.574** | 1.0× |
| `dilate` 3×3 (8UC1) | 0.865 | 0.874 | **0.876** | 1.0× |
| `warpPerspective` linear (8UC1) | **15.638** | 22.342 | 22.961 | **0.7× (slower)** |

¹ 0.507 ms in an earlier session. The `pyrDown` difference between the two KleidiCV builds is
noise.

**What the results show:**
- **KleidiCV is 2–5.5× faster** for `resize`, `pyrDown`, `flip` and `GaussianBlur`, and
  moderately faster for camera-frame colour conversion (NV21→BGR), `transpose`, `medianBlur`
  and `erode`.
- **`warpPerspective` is about 30% slower** with KleidiCV, in both KleidiCV builds and in
  both rounds. This is KleidiCV's own implementation, not a build-setting effect.
- **No change** for `cvtColor` BGR→GRAY, `Sobel` and `dilate`. Here Carotene (OpenCV's older
  NEON HAL, still in the build) or OpenCV's own NEON code is already about as fast, or
  KleidiCV doesn't take over for these exact parameters.
- **LTO gave no speedup** in any of these functions. `erode` was consistently faster without
  it. The LTO build is only about 2 MB smaller (48.8 MB against 50.9 MB).

**Limits:**
- **Coverage:** one parameter set per function, on random data, single-threaded, on one
  device. Multi-threaded results and other parameters may differ.
- **SVE2:** the S10 has no SVE2, so this doesn't measure KleidiCV's SVE2 paths. Armv9 phones
  (2022 onward) may gain more.
- **Round-to-round variation:** up to 15% on individual results, so differences of a few
  percent aren't meaningful.

## Reproducing

1. Build `libcvextern.so` with the configuration you want to test (`scripts/build.sh`). Copy
   it out of `libs/android/arm64-v8a/`, together with `libc++_shared.so`, before building the
   next configuration.
2. Compile the benchmark with the NDK:
   ```bash
   $ANDROID_NDK/toolchains/llvm/prebuilt/darwin-x86_64/bin/aarch64-linux-android24-clang \
       -O2 -Wall -o kcvbench benchmark/kcvbench.c -ldl
   ```
3. Push and run it on the device, pinned to one performance core. Check which cores the
   shell may use with `adb shell taskset <mask> true`, because Android's CPU sets change at
   run time:
   ```bash
   adb shell mkdir -p /data/local/tmp/kcvbench/lib
   adb push kcvbench /data/local/tmp/kcvbench/
   adb push libcvextern.so libc++_shared.so /data/local/tmp/kcvbench/lib/
   adb shell "cd /data/local/tmp/kcvbench && LD_LIBRARY_PATH=lib taskset 20 ./kcvbench lib/libcvextern.so 1"
   ```
   The first output line shows the library's `Custom HAL` setting, which confirms whether
   KleidiCV is in the build being measured.

The benchmark uses OpenCV 5's type encoding (`CV_CN_SHIFT` = 5, so `CV_8UC3` = 64). Update it
if the encoding changes again.

## Windows build script

`scripts/build.ps1` (building Android from Windows) uses the same configuration as
`scripts/build.sh`. It was switched at the same time but hasn't been benchmarked or
built-tested separately.
