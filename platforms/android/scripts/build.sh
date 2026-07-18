#!/bin/bash
# Build Emgu CV native library for Android on macOS.
#
# Usage:
#   cd platforms/android/scripts
#   ./build.sh [abi] [variant] [toolchain]
#
# ABI ($1):      arm64-v8a (default), x86_64, x86, armeabi-v7a
# variant ($2):  (empty) / full — all modules + contrib + Tesseract
#                core            — no contrib, no Tesseract, no Freetype
#                mini            — minimal modules, no contrib/Tesseract/Freetype
# toolchain ($3): optional Android toolchain name (passed as ANDROID_TOOLCHAIN_NAME)
#
# Required environment (set by maccfg.sh or the caller):
#   ANDROID_NDK  — path to Android NDK (e.g. ~/Library/Android/sdk/ndk/28.0.12916984)
#   CMAKE        — path to cmake executable (auto-detected from Homebrew / system if unset)
#   MAKE         — path to make or ninja (auto-detected if unset)
#
# To install the NDK on macOS, run:
#   platforms/android/scripts/install_ndk_macos.sh

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
SOURCE_DIR="$(cd "$SCRIPT_DIR/../../.." && pwd)"

ANDROID_ABI="${1:-arm64-v8a}"
VARIANT="${2:-}"
ANDROID_TOOLCHAIN_NAME="${3:-}"

ANDROID_NATIVE_API_LEVEL=24

# Load macOS path configuration
# shellcheck source=maccfg.sh
source "$SCRIPT_DIR/maccfg.sh" "$ANDROID_ABI"

echo "ANDROID_NDK: ${ANDROID_NDK:-<not found>}"
echo "CMAKE:       ${CMAKE:-<not found>}"
echo "MAKE:        ${MAKE:-<not found>}"

# ---- Validate prerequisites ----
if [ -z "$ANDROID_NDK" ] || [ ! -d "$ANDROID_NDK" ]; then
    echo ""
    echo "ERROR: ANDROID_NDK is not set or does not exist: ${ANDROID_NDK:-<unset>}"
    echo "  Install the NDK:  platforms/android/scripts/install_ndk_macos.sh"
    echo "  Then set:         export ANDROID_NDK=<path-to-ndk>"
    exit 1
fi

if [ -z "$CMAKE" ] || [ ! -x "$CMAKE" ]; then
    echo ""
    echo "ERROR: cmake not found.  Install with:  brew install cmake"
    exit 1
fi

if [ -z "$MAKE" ] || [ ! -x "$MAKE" ]; then
    echo ""
    echo "ERROR: make/ninja not found.  Install with:  brew install ninja"
    exit 1
fi

CMAKE_TOOLCHAIN_FILE="$ANDROID_NDK/build/cmake/android.toolchain.cmake"
if [ ! -f "$CMAKE_TOOLCHAIN_FILE" ]; then
    echo "ERROR: Android toolchain not found: $CMAKE_TOOLCHAIN_FILE"
    exit 1
fi

# Ninja vs. Unix Makefiles
if echo "$MAKE" | grep -qi ninja; then
    CMAKE_GENERATOR="Ninja"
else
    CMAKE_GENERATOR="Unix Makefiles"
fi

CPU_COUNT=$(sysctl -n hw.logicalcpu 2>/dev/null || nproc 2>/dev/null || echo 4)
BUILD_DIR="android_${ANDROID_ABI}"
INSTALL_FOLDER="$SOURCE_DIR/$BUILD_DIR/install"

# ---- Variant configuration ----
TESSERACT_OPTION="-DEMGU_CV_WITH_TESSERACT:BOOL=ON"
BUILD_CONTRIB=1
OPTIONAL_FLAGS=()

