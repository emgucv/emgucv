cd ..

if [ "$1" != "simulator" ]; then    
    rm -f CMakeCache.txt
    ios/configure-device_xcode.sh
    rm -rf ios/armv6 lib/Release bin/Release
    xcodebuild -sdk iphoneos -configuration Release ARCHS="armv6" -target ALL_BUILD clean build
    mkdir -p ios/armv6 
    cp -r lib/Release/* ios/armv6/
    cp -r bin/Release/* ios/armv6/
    cd ios/armv6
    libtool -static -o libemgucv_armv6.a *.a
    cd ../..
    
    rm -f CMakeCache.txt
    ios/configure-device_xcode.sh
    rm -rf ios/armv7 lib/Release bin/Release
    xcodebuild -sdk iphoneos -configuration Release ARCHS="armv7" -target ALL_BUILD clean build
    mkdir -p ios/armv7 
    cp -r lib/Release/* ios/armv7/
    cp -r bin/Release/* ios/armv7/
    cd ios/armv7
    libtool -static -o libemgucv_armv7.a *.a
    cd ../..
fi

rm -f CMakeCache.txt
ios/configure-simulator_xcode.sh
rm -rf ios/i386 lib/Release bin/Release
xcodebuild -sdk iphonesimulator -configuration Release -target ALL_BUILD clean build
mkdir -p ios/i386
cp -r lib/Release/* ios/i386/
cp -r bin/Release/* ios/i386/
cd ios/i386
libtool -static -o libemgucv_i386.a *.a
cd ../..

rm -rf ios/universal
mkdir -p ios/universal
if [ "$1" == "simulator" ]; then
    cp ios/i386/libemgucv_i386.a ios/universal/libemgucv.a
else
    lipo -create -output ios/universal/libemgucv.a ios/armv6/libemgucv_armv6.a ios/armv7/libemgucv_armv7.a ios/i386/libemgucv_i386.a
fi

mkdir -p Emgu.CV/PInvoke/MonoTouch
cp -f ios/universal/libemgucv.a Emgu.CV/PInvoke/MonoTouch

cd ios

