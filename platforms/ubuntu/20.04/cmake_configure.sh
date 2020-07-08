#!/bin/sh

cd "$(dirname "$0")"

cd ../../..

cd vtk
mkdir -p build
cd build
CFLAGS=-fPIC CXXFLAGS=-fPIC cmake -DBUILD_TESTING:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DCMAKE_BUILD_TYPE:STRING="Release" ..
make

cd ../../platforms/ubuntu/20.04

mkdir -p build
cd build
CFLAGS=-fPIC CXXFLAGS=-fPIC cmake -DBUILD_TESTS:BOOL=FALSE -DBUILD_PERF_TESTS:BOOL=FALSE -DBUILD_opencv_apps:BOOL=FALSE -DBUILD_DOCS:BOOL=FALSE -DWITH_TBB:BOOL=TRUE -DWITH_CUDA:BOOL=FALSE -DWITH_IPP:BOOL=FALSE -DWITH_EIGEN:BOOL=FALSE -DOPENCV_EXTRA_MODULES_PATH=../../../../opencv_contrib/modules -DBUILD_opencv_ts:BOOL=FALSE -DBUILD_opencv_java:BOOL=FALSE -DBUILD_opencv_python2:BOOL=FALSE -DBUILD_opencv_python3:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DVTK_DIR:String="$PWD/../../../../vtk/build" -DCMAKE_BUILD_TYPE:String="Release" -DCMAKE_CXX_STANDARD:String="11" $PWD/../../../..
make
cd ..
