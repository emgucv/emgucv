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

if [[ "$2" == "core" ]]; then
    CV_CONTRIB_OPTION=core
else
    CV_CONTRIB_OPTION=full
fi

if [ \( "$1" != "simulator" \) -a \( "$1" != "simulator_x86_64" \) ]; then    
    mkdir -p platforms/ios/armv7s     
    cd platforms/ios/armv7s
    ../configure_xcode.sh $CV_CONTRIB_OPTION device -DIOS_ARCH="armv7s" ${@:3}
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..
    
    mkdir -p platforms/ios/armv7
    cd platforms/ios/armv7
    ../configure_xcode.sh $CV_CONTRIB_OPTION device -DIOS_ARCH="armv7" ${@:3}
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..

    mkdir -p platforms/ios/arm64
    cd platforms/ios/arm64
    ../configure_xcode.sh $CV_CONTRIB_OPTION device -DIOS_ARCH="arm64" ${@:3}
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..
fi

if [ "$1" != "simulator_x86_64" ]; then
    mkdir -p platforms/ios/i386
    cd platforms/ios/i386
    #skip the first two parameter
    ../configure_xcode.sh $CV_CONTRIB_OPTION simulator -DIOS_ARCH="i386" -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE ${@:3}
    ./xcodebuild_wrapper -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build
    cd ../../..
fi

mkdir -p platforms/ios/x86_64
cd platforms/ios/x86_64
#skip the first two parameter    
../configure_xcode.sh $CV_CONTRIB_OPTION simulator -DIOS_ARCH="x86_64" -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE ${@:3}

./xcodebuild_wrapper WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs 8 -configuration Release -target ALL_BUILD clean build

cd ../../..

cd Emgu.CV.Platform/iOS
#compile Emgu.CV.World.iOS.dll
msbuild /p:Configuration=Release
cd ../../platforms/ios/x86_64
#build the package this time
./xcodebuild_wrapper WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs 8 -configuration Release -target package build

cd ..

