#!/bin/bash
# Build cvextern.a for Unity WebGL using the Emscripten toolchain bundled
# with a Unity Editor installation (PlaybackEngines/WebGLSupport/BuildTools).
#
# This is a SEPARATE toolchain from the one platforms/emscripten/ uses for
# the .NET/Blazor WebAssembly build: Unity bundles its own pinned Emscripten
# version (e.g. 3.1.39 / LLVM 17 for Unity 6000.3), which is not ABI
# compatible with the .NET SDK's Emscripten workload (3.1.56 / LLVM 19).
# The two builds must never share an output file -- this script writes to
# libs/unity-webgl/cvextern.a, distinct from libs/webgl/cvextern.a.
#
# Currently mini-only (no opencv_contrib, no dnn/calib/photo/features/video)
# to fit WebGL's tighter download-size budget -- see the module list below.
#
# Usage:
#   cd platforms/unity/webgl
#   ./cmake_configure_unity.sh                      # auto-detect newest installed Unity Editor
#   ./cmake_configure_unity.sh 6000.3.22f1           # specific Editor version

set -e
cd "$(dirname "$0")"
REPO_ROOT="$(cd "$PWD/../../.."; pwd)"

# ---------------------------------------------------------------------------
# Locate the Unity Editor and its bundled Emscripten toolchain
# ---------------------------------------------------------------------------
UNITY_EDITOR_ROOT="/Applications/Unity/Hub/Editor"
if [ -n "$1" ]; then
    EDITOR_VERSION="$1"
else
    EDITOR_VERSION=$(ls "$UNITY_EDITOR_ROOT" 2>/dev/null | sort -V | tail -1)
fi
if [ -z "$EDITOR_VERSION" ] || [ ! -d "$UNITY_EDITOR_ROOT/$EDITOR_VERSION" ]; then
    echo "ERROR: Could not find a Unity Editor installation under $UNITY_EDITOR_ROOT"
    echo "Install one first, e.g.:  unity install lts && unity install-modules -e <version> -m webgl"
    exit 1
fi

EMDIR="$UNITY_EDITOR_ROOT/$EDITOR_VERSION/PlaybackEngines/WebGLSupport/BuildTools/Emscripten"
if [ ! -f "$EMDIR/emscripten/emcc" ]; then
    echo "ERROR: Emscripten toolchain not found at $EMDIR"
    echo "The WebGL module may not be installed for Editor $EDITOR_VERSION:"
    echo "  unity install-modules -e $EDITOR_VERSION -m webgl"
    exit 1
fi

echo "Using Unity Editor: $EDITOR_VERSION"
echo "Using Emscripten SDK: $EMDIR"

export EM_CONFIG="$EMDIR/.emscripten"
export PATH="$EMDIR/emscripten:$EMDIR/llvm:$EMDIR/node:$PATH"

EMCC="$EMDIR/emscripten/emcc"
EMCMAKE="$EMDIR/emscripten/emcmake"
EMMAKE="$EMDIR/emscripten/emmake"
LLVM_AR="$EMDIR/llvm/llvm-ar"
EMSCRIPTEN_TOOLCHAIN="$EMDIR/emscripten/cmake/Modules/Platform/Emscripten.cmake"

"$EMCC" --version | head -1

# ---------------------------------------------------------------------------
# Configure -- mini variant (matches platforms/emscripten's "mini" build).
# flann stays enabled: in OpenCV 5, imgproc depends on geometry, which
# depends on flann.
#
# PNG is disabled here (unlike the Blazor mini build, which keeps it): Unity's
# own WebGL player runtime statically links its own libpng (for
# Texture2D.LoadImage), and cvextern.a bringing in a second copy causes
# "wasm-ld: duplicate symbol: png_get_uint_32" etc. at Unity's final link
# step. Decode PNGs via Texture2D + TextureConvert instead of
# CvInvoke.Imread/Imwrite on this platform.
# ---------------------------------------------------------------------------
BUILD_DIR="$PWD/build_unity"
mkdir -p "$BUILD_DIR"
cd "$BUILD_DIR"

"$EMCMAKE" cmake \
    -DCMAKE_TOOLCHAIN_FILE="$EMSCRIPTEN_TOOLCHAIN" \
    -DCMAKE_INTERPROCEDURAL_OPTIMIZATION:BOOL=FALSE \
    -DCMAKE_POSITION_INDEPENDENT_CODE:BOOL=TRUE \
    -DBUILD_TESTS:BOOL=FALSE \
    -DBUILD_PERF_TESTS:BOOL=FALSE \
    -DBUILD_opencv_apps:BOOL=FALSE \
    -DBUILD_DOCS:BOOL=FALSE \
    -DWITH_TBB:BOOL=TRUE \
    -DWITH_CUDA:BOOL=FALSE \
    -DWITH_IPP:BOOL=FALSE \
    -DWITH_EIGEN:BOOL=TRUE \
    -DEIGEN_INCLUDE_PATH="$REPO_ROOT/eigen" \
    -DOPENCV_EXTRA_MODULES_PATH:STRING= \
    -DBUILD_opencv_calib:BOOL=FALSE \
    -DBUILD_opencv_dnn:BOOL=FALSE \
    -DBUILD_opencv_photo:BOOL=FALSE \
    -DBUILD_opencv_features:BOOL=FALSE \
    -DBUILD_opencv_video:BOOL=FALSE \
    -DBUILD_opencv_ts:BOOL=FALSE \
    -DBUILD_opencv_java:BOOL=FALSE \
    -DBUILD_opencv_python2:BOOL=FALSE \
    -DBUILD_opencv_python3:BOOL=FALSE \
    -DBUILD_SHARED_LIBS:BOOL=FALSE \
    -DCMAKE_BUILD_TYPE:STRING=Release \
    -DCMAKE_CXX_STANDARD:STRING=17 \
    -DBUILD_ITT:BOOL=FALSE \
    -DCV_ENABLE_INTRINSICS:BOOL=FALSE \
    -DWITH_OPENCL:BOOL=OFF \
    -DBUILD_JPEG:BOOL=TRUE \
    -DWITH_JPEG:BOOL=TRUE \
    -DBUILD_PNG:BOOL=FALSE \
    -DWITH_PNG:BOOL=FALSE \
    -DBUILD_TIFF:BOOL=OFF \
    -DWITH_TIFF:BOOL=OFF \
    -DEMGU_CV_WITH_TIFF:BOOL=OFF \
    -DEMGU_CV_WITH_TESSERACT:BOOL=FALSE \
    -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE \
    -DWITH_PTHREADS_PF:BOOL=OFF \
    -DEMGU_CV_WITH_DEPTHAI:BOOL=OFF \
    -DEMGU_CV_EMSCRIPTEN_LLVM_AR_PATH="$LLVM_AR" \
    -DEMGU_CV_EMSCRIPTEN_OUTPUT_SUFFIX="" \
    -DEMGU_CV_EMSCRIPTEN_OUTPUT_DIR="unity-webgl" \
    -DEMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS:BOOL=FALSE \
    "$REPO_ROOT"

# ---------------------------------------------------------------------------
# Build cvextern only (not the .NET NuGet packaging target -- Unity
# consumes the .a directly via a Plugins/WebGL/ folder drop, see
# Emgu.CV.Unity/README.md).
# ---------------------------------------------------------------------------
"$EMMAKE" make -j"$(sysctl -n hw.ncpu 2>/dev/null || nproc)" cvextern VERBOSE=1
