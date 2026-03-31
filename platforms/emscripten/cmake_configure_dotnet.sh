#!/bin/bash
# Build libcvextern.bc for Blazor/WASM using the Emscripten toolchain that
# ships with the .NET SDK WASM workload (Microsoft.NET.Runtime.Emscripten.*).
#
# This produces libs/webgl/libcvextern.bc which is linked by
# Emgu.CV.Example/HelloWorld.Blazor via NativeFileReference.
#
# Usage:
#   cd platforms/emscripten
#   ./cmake_configure_dotnet.sh

set -e
cd "$(dirname "$0")"

# ---------------------------------------------------------------------------
# Locate the .NET SDK's Emscripten pack
# ---------------------------------------------------------------------------
DOTNET_PACKS="${DOTNET_ROOT:-$HOME/.dotnet}/packs"

EMSCRIPTEN_SDK_ROOT=$(find "$DOTNET_PACKS" \
    -maxdepth 5 \
    -name "emcc" \
    -path "*/Microsoft.NET.Runtime.Emscripten.*.Sdk.*" \
    2>/dev/null | head -1 | xargs -I{} dirname {})

if [ -z "$EMSCRIPTEN_SDK_ROOT" ]; then
    echo "ERROR: Could not find the .NET Emscripten SDK under $DOTNET_PACKS"
    echo "Install the WASM workload:  dotnet workload install wasm-tools"
    exit 1
fi

echo "Using Emscripten SDK: $EMSCRIPTEN_SDK_ROOT"

EMSCRIPTEN_TOOLCHAIN="$EMSCRIPTEN_SDK_ROOT/cmake/Modules/Platform/Emscripten.cmake"
EMCC="$EMSCRIPTEN_SDK_ROOT/emcc"
EMCMAKE="$EMSCRIPTEN_SDK_ROOT/emcmake"
EMMAKE="$EMSCRIPTEN_SDK_ROOT/emmake"

# Locate the LLVM/Node/Binaryen roots that ship alongside the SDK pack
EMSCRIPTEN_PACK_ROOT="$(dirname "$(dirname "$EMSCRIPTEN_SDK_ROOT")")"   # .../10.0.5
DOTNET_PACK_VERSION_DIR="$(dirname "$EMSCRIPTEN_PACK_ROOT")"             # .../Sdk.linux-x64
DOTNET_PACKS_DIR="$(dirname "$DOTNET_PACK_VERSION_DIR")"                 # ~/.dotnet/packs

# LLVM and Binaryen ship in the Sdk pack's tools/bin.
# emcc -flto -r needs 'llc' which is absent from the dotnet pack.
# Create a shim dir that re-exports every dotnet LLVM tool plus a 'llc' symlink
# pointing at the system llc (same major version as the dotnet clang).
LLVM_BIN="$EMSCRIPTEN_PACK_ROOT/tools/bin"
LLVM_SHIM_DIR="$PWD/build_dotnet/.llvm-shim"
mkdir -p "$LLVM_SHIM_DIR"
for f in "$LLVM_BIN"/*; do
    ln -sf "$f" "$LLVM_SHIM_DIR/$(basename "$f")" 2>/dev/null || true
done
# Find a system llc whose major version matches the dotnet clang (19)
DOTNET_CLANG_VER=$("$LLVM_BIN/clang" --version 2>&1 | sed -n 's/clang version \([0-9]*\).*/\1/p')
if [ -z "$DOTNET_CLANG_VER" ]; then DOTNET_CLANG_VER=19; fi
if [ -x "/usr/bin/llc-${DOTNET_CLANG_VER}" ]; then
    ln -sf "/usr/bin/llc-${DOTNET_CLANG_VER}" "$LLVM_SHIM_DIR/llc"
elif [ -x "/usr/bin/llc" ]; then
    ln -sf "/usr/bin/llc" "$LLVM_SHIM_DIR/llc"
fi
export DOTNET_EMSCRIPTEN_LLVM_ROOT="$LLVM_SHIM_DIR"
export DOTNET_EMSCRIPTEN_BINARYEN_ROOT="$EMSCRIPTEN_PACK_ROOT/tools"

# Node ships in a sibling Node pack (binary is 5 levels deep under packs/)
NODE_BIN=$(find "$DOTNET_PACKS_DIR" -maxdepth 6 -name "node" \
    -path "*/Microsoft.NET.Runtime.Emscripten.*.Node.*" 2>/dev/null | head -1)
if [ -n "$NODE_BIN" ]; then
    export DOTNET_EMSCRIPTEN_NODE_JS="$NODE_BIN"
else
    export DOTNET_EMSCRIPTEN_NODE_JS="$(which node)"
fi

# Use a Linux-native cache dir (not NTFS) to avoid WSL2 NTFS issues where
# rm -rf silently fails to remove files, causing cp to fail when the dotnet
# Cache pack has __string/__tuple as directories but the old cache has them
# as files.  $HOME is on ext4 so operations are reliable.
LINUX_EM_CACHE="$HOME/.emscripten_dotnet_cache"
export EM_CACHE="$LINUX_EM_CACHE"

