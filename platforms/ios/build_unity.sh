#!/usr/bin/env bash
set -e
# PNG and JPEG symbol-prefixing (EMGU_CV_PNG_PREFIX / EMGU_CV_JPEG_PREFIX) --
# originally added here to avoid duplicate/missing libpng and libjpeg-turbo
# symbols against Unity's own statically-linked copies at Unity's final app
# link -- is now the default in ./build.sh itself, so the same
# libs/iOS/libcvextern_ios.xcframework works for both regular iOS consumers
# and Unity iOS. This wrapper is now just an alias kept for anyone still
# calling it by name.
./build.sh $*
