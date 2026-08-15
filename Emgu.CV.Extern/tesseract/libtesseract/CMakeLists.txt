# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------
SET(TESSERACT_DIR "tesseract-ocr.git")
SET(TESSERACT_SRC_DIR "${TESSERACT_DIR}/src")
SET(TESSERACT_INCLUDE_DIR "${TESSERACT_DIR}/include")
SET(LEPTONICA_DIR "${LEPT_ROOT_DIR}/src")

# Get version with components from VERSION file.
FILE(STRINGS "${TESSERACT_DIR}/VERSION" VERSION_PLAIN)
STRING(REGEX REPLACE "^([^.]*)\\..*" "\\1" VERSION_MAJOR ${VERSION_PLAIN})
STRING(REGEX REPLACE "^[^.]*\\.([^.]*)\\..*" "\\1" VERSION_MINOR ${VERSION_PLAIN})
STRING(REGEX REPLACE "^[^.]*\\.[^.]*\\.([0-9]*).*" "\\1" VERSION_PATCH ${VERSION_PLAIN})
IF(EXISTS ${CMAKE_CURRENT_SOURCE_DIR}/.git)
    EXECUTE_PROCESS(COMMAND git --git-dir ${CMAKE_CURRENT_SOURCE_DIR}/.git describe --abbrev=4
        OUTPUT_VARIABLE GIT_REV)
        STRING(REGEX REPLACE "\n$" "" PACKAGE_VERSION "${GIT_REV}")
ENDIF()
IF(NOT PACKAGE_VERSION)
    SET(PACKAGE_VERSION ${VERSION_PLAIN})
ENDIF()