case "$VARIANT" in
    core)
        TESSERACT_OPTION="-DEMGU_CV_WITH_TESSERACT:BOOL=OFF"
        OPTIONAL_FLAGS+=("-DEMGU_CV_WITH_FREETYPE:BOOL=OFF")
        BUILD_CONTRIB=0
        ;;
    mini)
        TESSERACT_OPTION="-DEMGU_CV_WITH_TESSERACT:BOOL=OFF"
        OPTIONAL_FLAGS+=("-DEMGU_CV_WITH_FREETYPE:BOOL=OFF")
        BUILD_CONTRIB=0
        # flann must stay enabled: in OpenCV 5 imgproc depends on geometry, which depends on flann
        OPTIONAL_FLAGS+=(
            "-DBUILD_opencv_calib:BOOL=FALSE"
            "-DBUILD_opencv_dnn:BOOL=FALSE"
            "-DBUILD_opencv_photo:BOOL=FALSE"
            "-DBUILD_opencv_features:BOOL=FALSE"
            "-DBUILD_opencv_video:BOOL=FALSE"
        )
        ;;
esac

# ---- Base CMake flags shared by all sub-builds (eigen, freetype, harfbuzz, main) ----
BASE_CMAKE_FLAGS=(
    -G "$CMAKE_GENERATOR"
    "-DANDROID_ABI=$ANDROID_ABI"
    "-DANDROID_PLATFORM=$ANDROID_NATIVE_API_LEVEL"
    "-DCMAKE_TOOLCHAIN_FILE=$CMAKE_TOOLCHAIN_FILE"
    "-DCMAKE_MAKE_PROGRAM=$MAKE"
    "-DCMAKE_C_FLAGS:STRING=-std=c11"
    "-DCMAKE_CXX_FLAGS_RELEASE:STRING=-g0 -O3"
    "-DCMAKE_C_FLAGS_RELEASE:STRING=-g0 -O3"
    # Disable KleidiCV (ARM's optimized HAL, on by default for arm64 in
    # OpenCV 5.0). It is the only component that compiles objects with
    # -march=armv8-a+sve2, and under the whole-program (LTO) build that
    # SVE2 codegen leaks into libwebp's WebPGetColorPalette. SVE is absent
    # on the Android emulator and most real devices, so those instructions
    # make libcvextern.so crash with SIGILL. Turning KleidiCV off removes
    # every +sve2 object, so no SVE can be emitted anywhere. (Note:
    # CPU_BASELINE_DISABLE=SVE does NOT help here — the SVE comes from
    # KleidiCV/LTO, not OpenCV's own CPU baseline/dispatch.)
    "-DWITH_KLEIDICV=OFF"
    "-DCMAKE_SHARED_LINKER_FLAGS:STRING=-Wl,--gc-sections, -Wl,--exclude-libs,All"
    "-DCMAKE_POLICY_DEFAULT_CMP0069=NEW"
    "-DCMAKE_INTERPROCEDURAL_OPTIMIZATION:BOOL=ON"
    "-DCMAKE_INSTALL_PREFIX:STRING=$INSTALL_FOLDER"
    "-DCMAKE_BUILD_TYPE:STRING=Release"
)
if [ -n "$ANDROID_TOOLCHAIN_NAME" ]; then
    BASE_CMAKE_FLAGS+=("-DANDROID_TOOLCHAIN_NAME=$ANDROID_TOOLCHAIN_NAME")
fi

# ---- Build Eigen ----
echo ""
echo "=== Building Eigen ==="
cd "$SOURCE_DIR/eigen"
mkdir -p "$BUILD_DIR"
cd "$BUILD_DIR"
"$CMAKE" "${BASE_CMAKE_FLAGS[@]}" ..
EIGEN_DIR="$PWD"
"$CMAKE" --build . --config Release --target install
cd "$SOURCE_DIR"

