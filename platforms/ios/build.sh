#!/bin/zsh
set -e

if [ ! -L cmake ]
then 
  ln -s ${PWD}/../../opencv/platforms/ios/cmake cmake
fi

if [ ! -L ../../platforms/scripts ]
then 
  ln -s ${PWD}/../../opencv/platforms/scripts ../../platforms/scripts
fi

cd ../..

mkdir -p libs/iOS

if [ \( "$1" != "simulator" \) -a \( "$1" != "simulator_x86_64" \) ]; then    
    mkdir -p platforms/ios/armv7s     
    cd platforms/ios/armv7s
    ../configure-device_xcode.sh -DIOS_ARCH="armv7s" $*
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..
    
    mkdir -p platforms/ios/armv7
    cd platforms/ios/armv7
    ../configure-device_xcode.sh -DIOS_ARCH="armv7" $*
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..

    mkdir -p platforms/ios/arm64
    cd platforms/ios/arm64
    ../configure-device_xcode.sh -DIOS_ARCH="arm64" $*
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..
fi

if [ "$1" != "simulator_x86_64" ]; then
    mkdir -p platforms/ios/i386
    cd platforms/ios/i386
    if [ "$1" = "simulator" ]; then    
  #skip the first parameter
    ../configure-simulator_xcode.sh -DIOS_ARCH="i386" -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE ${@:2}
    else
    ../configure-simulator_xcode.sh -DIOS_ARCH="i386" -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE $*
    fi
    ./xcodebuild_wrapper -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..
fi

mkdir -p platforms/ios/x86_64
cd platforms/ios/x86_64
if [ \( "$1" = "simulator" \) -o \( "$1" = "simulator_x86_64" \) ]; then
  #skip the first parameter    
  ../configure-simulator_xcode.sh -DIOS_ARCH="x86_64" -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE ${@:2}
else
  ../configure-simulator_xcode.sh -DIOS_ARCH="x86_64" -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE $*
fi
./xcodebuild_wrapper WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build

cd ../../..

cd Emgu.CV.Platform/iOS
#compile Emgu.CV.World.iOS.dll
msbuild /p:Configuration=Release
cd ../../platforms/ios/x86_64
#build the package this time
./xcodebuild_wrapper WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs 8 -configuration Release -target package build

cd ..