IF(WIN32 AND CV_ICC)
  STRING(REGEX REPLACE "/O3" "/O2" CMAKE_C_FLAGS_RELEASE ${CMAKE_C_FLAGS_RELEASE})
  STRING(REGEX REPLACE "/O3" "/O2" CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
ENDIF()

IF (NOT (WIN32 OR IOS OR ANDROID))
  SET( CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fPIC" )
  SET( CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -fPIC" )
ENDIF()

INSTALL(
  FILES
  "${TESSERACT_DIR}/LICENSE"
  DESTINATION "lib"
  COMPONENT emgucv_source
  RENAME "tesseract-ocr.license.txt"
  )

#leptionica
ADD_SUBDIRECTORY(leptonica)

MACRO(SET_TESSERACT_PROJECT_PROPERTY PROJ_NAME)
  SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES
	FOLDER "tesseract"
    C_STANDARD 17
    C_STANDARD_REQUIRED ON
    CXX_STANDARD 17
    CXX_STANDARD_REQUIRED ON)
  IF(WIN32)
    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")
  ENDIF()
  IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
  ENDIF()
ENDMACRO()

IF(ANDROID)
  INCLUDE_DIRECTORIES("${CPUFEATURES_ROOT}")
ENDIF()

#ccutil
PROJECT(tesseract_ccutil)
# Get version with components from VERSION file.
FILE(STRINGS "${TESSERACT_DIR}/VERSION" VERSION_PLAIN)
STRING(REGEX REPLACE "^([^.]*)\\..*" "\\1" VERSION_MAJOR ${VERSION_PLAIN})
STRING(REGEX REPLACE "^[^.]*\\.([^.]*)\\..*" "\\1" VERSION_MINOR ${VERSION_PLAIN})
STRING(REGEX REPLACE "^[^.]*\\.[^.]*\\.([0-9]*).*" "\\1" VERSION_PATCH ${VERSION_PLAIN})

# Provide also same macro names as autoconf (see configure.ac).
SET(GENERIC_MAJOR_VERSION ${VERSION_MAJOR})
SET(GENERIC_MINOR_VERSION ${VERSION_MINOR})
SET(GENERIC_MICRO_VERSION ${VERSION_PATCH})
SET(PACKAGE_VERSION ${VERSION_PLAIN})
CONFIGURE_FILE(
    ${PROJECT_SOURCE_DIR}/${TESSERACT_INCLUDE_DIR}/tesseract/version.h.in
    ${PROJECT_SOURCE_DIR}/${TESSERACT_INCLUDE_DIR}/tesseract/version.h @ONLY)
SET_SOURCE_FILES_PROPERTIES(${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/ccutil/tessdatamanager.cpp  PROPERTIES COMPILE_DEFINITIONS PACKAGE_VERSION="${PACKAGE_VERSION}")
IF(MSVC)
  ADD_DEFINITIONS(-D__MSW32__ -W0)

  IF(NETFX_CORE)
    ADD_DEFINITIONS(-D_CRT_SECURE_NO_WARNINGS)
	IF("8.1" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		ADD_DEFINITIONS(-FI"${CMAKE_CURRENT_SOURCE_DIR}/${TESSERACT_DIR}/../windows_runtime_macro_81.h")
	ELSEIF("10.0" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		ADD_DEFINITIONS(-FI"${CMAKE_CURRENT_SOURCE_DIR}/${TESSERACT_DIR}/../windows_runtime_macro_100.h")
	ENDIF()
  ENDIF()
ENDIF()

FILE(GLOB tesseract_ccutil_srcs "${TESSERACT_SRC_DIR}/ccutil/*.cpp")
FILE(GLOB tesseract_ccutil_hdrs "${TESSERACT_SRC_DIR}/ccutil/*.h")

INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/${TESSERACT_INCLUDE_DIR}/")

SOURCE_GROUP("Src" FILES ${tesseract_ccutil_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_ccutil_hdrs})

IF(MSVC)
  INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/api/")
  ADD_DEFINITIONS(-D__MSW32__ -W0)
ENDIF()

ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_ccutil_srcs} ${tesseract_ccutil_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

#viewer
PROJECT(tesseract_viewer)
FILE(GLOB tesseract_viewer_srcs "${TESSERACT_SRC_DIR}/viewer/*.cpp")
FILE(GLOB tesseract_viewer_hdrs "${TESSERACT_SRC_DIR}/viewer/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_viewer_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_viewer_hdrs})
ADD_DEFINITIONS(-DHAVE_LIBLEPT)

IF(MSVC)
  #ADD_DEFINITIONS(-D__MSW32__ -W0)
  IF(NETFX_CORE)
    ADD_DEFINITIONS(-DGRAPHICS_DISABLED)
  ENDIF()
ENDIF()
INCLUDE_DIRECTORIES("${LEPTONICA_DIR}")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_viewer_srcs} ${tesseract_viewer_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} libleptonica)
ENDIF()

#cutil
PROJECT(tesseract_cutil)
FILE(GLOB tesseract_cutil_srcs "${TESSERACT_SRC_DIR}/cutil/*.cpp")
FILE(GLOB tesseract_cutil_hdrs "${TESSERACT_SRC_DIR}/cutil/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_cutil_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_cutil_hdrs})
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_cutil_srcs} ${tesseract_cutil_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_viewer)
ENDIF()

# tesseract >= 5.5 removed src/opencl and the target with it.
# The definitions and include directories below used to be set up by that
# target's section and apply to every target that follows.
ADD_DEFINITIONS(-DHAVE_LIBLEPT)
IF(MSVC)
  ADD_DEFINITIONS(-D__MSW32__ -W0)
ENDIF()
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${LEPTONICA_DIR}")

#ccstruct
PROJECT(tesseract_ccstruct)
FILE(GLOB tesseract_ccstruct_srcs "${TESSERACT_SRC_DIR}/ccstruct/*.cpp")
FILE(GLOB tesseract_ccstruct_hdrs "${TESSERACT_SRC_DIR}/ccstruct/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_ccstruct_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_ccstruct_hdrs})
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/cutil" "${LEPTONICA_DIR}")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_ccstruct_srcs} ${tesseract_ccstruct_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_cutil tesseract_viewer libleptonica)
ENDIF()

#dict
PROJECT(tesseract_dict)
FILE(GLOB tesseract_dict_srcs "${TESSERACT_SRC_DIR}/dict/*.cpp")
FILE(GLOB tesseract_dict_hdrs "${TESSERACT_SRC_DIR}/dict/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_dict_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_dict_hdrs})
IF(MSVC)
  ADD_DEFINITIONS(-D__MSW32__ -W0)
  INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_dict_srcs} ${tesseract_dict_hdrs})
SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})
IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer)
ENDIF()

#classify
PROJECT(tesseract_classify)
FILE(GLOB tesseract_classify_srcs "${TESSERACT_SRC_DIR}/classify/*.cpp")
FILE(GLOB tesseract_classify_hdrs "${TESSERACT_SRC_DIR}/classify/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_classify_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_classify_hdrs})
IF(MSVC)
  ADD_DEFINITIONS(-D__MSW32__ -W0)
  INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/dict")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_classify_srcs} ${tesseract_classify_hdrs})
SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})
IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_dict)
ENDIF()

