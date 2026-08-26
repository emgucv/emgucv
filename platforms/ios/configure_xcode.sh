#!/usr/bin/env bash -v
set -e
set -x
CURRENT_SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"


if [ "$1" == "core" ]; then
    CV_CONTRIB_OPTION=( -DOPENCV_EXTRA_MODULES_PATH:STRING= -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE -DEMGU_CV_WITH_TESSERACT:BOOL=FALSE )
elif [ "$1" == "mini" ]; then
    # flann must stay enabled: in OpenCV 5 imgproc depends on geometry, which depends on flann.
    CV_CONTRIB_OPTION=( -DOPENCV_EXTRA_MODULES_PATH:STRING= -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE -DEMGU_CV_WITH_TESSERACT:BOOL=FALSE -DBUILD_opencv_calib:BOOL=FALSE -DBUILD_opencv_dnn:BOOL=FALSE -DBUILD_opencv_photo:BOOL=FALSE -DBUILD_opencv_features:BOOL=FALSE -DBUILD_opencv_video:BOOL=FALSE )
else
    CV_CONTRIB_OPTION=( -DOPENCV_EXTRA_MODULES_PATH:STRING=$CURRENT_SCRIPT_DIR/../../opencv_contrib/modules -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE -DEMGU_CV_WITH_TESSERACT:BOOL=TRUE )
fi

#-DHB_HAVE_FREETYPE:BOOL=TRUE -DHB_BUILD_TESTS:BOOL=FALSE
INSTALL_FOLDER=$PWD/build/install


if [ "$2" == "device" ]; then
    CV_TOOLCHAIN_OPTION=( -DCMAKE_TOOLCHAIN_FILE=$CURRENT_SCRIPT_DIR/cmake/Toolchains/Toolchain-iPhoneOS_Xcode.cmake )
    CV_OPTIMIZATION_OPTION=
    INSTALL_FOLDER=$INSTALL_FOLDER/OpenCV_iPhoneOS_$3
    IPHONEOS_DEPLOYMENT_TARGET=14.2 
elif [ "$2" == "catalyst" ]; then
    CV_TOOLCHAIN_OPTION=( -DAPPLE_FRAME_WORK:BOOL=TRUE -DCMAKE_TOOLCHAIN_FILE=$CURRENT_SCRIPT_DIR/cmake/Toolchains/Toolchain-Catalyst_Xcode.cmake )
    CV_OPTIMIZATION_OPTION=
    INSTALL_FOLDER=$INSTALL_FOLDER/OpenCV_catalyst_$3
    IPHONEOS_DEPLOYMENT_TARGET=15.0
else #simulator
    CV_TOOLCHAIN_OPTION=( -DCMAKE_TOOLCHAIN_FILE=$CURRENT_SCRIPT_DIR/cmake/Toolchains/Toolchain-iPhoneSimulator_Xcode.cmake )
    #disable optimization for ios simulator. It is causing compilation issue when NEON is enabled for Apple Arm64.
    CV_OPTIMIZATION_OPTION=( -DCV_DISABLE_OPTIMIZATION:BOOL=TRUE )
    INSTALL_FOLDER=$INSTALL_FOLDER/OpenCV_iPhoneSimulator_$3
    IPHONEOS_DEPLOYMENT_TARGET=14.2
fi

#need to set it as an enviroment variable for 4.9.0 release
export IPHONEOS_DEPLOYMENT_TARGET=$IPHONEOS_DEPLOYMENT_TARGET

INSTALL_PREFIX_OPTION=( -DCMAKE_INSTALL_PREFIX=$INSTALL_FOLDER )

# CMAKE_XCODE_ATTRIBUTE_ bakes the deployment target into the generated
# pbxproj. Without it the target only travels via xcodebuild_wrapper's command
# line, and build.sh invokes xcodebuild directly, so Xcode fell back to the SDK
# default (e.g. minos 26.2) and clang emitted iOS16+-only objc_release_x*
# thunks that crash on iOS 15 at launch (issue #1016).
CMAKE_COMMON_OPTION=( -DIPHONEOS_DEPLOYMENT_TARGET:STRING="$IPHONEOS_DEPLOYMENT_TARGET" -DCMAKE_XCODE_ATTRIBUTE_IPHONEOS_DEPLOYMENT_TARGET:STRING="$IPHONEOS_DEPLOYMENT_TARGET" -DIOS_ARCH="$3" -DCMAKE_XCODE_ATTRIBUTE_CLANG_CXX_LANGUAGE_STANDARD="c++23" -DCMAKE_XCODE_ATTRIBUTE_CLANG_CXX_LIBRARY="libc++" -DCMAKE_FIND_ROOT_PATH:STRING="$INSTALL_FOLDER" -DBUILD_opencv_js_bindings_generator:BOOL=FALSE -DBUILD_opencv_python_tests:BOOL=FALSE -DBUILD_opencv_freetype:BOOL=FALSE -DWITH_OPENCL:BOOL=FALSE )

