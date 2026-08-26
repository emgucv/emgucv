#!/usr/bin/env bash
set -e
# PNG stays disabled (unverified whether it collides the way libpng did for
# WebGL). JPEG is enabled with every libjpeg-turbo linkage-visible symbol
# renamed via EMGU_CV_JPEG_PREFIX -- confirmed necessary, not just cautious:
# a plain -DWITH_JPEG:BOOL=TRUE builds fine but crashes at runtime
# (EXC_BAD_ACCESS/SIGBUS) because IronSourceAdQualitySDK, a Pod Unity bundles
# into a fresh iOS export by default, statically links its own libjpeg and
# Mach-O's ld only warns (doesn't error) on the resulting duplicate symbols,
# silently picking one implementation's struct layout for the other's calls.
./build.sh $* -DWITH_PNG:BOOL=FALSE -DBUILD_JPEG:BOOL=TRUE -DWITH_JPEG:BOOL=TRUE -DEMGU_CV_JPEG_PREFIX:BOOL=TRUE