#wordrec
PROJECT(tesseract_wordrec)
FILE(GLOB tesseract_wordrec_srcs "${TESSERACT_SRC_DIR}/wordrec/*.cpp")
FILE(GLOB tesseract_wordrec_hdrs "${TESSERACT_SRC_DIR}/wordrec/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_wordrec_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_wordrec_hdrs})
IF(MSVC)
  ADD_DEFINITIONS(-D__MSW32__ -W0)
  INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_wordrec_srcs} ${tesseract_wordrec_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_dict tesseract_classify)
ENDIF()

#textord
PROJECT(tesseract_textord)
FILE(GLOB tesseract_textord_srcs "${TESSERACT_SRC_DIR}/textord/*.cpp")
FILE(GLOB tesseract_textord_hdrs "${TESSERACT_SRC_DIR}/textord/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_textord_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_textord_hdrs})
ADD_DEFINITIONS(-DHAVE_LIBLEPT)
IF(MSVC)
  ADD_DEFINITIONS(-D__MSW32__ -W0)
  INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_SRC_DIR}/ccmain" "${TESSERACT_SRC_DIR}/wordrec")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_textord_srcs} ${tesseract_textord_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_dict tesseract_classify libleptonica tesseract_wordrec)
ENDIF()

#arch
PROJECT(tesseract_arch)

MESSAGE(STATUS "CPU_BASELINE: ${CPU_BASELINE}")
MESSAGE(STATUS "CPU_BASELINE_FINAL: ${CPU_BASELINE_FINAL}")

SET(AVX_OPT OFF)
IF ("AVX" IN_LIST CPU_BASELINE_FINAL)
  SET(AVX_OPT ON)
  MESSAGE(STATUS "TESSERACT with AVX: ON")
ELSE()
  MESSAGE(STATUS "TESSERACT with AVX: OFF")
ENDIF()

SET(AVX2_OPT OFF)
IF ("AVX2" IN_LIST CPU_BASELINE_FINAL)
  SET(AVX2_OPT ON)
  MESSAGE(STATUS "TESSERACT with AVX2: ON")
ELSE()
  MESSAGE(STATUS "TESSERACT with AVX2: OFF")
ENDIF()

SET(FMA_OPT OFF)

SET(SSE41_OPT OFF)
IF ("SSE4_1" IN_LIST CPU_BASELINE_FINAL)
  SET(SSE41_OPT ON)
  MESSAGE(STATUS "TESSERACT with SSE4_1: ON")
ELSE()
  MESSAGE(STATUS "TESSERACT with SSE4_1: OFF")
ENDIF()

SET(tesseract_arch_srcs "${TESSERACT_SRC_DIR}/arch/dotproduct.cpp" "${TESSERACT_SRC_DIR}/arch/intsimdmatrix.cpp" "${TESSERACT_SRC_DIR}/arch/simddetect.cpp")

SET(NEON_OPT OFF)
IF (("NEON" IN_LIST CPU_BASELINE_FINAL) OR NEON_INTRINSICS OR (IPHONESIMULATOR AND ("${IOS_ARCH}" STREQUAL "arm64")) OR (MAC_CATALYST AND ("${IOS_ARCH}" STREQUAL "arm64")) OR (ANDROID AND ("${ANDROID_ABI}" STREQUAL "arm64-v8a")))
  SET(NEON_OPT ON)
  LIST(APPEND tesseract_arch_srcs "${TESSERACT_SRC_DIR}/arch/dotproductneon.cpp" "${TESSERACT_SRC_DIR}/arch/intsimdmatrixneon.cpp")
  MESSAGE(STATUS "TESSERACT with NEON: ON")
