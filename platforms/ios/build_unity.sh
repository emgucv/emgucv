#!/usr/bin/env bash
set -e
./build.sh $* -DWITH_PNG:BOOL=FALSE -DWITH_JPEG:BOOL=FALSE 
