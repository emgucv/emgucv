#!/bin/sh

cd "$(dirname "$0")"

cd ../../..
cd vtk
mkdir -p build
cd build
CFLAGS=-fPIC CXXFLAGS=-fPIC cmake -DBUILD_TESTING:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DCMAKE_BUILD_TYPE:STRING="Release" .
make 
cd ../../platforms/centos/7

mkdir -p build
cd build

cmake3 -DBUILD_TESTS:BOOL=FALSE -DBUILD_PERF_TESTS:BOOL=FALSE -DBUILD_opencv_apps:BOOL=FALSE -DBUILD_DOCS:BOOL=FALSE -DWITH_TBB:BOOL=TRUE -DWITH_CUDA:BOOL=FALSE -DWITH_IPP:BOOL=FALSE -DOPENCV_EXTRA_MODULES_PATH="$PWD/../../../../opencv_contrib/modules" -DBUILD_opencv_ts:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DEMGU_CV_WITH_TESSERACT:BOOL=TRUE -DVTK_DIR:STRING="$PWD/../../../../vtk/build" -DCMAKE_BUILD_TYPE:STRING="Release" ../../../../
make
cd ..