if [ "$2" == "catalyst" ]; then
    #CV_CONTRIB_OPTION+=( -DBUILD_JPEG:BOOL=FALSE -DBUILD_OPENJPEG:BOOL=FALSE -DWITH_JPEG:BOOL=FALSE )
    MACOS_SDK_PATH="$(xcrun --sdk macosx --show-sdk-path)"
    CMAKE_CV_COMPILER_FLAGS="-target $3-apple-ios15.0-macabi -isysroot $MACOS_SDK_PATH -iframework $MACOS_SDK_PATH/System/iOSSupport/System/Library/Frameworks -isystem $MACOS_SDK_PATH/System/iOSSupport/usr/include"
    CMAKE_COMMON_OPTION+=( -DCMAKE_SYSTEM_NAME=Darwin -DCMAKE_C_FLAGS:STRING="$CMAKE_CV_COMPILER_FLAGS" -DCMAKE_CXX_FLAGS:STRING="$CMAKE_CV_COMPILER_FLAGS" -DCMAKE_EXE_LINKER_FLAGS:STRING="$CMAKE_CV_COMPILER_FLAGS" -DSWIFT_DISABLED:BOOL=TRUE -DIOS:BOOL=TRUE -DMAC_CATALYST:BOOL=TRUE -DWITH_OPENCL:BOOL=FALSE -DCMAKE_OSX_ARCHITECTURES=$3 -DCMAKE_OSX_SYSROOT=macosx -DCMAKE_CXX_COMPILER_WORKS:BOOL=TRUE -DCMAKE_C_COMPILER_WORKS:BOOL=TRUE )
    if [ "$3" != "arm64" ]; then
        CMAKE_COMMON_OPTION+=( -DWITH_KLEIDICV:BOOL=FALSE )
    fi
else
    # On arm64, cmake's CPU_BASELINE=DETECT promotes NEON_FP16 into the baseline
    # (since Apple arm64 has it by default). The baseline flags don't include
    # -march=armv8.2-a+fp16, so vfmaq_f16 (always_inline, requires fullfp16)
    # fails. Fix: pin baseline to NEON so NEON_FP16 stays in dispatch, where it
    # gets compiled with the proper -march=armv8.2-a+fp16 flags.
    if [ "$3" == "arm64" ]; then
        CMAKE_COMMON_OPTION+=( -DCPU_BASELINE=NEON )
    fi
    pushd $CURRENT_SCRIPT_DIR/../../eigen
    # Prefixed with ios_$2_ (device/simulator) so this doesn't collide with
    # platforms/macos's own eigen/build_$arch directories, nor between device
    # and simulator builds of the same arch here -- all three can target
    # "arm64" but configure with different CMake generators/toolchain files
    # (Xcode + iPhoneOS or iPhoneSimulator toolchain here vs. Unix Makefiles
    # for macOS), and CMake refuses to reconfigure a cache with a different
    # generator than it was created with.
    mkdir -p build_ios_${2}_$3
    cd build_ios_${2}_$3
    EIGEN_DIR=${PWD}
    cmake \
	-GXcode \
	${CV_TOOLCHAIN_OPTION[@]} \
	${INSTALL_PREFIX_OPTION[@]} \
	"${CMAKE_COMMON_OPTION[@]}" \
	${@:4} \
	..
    cmake --build . --config Release --target install
    popd    
fi


#pushd $CURRENT_SCRIPT_DIR/../../3rdParty/freetype2
#mkdir -p build_$3
#cd build_$3
#cmake \
#    -GXcode \
#    ${CV_TOOLCHAIN_OPTION[@]} \
#    ${INSTALL_PREFIX_OPTION[@]} \
#    ${CMAKE_COMMON_OPTION[@]} \
#    -DCMAKE_DISABLE_FIND_PACKAGE_ZLIB:BOOL=TRUE \
#    -DCMAKE_DISABLE_FIND_PACKAGE_BZip2:BOOL=TRUE \
#    -DCMAKE_DISABLE_FIND_PACKAGE_PNG:BOOL=TRUE \
#    -DCMAKE_DISABLE_FIND_PACKAGE_HarfBuzz:BOOL=TRUE \
#    ${@:4} \
#    ..
#cmake --build . --config Release --target install
#popd

#pushd $CURRENT_SCRIPT_DIR/../../harfbuzz
#mkdir -p build_$3
#cd build_$3
#cmake \
#    -GXcode \
#    ${CV_TOOLCHAIN_OPTION[@]} \
#    ${INSTALL_PREFIX_OPTION[@]} \
#    ${CMAKE_COMMON_OPTION[@]} \
#    -DHB_HAVE_FREETYPE:BOOL=TRUE \
#    ${@:4} \
#    ..
#cmake --build . --config Release --target install
#popd


cmake \
    -GXcode \
    -S "$CURRENT_SCRIPT_DIR/../.." \
    -B "." \
    ${CV_TOOLCHAIN_OPTION[@]} \
    ${CV_OPTIMIZATION_OPTION[@]} \
    ${CV_CONTRIB_OPTION[@]} \
    ${INSTALL_PREFIX_OPTION[@]} \
    "${CMAKE_COMMON_OPTION[@]}" \
    -DWITH_EIGEN:BOOL=ON \
    -DEigen3_DIR:STRING="$EIGEN_DIR" \
    -DBUILD_SHARED_LIBS:BOOL=FALSE \
    -DBUILD_PERF_TESTS:BOOL=FALSE \
    -DBUILD_TESTS:BOOL=FALSE \
    -DBUILD_opencv_apps:BOOL=FALSE \
    -DBUILD_opencv_java_bindings_generator:BOOL=FALSE \
    -DBUILD_opencv_python_bindings_generator:BOOL=FALSE \
    ${@:4}

