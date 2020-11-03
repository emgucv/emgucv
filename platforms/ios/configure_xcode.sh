#!/usr/bin/env bash
set -e

if [ "$1" == "core" ]; then
    CV_CONTRIB_OPTION=( -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE -DEMGU_CV_WITH_TESSERACT:BOOL=FALSE )
else
    CV_CONTRIB_OPTION=( -DOPENCV_EXTRA_MODULES_PATH=`dirname $0`/../../opencv_contrib/modules -DEMGU_CV_WITH_FREETYPE:BOOL=FALSE -DEMGU_CV_WITH_TESSERACT:BOOL=TRUE )
fi

#-DHB_HAVE_FREETYPE:BOOL=TRUE -DHB_BUILD_TESTS:BOOL=FALSE

if [ "$2" == "device" ]; then
    CV_TOOLCHAIN_OPTION=( -DCMAKE_TOOLCHAIN_FILE=`dirname $0`/cmake/Toolchains/Toolchain-iPhoneOS_Xcode.cmake -DCMAKE_INSTALL_PREFIX=../OpenCV_iPhoneOS )
else
    CV_TOOLCHAIN_OPTION=( -DCMAKE_TOOLCHAIN_FILE=`dirname $0`/cmake/Toolchains/Toolchain-iPhoneSimulator_Xcode.cmake -DCMAKE_INSTALL_PREFIX=../OpenCV_iPhoneSimulator )
fi
    
cmake \
-GXcode \
${CV_TOOLCHAIN_OPTION[@]} \
${CV_CONTRIB_OPTION[@]} \
-DBUILD_SHARED_LIBS:BOOL=FALSE \
-DBUILD_PERF_TESTS:BOOL=FALSE \
-DBUILD_TESTS:BOOL=FALSE \
-DBUILD_opencv_apps:BOOL=FALSE \
-DBUILD_opencv_java_bindings_generator:BOOL=FALSE \
-DBUILD_opencv_python_bindings_generator:BOOL=FALSE \
-DIPHONEOS_DEPLOYMENT_TARGET:STRING=9.0 \
${@:3} `dirname $0`/../.. 

