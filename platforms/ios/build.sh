#!/usr/bin/env bash

if [ ! -f cmake ]
then 
  ln -s ../../opencv/platforms/ios/cmake cmake
fi

cd ../..


if [ "$1" != "simulator" ]; then    

    rm -f CMakeCache.txt
    platforms/ios/configure-device_xcode.sh
    rm -rf platforms/ios/armv7s bin/Release opencv/3rdparty/lib/Release 
    xcodebuild -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="armv7s" -target ALL_BUILD clean build
    mkdir -p platforms/ios/armv7s 
    #cp -r lib/Release/* ios/armv7s/
    cp -r bin/Release/* platforms/ios/armv7s/
    cp -r opencv/3rdparty/lib/Release/* platforms/ios/armv7s/
    #cp -r opencv/lib/Release/* ios/armv7/
    cd platforms/ios/armv7s
    libtool -static -o libemgucv_armv7s.a *.a
    cd ../../..
    
    rm -f CMakeCache.txt
    platforms/ios/configure-device_xcode.sh
    rm -rf platforms/ios/armv7 bin/Release opencv/3rdparty/lib/Release
    xcodebuild -parallelizeTargets -jobs 8 -sdk iphoneos -configuration Release ARCHS="armv7" -target ALL_BUILD clean build
    mkdir -p platforms/ios/armv7 
    #cp -r lib/Release/* ios/armv7/
    cp -r bin/Release/* platforms/ios/armv7/
    cp -r opencv/3rdparty/lib/Release/* platforms/ios/armv7/
    #cp -r opencv/lib/Release/* ios/armv7/
    cd platforms/ios/armv7
    libtool -static -o libemgucv_armv7.a *.a
    cd ../../..

fi

rm -f CMakeCache.txt
platforms/ios/configure-simulator_xcode.sh
rm -rf platforms/ios/i386 bin/Release opencv/3rdparty/lib/Release
xcodebuild -parallelizeTargets -jobs 8 -sdk iphonesimulator -configuration Release -target ALL_BUILD clean build
mkdir -p platforms/ios/i386
#cp -r lib/Release/* ios/i386/
cp -r bin/Release/* platforms/ios/i386/
#cp -r opencv/lib/Release/* ios/i386/
cp -r opencv/3rdparty/lib/Release/* platforms/ios/i386/
cd platforms/ios/i386
libtool -static -o libemgucv_i386.a *.a
cd ../../..

rm -rf platforms/ios/universal
mkdir -p platforms/ios/universal
if [ "$1" == "simulator" ]; then
    cp platforms/ios/i386/libemgucv_i386.a platforms/ios/universal/libemgucv.a
else
#    lipo -create -output ios/universal/libemgucv.a ios/armv6/libemgucv_armv6.a ios/armv7/libemgucv_armv7.a ios/i386/libemgucv_i386.a
    lipo -create -output platforms/ios/universal/libemgucv.a platforms/ios/armv7/libemgucv_armv7.a platforms/ios/armv7s/libemgucv_armv7s.a platforms/ios/i386/libemgucv_i386.a
fi

mkdir -p Emgu.CV/PInvoke/iOS
cp -f platforms/ios/universal/libemgucv.a Emgu.CV/PInvoke/iOS

cd platforms/ios

