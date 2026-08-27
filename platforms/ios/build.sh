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
elif [[ "$2" == "mini" ]]; then
    CV_CONTRIB_OPTION=mini
else
    CV_CONTRIB_OPTION=full
fi

JOB_COUNT=1

# PNG and JPEG are built with every libpng/libjpeg-turbo linkage-visible
# symbol renamed (EMGU_CV_PNG_PREFIX / EMGU_CV_JPEG_PREFIX -- see
# cmake/PngPrefix.cmake, cmake/JpegPrefix.cmake, and
# Emgu.CV.Extern/cmake/BuildCvExternTarget.cmake). This was originally added
# only for the Unity iOS build (build_unity.sh), to avoid duplicate/missing
# libpng and libjpeg-turbo symbols against Unity's own statically-linked
# copies at Unity's final app link. It's the default here too so a single
# libs/iOS/libcvextern_ios.xcframework build works for both regular iOS
# consumers and Unity iOS, instead of needing two separately-built
# xcframeworks that would overwrite the same output path -- the renaming is
# purely internal linkage inside cvextern's own static libs and doesn't
# change any public `cve*`-prefixed API surface, so it's safe for regular
# consumers too. Pass -DEMGU_CV_PNG_PREFIX:BOOL=FALSE / -DEMGU_CV_JPEG_PREFIX:BOOL=FALSE
# as extra args to opt out for a specific build.
PNG_JPEG_PREFIX_OPTIONS=( -DBUILD_PNG:BOOL=TRUE -DWITH_PNG:BOOL=TRUE -DEMGU_CV_PNG_PREFIX:BOOL=TRUE -DBUILD_JPEG:BOOL=TRUE -DWITH_JPEG:BOOL=TRUE -DEMGU_CV_JPEG_PREFIX:BOOL=TRUE )

#    mkdir -p platforms/ios/iphoneos_armv7s
#    cd platforms/ios/iphoneos_armv7s
#    ../configure_xcode.sh $CV_CONTRIB_OPTION device armv7s ${@:3}
#    ./xcodebuild_wrapper -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
#    cd ../../..
    
#    mkdir -p platforms/ios/iphoneos_armv7
#    cd platforms/ios/iphoneos_armv7
#    ../configure_xcode.sh $CV_CONTRIB_OPTION device armv7 ${@:3}
#    ./xcodebuild_wrapper -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
#    cd ../../..

if [ "$1" = "device_arm64" ] || [ "$1" = "" ] || [ "$1" = "all" ]; then    
    mkdir -p platforms/ios/iphoneos_arm64
    cd platforms/ios/iphoneos_arm64
    ../configure_xcode.sh $CV_CONTRIB_OPTION device arm64 "${PNG_JPEG_PREFIX_OPTIONS[@]}" ${@:3}
    #./xcodebuild_wrapper -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
    xcodebuild ARCHS=arm64 ONLY_ACTIVE_ARCH=NO CODE_SIGN_IDENTITY="" CODE_SIGNING_REQUIRED=NO CODE_SIGNING_ALLOWED=NO -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
    cd ../../..
fi

if [ "$1" = "simulator_arm64" ] || [ "$1" = "" ] || [ "$1" = "all" ]; then
    mkdir -p platforms/ios/simulator_arm64
    cd platforms/ios/simulator_arm64
    #skip the first two parameter
    ../configure_xcode.sh $CV_CONTRIB_OPTION simulator arm64 -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE "${PNG_JPEG_PREFIX_OPTIONS[@]}" ${@:3}
    #./xcodebuild_wrapper -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
    xcodebuild ARCHS=arm64 ONLY_ACTIVE_ARCH=NO CODE_SIGN_IDENTITY="" CODE_SIGNING_REQUIRED=NO CODE_SIGNING_ALLOWED=NO -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
    cd ../../..
fi

if [ "$1" = "simulator_x86_64" ] || [ "$1" = "" ] || [ "$1" = "all" ]; then
    mkdir -p platforms/ios/simulator_x86_64
    cd platforms/ios/simulator_x86_64
    #skip the first two parameter    
    ../configure_xcode.sh $CV_CONTRIB_OPTION simulator x86_64 -DBUILD_IPP_IW:BOOL=FALSE -DWITH_IPP:BOOL=FALSE "${PNG_JPEG_PREFIX_OPTIONS[@]}" ${@:3}
    #./xcodebuild_wrapper WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
    xcodebuild ARCHS=x86_64 ONLY_ACTIVE_ARCH=NO CODE_SIGN_IDENTITY="" CODE_SIGNING_REQUIRED=NO CODE_SIGNING_ALLOWED=NO WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target ALL_BUILD build
    cd ../../..
fi

if [ "$1" = "simulator_x86_64" ] || [ "$1" = "" ] || [ "$1" = "all" ]; then
    cd Emgu.CV.Runtime/Maui/UI
    #msbuild /p:Configuration=Release
    dotnet restore
    dotnet build
    cd ../../../platforms/ios/simulator_x86_64
    #build the package this time
    #./xcodebuild_wrapper WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target package build
    xcodebuild CODE_SIGN_IDENTITY="" CODE_SIGNING_REQUIRED=NO CODE_SIGNING_ALLOWED=NO WARNING_CFLAGS=-Wno-implicit-function-declaration -parallelizeTargets -jobs ${JOB_COUNT} -configuration Release -target package build
    cd ..
fi

