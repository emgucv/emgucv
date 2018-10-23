#!/usr/bin/env bash
set -e
cmake -GXcode -DCMAKE_TOOLCHAIN_FILE=`dirname $0`/cmake/Toolchains/Toolchain-iPhoneOS_Xcode.cmake -DCMAKE_INSTALL_PREFIX=../OpenCV_iPhoneOS -DOPENCV_EXTRA_MODULES_PATH=`dirname $0`/../../opencv_contrib/modules -DBUILD_opencv_hdf:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DBUILD_PERF_TESTS:BOOL=FALSE -DBUILD_TESTS:BOOL=FALSE -DBUILD_opencv_apps:BOOL=FALSE -DBUILD_opencv_java_bindings_generator:BOOL=FALSE -DBUILD_opencv_python_bindings_generator:BOOL=FALSE -DOPENCV_SKIP_VISIBILITY_HIDDEN:BOOL=TRUE $* `dirname $0`/../.. 