# Seed the Linux-native cache from the dotnet pre-built Cache pack on first use.
# The dotnet Cache pack has pre-compiled sysroot libs (libc, libopenal, etc.) that
# cannot be rebuilt from scratch due to bugs in the emscripten 3.1.56 source
# (e.g. al.c calls emscripten_errf without declaring it).
DOTNET_CACHE_PACK=$(find "$DOTNET_PACKS_DIR" \
    -maxdepth 7 \
    -type d \
    -name "cache" \
    -path "*/Microsoft.NET.Runtime.Emscripten.*.Cache.*" \
    2>/dev/null | head -1)
if [ -n "$DOTNET_CACHE_PACK" ] && \
   [ ! -f "$EM_CACHE/sysroot/lib/wasm32-emscripten/libc.a" ]; then
    echo "Seeding Linux EM_CACHE from dotnet Cache pack: $DOTNET_CACHE_PACK"
    rm -rf "$EM_CACHE"
    cp -r "$DOTNET_CACHE_PACK" "$EM_CACHE"
fi

mkdir -p "$EM_CACHE"

# FROZEN_CACHE must be an empty string so Python's bool() evaluates it as False.
# (bool('0') == True in Python, so "0" does NOT work here.)
export FROZEN_CACHE=""

# Skip sanity checks during the parallel make so emcc does not clear the sysroot
# that the warmup step just built.
export EM_IGNORE_SANITY=1

echo "  LLVM_ROOT:    $DOTNET_EMSCRIPTEN_LLVM_ROOT"
echo "  BINARYEN_ROOT:$DOTNET_EMSCRIPTEN_BINARYEN_ROOT"
echo "  NODE_JS:      $DOTNET_EMSCRIPTEN_NODE_JS"
echo "  EM_CACHE:     $EM_CACHE"
echo "  FROZEN_CACHE: '${FROZEN_CACHE}' (empty = False in Python)"

# ---------------------------------------------------------------------------
# Configure
# ---------------------------------------------------------------------------
REPO_ROOT="$(cd "$PWD/../.."; pwd)"
BUILD_DIR="$PWD/build_dotnet"

mkdir -p "$BUILD_DIR"
cd "$BUILD_DIR"

"$EMCC" --version

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
    -DWITH_EIGEN:BOOL=FALSE \
    -DOPENCV_EXTRA_MODULES_PATH="$REPO_ROOT/opencv_contrib/modules" \
    -DBUILD_opencv_ts:BOOL=FALSE \
    -DBUILD_opencv_java:BOOL=FALSE \
    -DBUILD_opencv_python2:BOOL=FALSE \
    -DBUILD_opencv_python3:BOOL=FALSE \
    -DBUILD_SHARED_LIBS:BOOL=FALSE \
    -DCMAKE_BUILD_TYPE:STRING=Release \
    -DCMAKE_CXX_STANDARD:STRING=11 \
    -DBUILD_ITT:BOOL=FALSE \
    -DCV_ENABLE_INTRINSICS:BOOL=FALSE \
    -DWITH_OPENCL:BOOL=OFF \
    -DBUILD_JPEG:BOOL=TRUE \
    -DBUILD_PNG:BOOL=TRUE \
    -DBUILD_TIFF:BOOL=OFF \
    -DWITH_TIFF:BOOL=OFF \
    -DEMGU_CV_WITH_TIFF:BOOL=OFF \
    -DEMGU_CV_WITH_TESSERACT:BOOL=FALSE \
    -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE \
    -DWITH_PTHREADS_PF:BOOL=OFF \
    -DEMGU_CV_WITH_DEPTHAI:BOOL=OFF \
    "$REPO_ROOT"

# ---------------------------------------------------------------------------
# Warm up the emscripten sysroot before parallel compilation.
# emcc clears the cache on config change; a single serial compile forces it to
# fully rebuild the sysroot before the parallel make jobs start.
# ---------------------------------------------------------------------------
echo "Warming up emscripten sysroot..."
echo 'int main(){return 0;}' > /tmp/em_warmup.c
"$EMCC" /tmp/em_warmup.c -o /tmp/em_warmup.js
rm -f /tmp/em_warmup.c /tmp/em_warmup.js
echo "Sysroot ready."

# ---------------------------------------------------------------------------
# Build cvextern
# ---------------------------------------------------------------------------
"$EMMAKE" make cvextern -j$(nproc) VERBOSE=1

# ---------------------------------------------------------------------------
# Link all bitcode into a single libcvextern.bc
# ---------------------------------------------------------------------------
# Use llvm-link instead of "emcc -r" because emcc 3.1.56 -r produces a wasm
# relocatable object rather than LLVM bitcode. llvm-link merges all bitcode
# (OpenCV modules, 3rdparty libs, and cvextern wrapper objects) into a single
# LLVM IR bitcode file that the Blazor NativeFileReference link step expects.
cd "$REPO_ROOT"

"$LLVM_SHIM_DIR/llvm-link" \
    -o libs/webgl/libcvextern.bc \
    platforms/emscripten/build_dotnet/bin/webgl/*.bc \
    platforms/emscripten/build_dotnet/opencv/3rdparty/lib/*.bc \
    platforms/emscripten/build_dotnet/Emgu.CV.Extern/CMakeFiles/cvextern.dir/**/*.o \
    platforms/emscripten/build_dotnet/Emgu.CV.Extern/CMakeFiles/cvextern.dir/*.o

echo ""
echo "Done. Output: libs/webgl/libcvextern.bc"
ls -lh libs/webgl/libcvextern.bc
