cd ..
ios/configure-device_xcode.sh
xcodebuild -sdk iphoneos -configuration Release -target ALL_BUILD clean build
rm -rf ios/armv7
mkdir -p ios/armv7
cp -r lib/Release/* ios/armv7/
cd ios/armv7
for i in *; do j=`echo $i | cut -d . -f 1`; j=$j"_armv7.a";mv $i $j; done
cd ../..
mkdir -p Emgu.CV/PInvoke/MonoTouch/armv7
mv ios/armv7/* Emgu.CV/PInvoke/MonoTouch/armv7
rm CMakeCache.txt
ios/configure-simulator_xcode.sh
xcodebuild -sdk iphonesimulator -configuration Release -target ALL_BUILD clean build
rm -rf ios/i386
mkdir -p ios/i386
cp -r lib/Release/* ios/i386/
cd ios/i386
for i in *; do j=`echo $i | cut -d . -f 1`; j=$j"_i386.a";mv $i $j; done
cd ../..
mkdir -p Emgu.CV/PInvoke/MonoTouch/i386
mv ios/i386/* Emgu.CV/PInvoke/MonoTouch/i386
cd ios

