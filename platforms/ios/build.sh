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
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target package build
#    cp -r ../../../libs/Release/* bin/Release
#    cp -r opencv/3rdparty/lib/Release/* bin/Release
#    cp -r opencv/lib/Release/* bin/Release
#    cp -r freetype2/Release-iphoneos/* bin/Release
#    cp -r harfbuzz/Release-iphoneos/* bin/Release
#    libtool -static -o libemgucv_armv7s.a bin/Release/*.a
    cd ../../..
    
    mkdir -p platforms/ios/armv7
    cd platforms/ios/armv7
    ../configure-device_xcode.sh -DIOS_ARCH="armv7" $*
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target package build
#    cp -r ../../../libs/Release/* bin/Release
#    cp -r opencv/3rdparty/lib/Release/* bin/Release
#    cp -r opencv/lib/Release/* bin/Release	
#    cp -r freetype2/Release-iphoneos/* bin/Release
#    cp -r harfbuzz/Release-iphoneos/* bin/Release
#    libtool -static -o libemgucv_armv7.a bin/Release/*.a
    cd ../../..

    mkdir -p platforms/ios/arm64
    cd platforms/ios/arm64
    ../configure-device_xcode.sh -DIOS_ARCH="arm64" $*
    ./xcodebuild_wrapper BITCODE_GENERATION_MODE=bitcode -parallelizeTargets -jobs 8 -configuration Release -target package build
#    cp -r ../../../libs/Release/* bin/Release
#    cp -r opencv/3rdparty/lib/Release/* bin/Release  
#    cp -r opencv/lib/Release/* bin/Release
#    cp -r freetype2/Release-iphoneos/* bin/Release
#    cp -r harfbuzz/Release-iphoneos/* bin/Release
#    libtool -static -o libemgucv_arm64.a bin/Release/*.a
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
    ./xcodebuild_wrapper -parallelizeTargets -jobs 8 -configuration Release -target package build
#    cp -r ../../../libs/Release/* bin/Release
#    cp -r opencv/3rdparty/lib/Release/* bin/Release  
#    cp -r opencv/lib/Release/* bin/Release
#    cp -r freetype2/Release-iphonesimulator/* bin/Release
#    cp -r harfbuzz/Release-iphonesimulator/* bin/Release
    #cp -r opencv/3rdparty/ippicv/ippiw_mac/lib/ia32/* bin/Release
    #cp -r opencv/3rdparty/ippicv/ippicv_mac/lib/ia32/* bin/Release
#    libtool -static -o libemgucv_i386.a bin/Release/*.a
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
./xcodebuild_wrapper WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs 8 -configuration Release -target package build
#cp -r ../../../libs/Release/* bin/Release
#cp -r opencv/3rdparty/lib/Release/* bin/Release
#cp -r opencv/lib/Release/* bin/Release
#cp -r freetype2/Release-iphonesimulator/* bin/Release
#cp -r harfbuzz/Release-iphonesimulator/* bin/Release
#cp -r opencv/3rdparty/ippicv/ippiw_mac/lib/intel64/* bin/Release
#cp -r opencv/3rdparty/ippicv/ippicv_mac/lib/intel64/* bin/Release

#libtool -static -o libemgucv_x86_64.a bin/Release/*.a
cd ../../..

#rm -rf platforms/ios/universal
#mkdir -p platforms/ios/universal
#if [ "$1" == "simulator" ]; then
    #skip the first parameter
#    lipo -create -output platforms/ios/universal/libemgucv.a platforms/ios/i386/libemgucv_i386.a platforms/ios/x86_64/libemgucv_x86_64.a
#elif [ "$1" == "simulator_x86_64" ]; then
#    cp -f platforms/ios/x86_64/libemgucv_x86_64.a platforms/ios/universal/libemgucv.a 
#else
#    lipo -create -output platforms/ios/universal/libemgucv.a platforms/ios/armv7/libemgucv_armv7.a platforms/ios/armv7s/libemgucv_armv7s.a platforms/ios/arm64/libemgucv_arm64.a platforms/ios/i386/libemgucv_i386.a  platforms/ios/x86_64/libemgucv_x86_64.a
#fi

#mkdir -p libs/iOS
#cp -f platforms/ios/universal/libemgucv.a libs/iOS/libcvextern.a

cd platforms/ios

