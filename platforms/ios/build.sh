#!/usr/bin/env bash
set -e

if [ ! -f cmake ]
then 
  ln -s ../../opencv/platforms/ios/cmake cmake
fi

cd ../..

if [ "$1" != "simulator" ]; then    

    mkdir -p platforms/ios/armv7s     
    cd platforms/ios/armv7s
    ../configure-device_xcode.sh -DIOS_ARCH="armv7s" $*
    xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="armv7s" -target ALL_BUILD clean build
    cp -r ../../../libs/Release/* bin/Release
    cp -r opencv/3rdparty/lib/Release/* bin/Release  
    libtool -static -o libemgucv_armv7s.a bin/Release/*.a
    cd ../../..
    
    mkdir -p platforms/ios/armv7
    cd platforms/ios/armv7
    ../configure-device_xcode.sh -DIOS_ARCH="armv7" $*
    xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="armv7" -target ALL_BUILD clean build
    cp -r ../../../libs/Release/* bin/Release
    cp -r opencv/3rdparty/lib/Release/* bin/Release  
    libtool -static -o libemgucv_armv7.a bin/Release/*.a
    cd ../../..

    mkdir -p platforms/ios/arm64
    cd platforms/ios/arm64
    ../configure-device_xcode.sh -DIOS_ARCH="arm64" $*
    xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="arm64" -target ALL_BUILD clean build
    cp -r ../../../libs/Release/* bin/Release
    cp -r opencv/3rdparty/lib/Release/* bin/Release  
    libtool -static -o libemgucv_arm64.a bin/Release/*.a
    cd ../../..
fi


mkdir -p platforms/ios/i386
cd platforms/ios/i386
if [ "$1" == "simulator" ]; then    
  ../configure-simulator_xcode.sh -DIOS_ARCH="i386" ${@:2}
else
  ../configure-simulator_xcode.sh -DIOS_ARCH="i386" $*
fi
xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphonesimulator -configuration Release ARCHS="i386" -target ALL_BUILD clean build
cp -r ../../../libs/Release/* bin/Release
cp -r opencv/3rdparty/lib/Release/* bin/Release  
cp -r opencv/3rdparty/ippicv/ippiw_mac/lib/ia32/* bin/Release
cp -r opencv/3rdparty/ippicv/ippicv_mac/lib/ia32/* bin/Release
libtool -static -o libemgucv_i386.a bin/Release/*.a
cd ../../..

mkdir -p platforms/ios/x86_64
cd platforms/ios/x86_64
if [ "$1" == "simulator" ]; then    
  ../configure-simulator_xcode.sh -DIOS_ARCH="x86_64" ${@:2}
else
  ../configure-simulator_xcode.sh -DIOS_ARCH="x86_64" $*
fi
xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs 8 -sdk iphonesimulator -configuration Release ARCHS="x86_64" -target ALL_BUILD clean build
cp -r ../../../libs/Release/* bin/Release
cp -r opencv/3rdparty/lib/Release/* bin/Release
cp -r opencv/3rdparty/ippicv/ippiw_mac/lib/intel64/* bin/Release
cp -r opencv/3rdparty/ippicv/ippicv_mac/lib/intel64/* bin/Release

libtool -static -o libemgucv_x86_64.a bin/Release/*.a
cd ../../..

rm -rf platforms/ios/universal
mkdir -p platforms/ios/universal
if [ "$1" == "simulator" ]; then
    lipo -create -output platforms/ios/universal/libemgucv.a platforms/ios/i386/libemgucv_i386.a platforms/ios/x86_64/libemgucv_x86_64.a
else
    lipo -create -output platforms/ios/universal/libemgucv.a platforms/ios/armv7/libemgucv_armv7.a platforms/ios/armv7s/libemgucv_armv7s.a platforms/ios/arm64/libemgucv_arm64.a platforms/ios/i386/libemgucv_i386.a  platforms/ios/x86_64/libemgucv_x86_64.a
fi

mkdir -p libs/iOS
cp -f platforms/ios/universal/libemgucv.a libs/iOS/libcvextern.a

cd platforms/ios

