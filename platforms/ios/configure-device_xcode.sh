#!/usr/bin/env bash
set -e
cmake -GXcode -DCMAKE_TOOLCHAIN_FILE=./platforms/ios/cmake/Toolchains/Toolchain-iPhoneOS_Xcode.cmake -DCMAKE_INSTALL_PREFIX=../OpenCV_iPhoneOS -DOPENCV_EXTRA_MODULES_PATH=opencv_contrib/modules -DBUILD_opencv_hdf:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DBUILD_PERF_TESTS:BOOL=FALSE -DBUILD_TESTS:BOOL=FALSE -DBUILD_opencv_apps:BOOL=FALSE $* ./ 
