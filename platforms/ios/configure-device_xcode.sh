#!/usr/bin/env bash
set -e

if [ "$1" == "core" ]; then
    CV_CONTRIB_OPTION=
else
    CV_CONTRIB_OPTION=-DOPENCV_EXTRA_MODULES_PATH=`dirname $0`/../../opencv_contrib/modules
fi

cmake -GXcode -DHB_BUILD_TESTS:BOOL=FALSE -DCMAKE_TOOLCHAIN_FILE=`dirname $0`/cmake/Toolchains/Toolchain-iPhoneOS_Xcode.cmake -DCMAKE_INSTALL_PREFIX=../OpenCV_iPhoneOS $CV_CONTRIB_OPTION -DBUILD_opencv_hdf:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DBUILD_PERF_TESTS:BOOL=FALSE -DBUILD_TESTS:BOOL=FALSE -DBUILD_opencv_apps:BOOL=FALSE -DBUILD_opencv_java_bindings_generator:BOOL=FALSE -DBUILD_opencv_python_bindings_generator:BOOL=FALSE -DCMAKE_C_FLAGS=-fembed-bitcode -DCMAKE_CXX_FLAGS=-fembed-bitcode -DCMAKE_CXX_FLAGS_RELEASE:STRING=-g0 -DCMAKE_C_FLAGS_RELEASE:STRING=-g0 -DIPHONEOS_DEPLOYMENT_TARGET:STRING=8.0 -DHB_HAVE_FREETYPE:BOOL=TRUE ${@:2} `dirname $0`/../.. 
