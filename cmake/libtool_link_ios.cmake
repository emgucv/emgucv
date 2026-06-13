# Helper script for iOS/Catalyst static library linking.
# Invoked via cmake -P with the following -D variables:
#   ARCH                - target architecture (e.g. arm64)
#   OUTPUT              - output .a path
#   BIN_DIR             - optional directory containing additional .a files
#   OPENCV_MODULES_DIR  - opencv module Release lib dir
#   OPENCV_3RDPARTY_DIR - opencv 3rdparty Release lib dir
#   RUNTIME_DIR         - cmake runtime output dir (contains cvextern .a)
#   WITH_TESSERACT      - TRUE/FALSE
#   TESSERACT_DIR       - tesseract libs Release dir (when WITH_TESSERACT)
#   FREETYPE_LIBS       - optional semicolon-separated list of freetype .a paths
#   HARFBUZZ_LIBS       - optional semicolon-separated list of harfbuzz .a paths

file(GLOB BIN_LIBS          "${BIN_DIR}/*.a")
file(GLOB OPENCV_MODULE_LIBS "${OPENCV_MODULES_DIR}/*.a")
file(GLOB OPENCV_3RD_LIBS   "${OPENCV_3RDPARTY_DIR}/*.a")
file(GLOB RUNTIME_LIBS      "${RUNTIME_DIR}/*.a")

set(ALL_LIBS ${BIN_LIBS} ${OPENCV_MODULE_LIBS} ${OPENCV_3RD_LIBS} ${RUNTIME_LIBS})

if(WITH_TESSERACT)
  file(GLOB TESSERACT_LIBS "${TESSERACT_DIR}/*.a")
  list(APPEND ALL_LIBS ${TESSERACT_LIBS})
endif()

if(FREETYPE_LIBS)
  list(APPEND ALL_LIBS ${FREETYPE_LIBS})
endif()
if(HARFBUZZ_LIBS)
  list(APPEND ALL_LIBS ${HARFBUZZ_LIBS})
endif()

if(NOT ALL_LIBS)
  message(FATAL_ERROR "libtool_link_ios: no input .a files found")
endif()

execute_process(
  COMMAND libtool -static -arch_only ${ARCH} -o "${OUTPUT}" ${ALL_LIBS}
  RESULT_VARIABLE result
)
if(NOT result EQUAL 0)
  message(FATAL_ERROR "libtool failed with exit code: ${result}")
endif()
