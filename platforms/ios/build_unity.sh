#!/usr/bin/env bash
set -e
# Both PNG and JPEG are enabled with every libpng/libjpeg-turbo
# linkage-visible symbol renamed (EMGU_CV_PNG_PREFIX / EMGU_CV_JPEG_PREFIX)
# -- confirmed necessary for JPEG, not just cautious: a plain
# -DWITH_JPEG:BOOL=TRUE builds fine but crashes at runtime
# (EXC_BAD_ACCESS/SIGBUS) because IronSourceAdQualitySDK (a Pod Unity bundles
# into a fresh iOS export by default) and Unity's own UnityRuntime.framework
# both statically link their own libjpeg, and Mach-O's ld only warns
# (doesn't error) on the resulting duplicate symbols, silently picking one
# implementation's struct layout for the other's calls. PNG is enabled on
# the same precaution -- Unity's own runtime statically links libpng too,
# for Texture2D.LoadImage, matching the WebGL collision this was proven
# against first.
./build.sh $* -DBUILD_PNG:BOOL=TRUE -DWITH_PNG:BOOL=TRUE -DEMGU_CV_PNG_PREFIX:BOOL=TRUE -DBUILD_JPEG:BOOL=TRUE -DWITH_JPEG:BOOL=TRUE -DEMGU_CV_JPEG_PREFIX:BOOL=TRUE