ELSE()
  MESSAGE(STATUS "TESSERACT with NEON: OFF")
ENDIF()

SET(TESSERACT_SIMDDETECT_COMPILE_FLAGS)
IF(AVX_OPT)
   LIST(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/dotproductavx.cpp)
   SET_SOURCE_FILES_PROPERTIES(${TESSERACT_SRC_DIR}/arch/dotproductavx.cpp PROPERTIES COMPILE_FLAGS "-D__AVX__")
   SET(TESSERACT_SIMDDETECT_COMPILE_FLAGS "${TESSERACT_SIMDDETECT_COMPILE_FLAGS} -DHAVE_AVX")
ENDIF()
IF(AVX2_OPT)
   LIST(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/intsimdmatrixavx2.cpp)
   SET_SOURCE_FILES_PROPERTIES(${TESSERACT_SRC_DIR}/arch/intsimdmatrixavx2.cpp PROPERTIES COMPILE_FLAGS "-D__AVX2__")
   SET(TESSERACT_SIMDDETECT_COMPILE_FLAGS "${TESSERACT_SIMDDETECT_COMPILE_FLAGS} -DHAVE_AVX2")
ENDIF()
IF(FMA_OPT)
   LIST(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/dotproductfma.cpp)
   SET_SOURCE_FILES_PROPERTIES(${TESSERACT_SRC_DIR}/arch/dotproductfma.cpp PROPERTIES COMPILE_FLAGS "-mfma")
ENDIF()
IF(SSE41_OPT)
   LIST(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/dotproductsse.cpp ${TESSERACT_SRC_DIR}/arch/intsimdmatrixsse.cpp)
   SET_SOURCE_FILES_PROPERTIES(${TESSERACT_SRC_DIR}/arch/dotproductsse.cpp ${TESSERACT_SRC_DIR}/arch/intsimdmatrixsse.cpp PROPERTIES COMPILE_FLAGS "-D__SSE4_1__ -msse4.1")
   SET(TESSERACT_SIMDDETECT_COMPILE_FLAGS "${TESSERACT_SIMDDETECT_COMPILE_FLAGS} -DHAVE_SSE4_1")
ENDIF()
IF(NEON_OPT)
   LIST(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/dotproductneon.cpp ${TESSERACT_SRC_DIR}/arch/intsimdmatrixneon.cpp)
   IF(CMAKE_SIZEOF_VOID_P EQUAL 4)
      # 32-bit ARM: NEON is not always-on; -mfpu=neon is required per translation unit.
      # Use CMAKE_SIZEOF_VOID_P (compiler target width) not CMAKE_SYSTEM_PROCESSOR
      # because on Pi OS 32-bit with a 64-bit kernel, uname -m reports aarch64.
      SET_SOURCE_FILES_PROPERTIES(${TESSERACT_SRC_DIR}/arch/dotproductneon.cpp ${TESSERACT_SRC_DIR}/arch/intsimdmatrixneon.cpp PROPERTIES COMPILE_FLAGS "-D__ARM_NEON -mfpu=neon")
   else()
      SET_SOURCE_FILES_PROPERTIES(${TESSERACT_SRC_DIR}/arch/dotproductneon.cpp ${TESSERACT_SRC_DIR}/arch/intsimdmatrixneon.cpp PROPERTIES COMPILE_FLAGS "-D__ARM_NEON")
   ENDIF()
   SET(TESSERACT_SIMDDETECT_COMPILE_FLAGS "${TESSERACT_SIMDDETECT_COMPILE_FLAGS} -DHAVE_NEON")
ENDIF()

MESSAGE(STATUS "------------------------------------ TESSERACT_SIMDDETECT_COMPILE_FLAGS: ${TESSERACT_SIMDDETECT_COMPILE_FLAGS} -------------------------------------")
IF(TESSERACT_SIMDDETECT_COMPILE_FLAGS)
  SET_SOURCE_FILES_PROPERTIES(${TESSERACT_SRC_DIR}/arch/simddetect.cpp PROPERTIES COMPILE_FLAGS "${TESSERACT_SIMDDETECT_COMPILE_FLAGS}")
