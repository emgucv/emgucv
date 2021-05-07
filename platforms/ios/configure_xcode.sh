#!/usr/bin/env bash -v
set -e
set -x
CURRENT_SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"


if [ "$1" == "core" ]; then
    CV_CONTRIB_OPTION=( -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE -DEMGU_CV_WITH_TESSERACT:BOOL=FALSE )
else
    CV_CONTRIB_OPTION=( -DOPENCV_EXTRA_MODULES_PATH=$CURRENT_SCRIPT_DIR/../../opencv_contrib/modules -DEMGU_CV_WITH_FREETYPE:BOOL=TRUE -DEMGU_CV_WITH_TESSERACT:BOOL=TRUE )
fi

#-DHB_HAVE_FREETYPE:BOOL=TRUE -DHB_BUILD_TESTS:BOOL=FALSE
INSTALL_FOLDER=$PWD/build/install


if [ "$2" == "device" ]; then
    CV_TOOLCHAIN_OPTION=( -DCMAKE_TOOLCHAIN_FILE=$CURRENT_SCRIPT_DIR/cmake/Toolchains/Toolchain-iPhoneOS_Xcode.cmake )
    INSTALL_FOLDER=$INSTALL_FOLDER/OpenCV_iPhoneOS_$3
else
    CV_TOOLCHAIN_OPTION=( -DCMAKE_TOOLCHAIN_FILE=$CURRENT_SCRIPT_DIR/cmake/Toolchains/Toolchain-iPhoneSimulator_Xcode.cmake )
    INSTALL_FOLDER=$INSTALL_FOLDER/OpenCV_iPhoneSimulator_$3
fi

INSTALL_PREFIX_OPTION=( -DCMAKE_INSTALL_PREFIX=$INSTALL_FOLDER )

CMAKE_COMMON_OPTION=( -DIOS_ARCH="$3" -DCMAKE_XCODE_ATTRIBUTE_BITCODE_GENERATION_MODE:STRING="bitcode" -DCMAKE_FIND_ROOT_PATH:STRING="$INSTALL_FOLDER" -DIPHONEOS_DEPLOYMENT_TARGET:STRING=9.0 )

pushd $CURRENT_SCRIPT_DIR/../../eigen
mkdir -p build_$3
cd build_$3
EIGEN_DIR=${PWD}
cmake \
    -GXcode \
    ${CV_TOOLCHAIN_OPTION[@]} \
    ${INSTALL_PREFIX_OPTION[@]} \
    ${CMAKE_COMMON_OPTION[@]} \
    ${@:4} \
    ..
cmake --build . --config Release --target install
popd

pushd $CURRENT_SCRIPT_DIR/../../3rdParty/freetype2
mkdir -p build_$3
cd build_$3
cmake \
    -GXcode \
    ${CV_TOOLCHAIN_OPTION[@]} \
    ${INSTALL_PREFIX_OPTION[@]} \
    ${CMAKE_COMMON_OPTION[@]} \
    -DCMAKE_DISABLE_FIND_PACKAGE_ZLIB:BOOL=TRUE \
    -DCMAKE_DISABLE_FIND_PACKAGE_BZip2:BOOL=TRUE \
    -DCMAKE_DISABLE_FIND_PACKAGE_PNG:BOOL=TRUE \
    -DCMAKE_DISABLE_FIND_PACKAGE_HarfBuzz:BOOL=TRUE \
    ${@:4} \
    ..
cmake --build . --config Release --target install
popd

pushd $CURRENT_SCRIPT_DIR/../../harfbuzz
mkdir -p build_$3
cd build_$3
cmake \
    -GXcode \
    ${CV_TOOLCHAIN_OPTION[@]} \
    ${INSTALL_PREFIX_OPTION[@]} \
    ${CMAKE_COMMON_OPTION[@]} \
    -DHB_HAVE_FREETYPE:BOOL=TRUE \
    ${@:4} \
    ..
cmake --build . --config Release --target install
popd


cmake \
-GXcode \
${CV_TOOLCHAIN_OPTION[@]} \
${CV_CONTRIB_OPTION[@]} \
${INSTALL_PREFIX_OPTION[@]} \
${CMAKE_COMMON_OPTION[@]} \
-DWITH_EIGEN:BOOL=ON \
-DEigen3_DIR:STRING="$EIGEN_DIR" \
-DBUILD_SHARED_LIBS:BOOL=FALSE \
-DBUILD_PERF_TESTS:BOOL=FALSE \
-DBUILD_TESTS:BOOL=FALSE \
-DBUILD_opencv_apps:BOOL=FALSE \
-DBUILD_opencv_java_bindings_generator:BOOL=FALSE \
-DBUILD_opencv_python_bindings_generator:BOOL=FALSE \
${@:4} $CURRENT_SCRIPT_DIR/../.. 

