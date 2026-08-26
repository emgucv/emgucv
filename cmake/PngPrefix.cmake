# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Optional: rename every libpng public symbol (via libpng's own PNG_PREFIX
#  mechanism) so cvextern.a's statically-linked libpng doesn't collide with
#  a libpng already linked into the host application -- e.g. Unity's WebGL
#  player runtime, or Unity's iOS UnityRuntime.framework, both of which link
#  their own libpng for Texture2D.LoadImage and would otherwise hit a
#  duplicate-symbol failure (a hard wasm-ld error on WebGL; a silent,
#  runtime-unsafe warning on Mach-O's ld for iOS -- see EMGU_CV_JPEG_PREFIX's
#  comment in cmake/JpegPrefix.cmake for why "just a warning" still isn't
#  safe to leave unresolved) at final link.
#
#  Not Emscripten-specific despite the mechanism being proven there first --
#  the collision risk is about the *host application* also linking libpng,
#  which applies equally to the iOS Unity build.
#
#  Generates ${CMAKE_BINARY_DIR}/emgu_png_prefix_generated/pngprefix.h;
#  EMGU_CV_PNG_PREFIX_HEADER is consumed by BuildCvExternTarget.cmake, which
#  force-includes it (with -DPNG_PREFIX=emgu_) on the libpng and
#  opencv_imgcodecs targets specifically, once those targets exist.
# ----------------------------------------------------------------------------

IF(EMGU_CV_PNG_PREFIX)
  SET(EMGU_CV_PNG_PREFIX_DIR "${CMAKE_BINARY_DIR}/emgu_png_prefix_generated")
  SET(EMGU_CV_PNG_PREFIX_HEADER "${EMGU_CV_PNG_PREFIX_DIR}/pngprefix.h")
  FILE(MAKE_DIRECTORY "${EMGU_CV_PNG_PREFIX_DIR}")
  EXECUTE_PROCESS(
    COMMAND python3
            "${CMAKE_CURRENT_LIST_DIR}/../Emgu.CV.Extern/cmake/generate_png_prefix.py"
            "${CMAKE_CURRENT_LIST_DIR}/../opencv/3rdparty/libpng/png.h"
            "${EMGU_CV_PNG_PREFIX_HEADER}"
            "emgu_"
    RESULT_VARIABLE EMGU_CV_PNG_PREFIX_GEN_RESULT
    OUTPUT_VARIABLE EMGU_CV_PNG_PREFIX_GEN_OUTPUT
    ERROR_VARIABLE EMGU_CV_PNG_PREFIX_GEN_OUTPUT)
  IF(NOT EMGU_CV_PNG_PREFIX_GEN_RESULT EQUAL 0)
    MESSAGE(FATAL_ERROR "Failed to generate pngprefix.h: ${EMGU_CV_PNG_PREFIX_GEN_OUTPUT}")
  ENDIF()
  MESSAGE(STATUS "EMGU_CV_PNG_PREFIX: ${EMGU_CV_PNG_PREFIX_GEN_OUTPUT}")
ENDIF()