# ---- Build FreetType2 and HarfBuzz (full build only) ----
CONTRIB_CMAKE_FLAGS=()
if [ "$BUILD_CONTRIB" -eq 1 ]; then
    echo ""
    echo "=== Building freetype2 ==="
    cd "$SOURCE_DIR/3rdParty/freetype2"
    mkdir -p "$BUILD_DIR"
    cd "$BUILD_DIR"
    "$CMAKE" "${BASE_CMAKE_FLAGS[@]}" -DCMAKE_POLICY_VERSION_MINIMUM=3.5 ..
    "$CMAKE" --build . --config Release --parallel --target install
    cd "$SOURCE_DIR"

    echo ""
    echo "=== Building harfbuzz ==="
    cd "$SOURCE_DIR/harfbuzz"
    mkdir -p "$BUILD_DIR"
    cd "$BUILD_DIR"
    "$CMAKE" "${BASE_CMAKE_FLAGS[@]}" \
        -DCMAKE_POLICY_VERSION_MINIMUM=3.5 \
        "-DCMAKE_FIND_ROOT_PATH:STRING=$INSTALL_FOLDER" \
        "-DHB_HAVE_FREETYPE:BOOL=TRUE" \
        "-DCMAKE_CXX_FLAGS=-Wno-cast-function-type-strict" \
        ..
    "$CMAKE" --build . --config Release --parallel --target install
    cd "$SOURCE_DIR"

    CONTRIB_CMAKE_FLAGS+=("-DOPENCV_EXTRA_MODULES_PATH:String=$SOURCE_DIR/opencv_contrib/modules")
else
    CONTRIB_CMAKE_FLAGS+=("-DEMGU_CV_WITH_FREETYPE:BOOL=OFF")
fi

# ---- Build main Emgu CV ----
echo ""
echo "=== Building Emgu CV (ABI=$ANDROID_ABI, variant=${VARIANT:-full}) ==="
mkdir -p "$SOURCE_DIR/$BUILD_DIR"
cd "$SOURCE_DIR/$BUILD_DIR"

"$CMAKE" "${BASE_CMAKE_FLAGS[@]}" \
    "$TESSERACT_OPTION" \
    "${OPTIONAL_FLAGS[@]}" \
    "${CONTRIB_CMAKE_FLAGS[@]}" \
    -DBUILD_SHARED_LIBS:BOOL=OFF \
    -DBUILD_ANDROID_EXAMPLES:BOOL=OFF \
    -DBUILD_PERF_TESTS:BOOL=OFF \
    -DPARALLEL_ENABLE_PLUGINS:BOOL=OFF \
    -DVIDEOIO_ENABLE_PLUGINS:BOOL=OFF \
    -DHIGHGUI_ENABLE_PLUGINS:BOOL=OFF \
    -DWITH_IPP:BOOL=OFF \
    -DBUILD_DOCS:BOOL=OFF \
    -DBUILD_TESTS:BOOL=OFF \
    -DBUILD_WITH_DEBUG_INFO:BOOL=OFF \
    -DBUILD_opencv_java:BOOL=OFF \
    -DBUILD_opencv_java_bindings_generator:BOOL=OFF \
    -DBUILD_opencv_ts:BOOL=OFF \
    -DWITH_ITT:BOOL=OFF \
    -DWITH_OPENCL:BOOL=ON \
    -DWITH_CUDA:BOOL=OFF \
    -DBUILD_ANDROID_PROJECTS=OFF \
    -DWITH_EIGEN:BOOL=ON \
    -DBUILD_FAT_JAVA_LIB:BOOL=FALSE \
    -DBUILD_JAVA:BOOL=FALSE \
    -DEMGU_CV_WITH_DEPTHAI:BOOL=FALSE \
    "-DCMAKE_FIND_ROOT_PATH:STRING=$INSTALL_FOLDER" \
    "-DEigen3_DIR:STRING=$EIGEN_DIR" \
    "$SOURCE_DIR"

echo ""
echo "Building native libs..."
if echo "$MAKE" | grep -qi ninja; then
    "$MAKE" -j "$CPU_COUNT"
else
    "$MAKE" -j "$CPU_COUNT" VERBOSE=1 package
fi

echo ""
echo "=== Build complete: $BUILD_DIR ==="
