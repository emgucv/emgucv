# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Optional: rename every libjpeg-turbo symbol that needs external linkage
#  (via generate_jpeg_prefix.py) so cvextern.a's statically-linked
#  libjpeg-turbo doesn't collide with a libjpeg already linked into the host
#  application -- e.g. Unity's iOS build bundling a third-party Pod
#  (IronSourceAdQualitySDK) that statically links its own libjpeg. Mach-O's
#  linker only warns on duplicate symbols and picks one arbitrarily, which is
#  not safe: the two libjpeg builds' internal struct layouts can differ,
#  producing an EXC_BAD_ACCESS/SIGBUS at runtime rather than a link error.
#
#  Generates ${CMAKE_BINARY_DIR}/emgu_jpeg_prefix_generated/jpegprefix.h;
#  EMGU_CV_JPEG_PREFIX_HEADER is consumed by BuildCvExternTarget.cmake, which
#  force-includes it on the libjpeg-turbo and opencv_imgcodecs targets
#  specifically, once those targets exist. Unlike libpng, libjpeg-turbo has
#  no built-in prefix mechanism to hook into -- the generated header's plain
#  #define renames are the entire mechanism, not a PNG_PREFIX-style opt-in.
# ----------------------------------------------------------------------------

IF(EMGU_CV_JPEG_PREFIX)
  SET(EMGU_CV_JPEG_PREFIX_DIR "${CMAKE_BINARY_DIR}/emgu_jpeg_prefix_generated")
  SET(EMGU_CV_JPEG_PREFIX_HEADER "${EMGU_CV_JPEG_PREFIX_DIR}/jpegprefix.h")
  FILE(MAKE_DIRECTORY "${EMGU_CV_JPEG_PREFIX_DIR}")
  EXECUTE_PROCESS(
    COMMAND python3
            "${CMAKE_CURRENT_LIST_DIR}/../Emgu.CV.Extern/cmake/generate_jpeg_prefix.py"
            "${CMAKE_CURRENT_LIST_DIR}/../opencv/3rdparty/libjpeg-turbo/src"
            "${EMGU_CV_JPEG_PREFIX_HEADER}"
            "emgu_"
    RESULT_VARIABLE EMGU_CV_JPEG_PREFIX_GEN_RESULT
    OUTPUT_VARIABLE EMGU_CV_JPEG_PREFIX_GEN_OUTPUT
    ERROR_VARIABLE EMGU_CV_JPEG_PREFIX_GEN_OUTPUT)
  IF(NOT EMGU_CV_JPEG_PREFIX_GEN_RESULT EQUAL 0)
    MESSAGE(FATAL_ERROR "Failed to generate jpegprefix.h: ${EMGU_CV_JPEG_PREFIX_GEN_OUTPUT}")
  ENDIF()
  MESSAGE(STATUS "EMGU_CV_JPEG_PREFIX: ${EMGU_CV_JPEG_PREFIX_GEN_OUTPUT}")
ENDIF()
