#!/usr/bin/env bash

if [ ! -f cmake ]
then 
  ln -s ../../opencv/platforms/ios/cmake cmake
fi

cd ../..


if [ "$1" != "simulator" ]; then    

    rm -f CMakeCache.txt
    platforms/ios/configure-device_xcode.sh -DIOS_ARCH="armv7s" $*
    rm -rf platforms/ios/armv7s bin/Release opencv/3rdparty/lib/Release 
    xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="armv7s" -target ALL_BUILD clean build
    mkdir -p platforms/ios/armv7s 
    #cp -r lib/Release/* ios/armv7s/
    cp -r bin/Release/* platforms/ios/armv7s/
    cp -r opencv/3rdparty/lib/Release/* platforms/ios/armv7s/
    #cp -r opencv/lib/Release/* ios/armv7/
    cd platforms/ios/armv7s
    libtool -static -o libemgucv_armv7s.a *.a
    cd ../../..
    
    rm -f CMakeCache.txt
    platforms/ios/configure-device_xcode.sh -DIOS_ARCH="armv7" $*
    rm -rf platforms/ios/armv7 bin/Release opencv/3rdparty/lib/Release
    xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="armv7" -target ALL_BUILD clean build
    mkdir -p platforms/ios/armv7 
    #cp -r lib/Release/* ios/armv7/
    cp -r bin/Release/* platforms/ios/armv7/
    cp -r opencv/3rdparty/lib/Release/* platforms/ios/armv7/
    #cp -r opencv/lib/Release/* ios/armv7/
    cd platforms/ios/armv7
    libtool -static -o libemgucv_armv7.a *.a
    cd ../../..

    rm -f CMakeCache.txt
    platforms/ios/configure-device_xcode.sh -DIOS_ARCH="arm64" $*
    rm -rf platforms/ios/arm64 bin/Release opencv/3rdparty/lib/Release
    xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="arm64" -target ALL_BUILD clean build
    mkdir -p platforms/ios/arm64 
    #cp -r lib/Release/* ios/arm64/
    cp -r bin/Release/* platforms/ios/arm64/
    cp -r opencv/3rdparty/lib/Release/* platforms/ios/arm64/
    #cp -r opencv/lib/Release/* ios/arm64/
    cd platforms/ios/arm64
    libtool -static -o libemgucv_arm64.a *.a
    cd ../../..

fi

rm -f CMakeCache.txt
if [ "$1" == "simulator" ]; then    
  platforms/ios/configure-simulator_xcode.sh -DIOS_ARCH="i386" ${@:2}
else
  platforms/ios/configure-simulator_xcode.sh -DIOS_ARCH="i386" $*
fi
rm -rf platforms/ios/i386 bin/Release opencv/3rdparty/lib/Release
xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphonesimulator -configuration Release ARCHS="i386" -target ALL_BUILD clean build
mkdir -p platforms/ios/i386
#cp -r lib/Release/* ios/i386/
cp -r bin/Release/* platforms/ios/i386/
#cp -r opencv/lib/Release/* ios/i386/
cp -r opencv/3rdparty/lib/Release/* platforms/ios/i386/
cd platforms/ios/i386
libtool -static -o libemgucv_i386.a *.a
cd ../../..

rm -f CMakeCache.txt
if [ "$1" == "simulator" ]; then    
  platforms/ios/configure-simulator_xcode.sh -DIOS_ARCH="x86_64" ${@:2}
else
  platforms/ios/configure-simulator_xcode.sh -DIOS_ARCH="x86_64" $*
fi
rm -rf platforms/ios/x86_64 bin/Release opencv/3rdparty/lib/Release
xcodebuild IPHONEOS_DEPLOYMENT_TARGET=6.0 -parallelizeTargets -jobs 8 -sdk iphonesimulator -configuration Release ARCHS="x86_64" -target ALL_BUILD clean build
mkdir -p platforms/ios/x86_64
#cp -r lib/Release/* ios/x86_64/
cp -r bin/Release/* platforms/ios/x86_64/
#cp -r opencv/lib/Release/* ios/x86_64/
cp -r opencv/3rdparty/lib/Release/* platforms/ios/x86_64/
cd platforms/ios/x86_64
libtool -static -o libemgucv_x86_64.a *.a
cd ../../..

rm -rf platforms/ios/universal
mkdir -p platforms/ios/universal
if [ "$1" == "simulator" ]; then
    lipo -create -output platforms/ios/universal/libemgucv.a  platforms/ios/i386/libemgucv_i386.a  platforms/ios/x86_64/libemgucv_x86_64.a
else
    lipo -create -output platforms/ios/universal/libemgucv.a platforms/ios/armv7/libemgucv_armv7.a platforms/ios/armv7s/libemgucv_armv7s.a platforms/ios/arm64/libemgucv_arm64.a platforms/ios/i386/libemgucv_i386.a  platforms/ios/x86_64/libemgucv_x86_64.a
fi

mkdir -p Emgu.CV/PInvoke/iOS
cp -f platforms/ios/universal/libemgucv.a Emgu.CV/PInvoke/iOS

cd platforms/ios