ENDIF()

FILE(GLOB tesseract_arch_hdrs "${TESSERACT_SRC_DIR}/arch/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_arch_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_arch_hdrs})
ADD_DEFINITIONS(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_arch_srcs} ${tesseract_arch_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

#lstm
PROJECT(tesseract_lstm)
FILE(GLOB tesseract_lstm_srcs "${TESSERACT_SRC_DIR}/lstm/*.cpp")
FILE(GLOB tesseract_lstm_hdrs "${TESSERACT_SRC_DIR}/lstm/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_lstm_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_lstm_hdrs})
ADD_DEFINITIONS(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/arch")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_lstm_srcs} ${tesseract_lstm_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_arch)
ENDIF()

#ccmain
PROJECT(tesseract_ccmain)
FILE(GLOB tesseract_ccmain_srcs "${TESSERACT_SRC_DIR}/ccmain/*.cpp")
FILE(GLOB tesseract_ccmain_hdrs "${TESSERACT_SRC_DIR}/ccmain/*.h")
SOURCE_GROUP("Src" FILES ${tesseract_ccmain_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_ccmain_hdrs})
ADD_DEFINITIONS(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_SRC_DIR}/wordrec" "${TESSERACT_SRC_DIR}/textord" "${TESSERACT_SRC_DIR}/lstm")
ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_ccmain_srcs} ${tesseract_ccmain_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_dict tesseract_classify libleptonica tesseract_wordrec tesseract_textord tesseract_lstm)
ENDIF()

#api
PROJECT(tesseract_api)
FILE(READ "${TESSERACT_DIR}/VERSION" TESSERACT_VERSION)
STRING(STRIP "${TESSERACT_VERSION}" TESSERACT_VERSION)
FILE(WRITE "${TESSERACT_SRC_DIR}/api/config_auto.h" "#define PACKAGE_VERSION \"${TESSERACT_VERSION}\"")
FILE(GLOB tesseract_api_srcs "${TESSERACT_SRC_DIR}/api/*.cpp")
FILE(GLOB tesseract_api_srcs_excludes "${TESSERACT_SRC_DIR}/api/tesseractmain.cpp" "${TESSERACT_SRC_DIR}/api/capi.cpp")
LIST(REMOVE_ITEM tesseract_api_srcs ${tesseract_api_srcs_excludes})
FILE(GLOB tesseract_api_hdrs "${TESSERACT_SRC_DIR}/api/*.h" "${TESSERACT_INCLUDE_DIR}/tesseract/baseapi.h" "${TESSERACT_INCLUDE_DIR}/tesseract/renderer.h")

SOURCE_GROUP("Src" FILES ${tesseract_api_srcs})
SOURCE_GROUP("Include" FILES ${tesseract_api_hdrs})
ADD_DEFINITIONS(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
INCLUDE_DIRECTORIES("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_SRC_DIR}/ccmain" "${TESSERACT_SRC_DIR}/wordrec" "${TESSERACT_SRC_DIR}/textord")
SET_SOURCE_FILES_PROPERTIES(${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/baseapi.cpp  PROPERTIES COMPILE_DEFINITIONS PACKAGE_VERSION="${PACKAGE_VERSION}")
SET_SOURCE_FILES_PROPERTIES(${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/renderer.cpp  PROPERTIES COMPILE_DEFINITIONS PACKAGE_VERSION="${PACKAGE_VERSION}")

ADD_LIBRARY(${PROJECT_NAME} STATIC ${tesseract_api_srcs} ${tesseract_api_hdrs})
IF(NOT MSVC)
  SET_PROPERTY(
  SOURCE "${TESSERACT_SRC_DIR}/api/baseapi.cpp"
  APPEND PROPERTY COMPILE_FLAGS -DVERSION=0
  )
ENDIF()

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF(NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccmain tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_dict tesseract_classify libleptonica tesseract_wordrec tesseract_textord)
ENDIF()
