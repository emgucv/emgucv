#!/bin/bash

cd "$(dirname "$0")"

BUILD_TYPE=full
if [[ $# -gt 0 ]]; then
    if [ "$1" == "core" ]; then
	BUILD_TYPE=core
    fi
fi

cd ../../..

if [ "$BUILD_TYPE" == "core" ]; then
    echo "Performing a core build"
    TESSERACT_OPTION=-DEMGU_CV_WITH_TESSERACT:BOOL=FALSE
    VTK_OPTION=
    CONTRIB_OPTION=
else
    echo "Performing a full build"
    TESSERACT_OPTION=-DEMGU_CV_WITH_TESSERACT:BOOL=TRUE
    cd vtk
    mkdir -p build
    cd build
    CFLAGS=-fPIC CXXFLAGS=-fPIC cmake -DBUILD_TESTING:BOOL=FALSE -DBUILD_SHARED_LIBS:BOOL=FALSE -DCMAKE_BUILD_TYPE:STRING="Release" ..
    make
    VTK_OPTION=-DVTK_DIR:String="$PWD"
    cd ../..
    CONTRIB_OPTION=-DOPENCV_EXTRA_MODULES_PATH=../../../../opencv_contrib/modules 
fi

cd eigen
mkdir -p build
cd build
CFLAGS=-fPIC CXXFLAGS=-fPIC cmake -DCMAKE_BUILD_TYPE:STRING="Release" ..
make
cd ../../platforms/ubuntu/20.04

mkdir -p build
cd build
CFLAGS=-fPIC CXXFLAGS=-fPIC cmake \
      $TESSERACT_OPTION \
      -DBUILD_TESTS:BOOL=FALSE \
      -DBUILD_PERF_TESTS:BOOL=FALSE \
      -DBUILD_opencv_apps:BOOL=FALSE \
      -DBUILD_DOCS:BOOL=FALSE \
      $CONTRIB_OPTION \
      -DBUILD_opencv_ts:BOOL=FALSE \
      -DBUILD_opencv_java:BOOL=FALSE \
      -DBUILD_opencv_python2:BOOL=FALSE \
      -DBUILD_opencv_python3:BOOL=FALSE \
      -DBUILD_SHARED_LIBS:BOOL=FALSE \
      $VTK_OPTION \
      -DWITH_EIGEN:BOOL=TRUE \
      -DEigen3_DIR:String="$PWD/../../../../eigen/build" \
      -DCMAKE_BUILD_TYPE:String="Release" \
      -DCMAKE_CXX_STANDARD:String="11" \
      $PWD/../../../..

#      -DWITH_TBB:BOOL=TRUE \
#      -DWITH_CUDA:BOOL=FALSE \
    
C_INCLUDE_PATH=$PWD/../../../../eigen/ CPLUS_INCLUDE_PATH=$PWD/../../../../eigen/ make

cd ..
