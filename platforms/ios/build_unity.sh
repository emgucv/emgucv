#!/usr/bin/env bash

./build.sh -DWITH_PNG:BOOL=FALSE -DCMAKE_CXX_FLAGS="-std=c++11 -stdlib=libc++" -DCMAKE_EXE_LINKER_FLAGS="-std=c++11 -stdlib=libc++" 
