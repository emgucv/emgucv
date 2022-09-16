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

mkdir -p platforms/ios/catalyst_$1     
cd platforms/ios/catalyst_$1
../configure_xcode.sh $CV_CONTRIB_OPTION catalyst $1 -DCMAKE_XCODE_ATTRIBUTE_CLANG_CXX_LANGUAGE_STANDARD="c++20" -DCMAKE_XCODE_ATTRIBUTE_CLANG_CXX_LIBRARY="libc++"  ${@:3}
cmake --build . --config Release --target package
cd ../../..
    


