# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

cmake_minimum_required(VERSION 2.8)
SET(TESSERACT_DIR "tesseract-ocr.git")
SET(TESSERACT_SRC_DIR "${TESSERACT_DIR}/src")
SET(LEPTONICA_DIR "${LEPT_ROOT_DIR}/src")

# Get version with components from VERSION file.
file(STRINGS "${TESSERACT_DIR}/VERSION" VERSION_PLAIN)
string(REGEX REPLACE "^([^.]*)\\..*" "\\1" VERSION_MAJOR ${VERSION_PLAIN})
string(REGEX REPLACE "^[^.]*\\.([^.]*)\\..*" "\\1" VERSION_MINOR ${VERSION_PLAIN})
string(REGEX REPLACE "^[^.]*\\.[^.]*\\.([0-9]*).*" "\\1" VERSION_PATCH ${VERSION_PLAIN})
if(EXISTS ${CMAKE_CURRENT_SOURCE_DIR}/.git)
    execute_process(COMMAND git --git-dir ${CMAKE_CURRENT_SOURCE_DIR}/.git describe --abbrev=4
        OUTPUT_VARIABLE GIT_REV)
        string(REGEX REPLACE "\n$" "" PACKAGE_VERSION "${GIT_REV}")
endif()
if(NOT PACKAGE_VERSION)
    set(PACKAGE_VERSION ${VERSION_PLAIN})
endif()

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
  set_target_properties(${PROJECT_NAME} PROPERTIES 
	FOLDER "tesseract"
    C_STANDARD 11
    C_STANDARD_REQUIRED ON
    CXX_STANDARD 11
    CXX_STANDARD_REQUIRED ON)

  IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
    set_target_properties(${PROJECT_NAME} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
  ENDIF()  
ENDMACRO()

#ccutil
PROJECT(tesseract_ccutil)
# Get version with components from VERSION file.
file(STRINGS "${TESSERACT_DIR}/VERSION" VERSION_PLAIN)
string(REGEX REPLACE "^([^.]*)\\..*" "\\1" VERSION_MAJOR ${VERSION_PLAIN})
string(REGEX REPLACE "^[^.]*\\.([^.]*)\\..*" "\\1" VERSION_MINOR ${VERSION_PLAIN})
string(REGEX REPLACE "^[^.]*\\.[^.]*\\.([0-9]*).*" "\\1" VERSION_PATCH ${VERSION_PLAIN})

# Provide also same macro names as autoconf (see configure.ac).
set(GENERIC_MAJOR_VERSION ${VERSION_MAJOR})
set(GENERIC_MINOR_VERSION ${VERSION_MINOR})
set(GENERIC_MICRO_VERSION ${VERSION_PATCH})
SET(PACKAGE_VERSION ${VERSION_PLAIN})
#MESSAGE(STATUS "tesseract version source file: ${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/tess_version.h.in")
#MESSAGE(STATUS "tesseract version output file: ${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/tess_version.h")
configure_file(
    ${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/tess_version.h.in
    ${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/tess_version.h @ONLY)
SET_SOURCE_FILES_PROPERTIES(${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/ccutil/tessdatamanager.cpp  PROPERTIES COMPILE_DEFINITIONS PACKAGE_VERSION="${PACKAGE_VERSION}")
#add_definitions(-DPACKAGE_VERSION="${PACKAGE_VERSION}")	
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  #include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
  
  IF(NETFX_CORE)
    add_definitions(-D_CRT_SECURE_NO_WARNINGS) 
	IF("8.1" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		add_definitions(-FI"${CMAKE_CURRENT_SOURCE_DIR}/${TESSERACT_DIR}/../windows_runtime_macro_81.h")
	ELSEIF("10.0" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		add_definitions(-FI"${CMAKE_CURRENT_SOURCE_DIR}/${TESSERACT_DIR}/../windows_runtime_macro_100.h")
	ENDIF()
  ENDIF()
ENDIF()

file(GLOB tesseract_ccutil_srcs "${TESSERACT_SRC_DIR}/ccutil/*.cpp")
#file(GLOB tesseract_ccutil_srcs_excludes "${TESSERACT_SRC_DIR}/ccutil/scanutils.cpp")
#LIST(REMOVE_ITEM tesseract_ccutil_srcs ${tesseract_ccutil_srcs_excludes})
file(GLOB tesseract_ccutil_hdrs "${TESSERACT_SRC_DIR}/ccutil/*.h")
IF(ANDROID)
  LIST(APPEND tesseract_ccutil_hdrs "${CMAKE_CURRENT_SOURCE_DIR}/glob/android/glob.h")
  LIST(APPEND tesseract_ccutil_srcs "${CMAKE_CURRENT_SOURCE_DIR}/glob/android/glob.c")
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/glob/android")
ENDIF()

source_group("Src" FILES ${tesseract_ccutil_srcs})
source_group("Include" FILES ${tesseract_ccutil_hdrs})

IF(MSVC)
  #include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
  include_directories("${TESSERACT_SRC_DIR}/api/")
  add_definitions(-D__MSW32__ -W0 -DWINDLLNAME="\\"tesseract\\"")
ENDIF()

add_library(${PROJECT_NAME} STATIC ${tesseract_ccutil_srcs} ${tesseract_ccutil_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

#viewer
PROJECT(tesseract_viewer)
file(GLOB tesseract_viewer_srcs "${TESSERACT_SRC_DIR}/viewer/*.cpp")
file(GLOB tesseract_viewer_srcs_excludes "${TESSERACT_SRC_DIR}/viewer/svpaint.cpp")
LIST(REMOVE_ITEM tesseract_viewer_srcs ${tesseract_viewer_srcs_excludes})
file(GLOB tesseract_viewer_hdrs "${TESSERACT_SRC_DIR}/viewer/*.h")
source_group("Src" FILES ${tesseract_viewer_srcs})
source_group("Include" FILES ${tesseract_viewer_hdrs})
add_definitions(-DHAVE_LIBLEPT)

IF(MSVC)
  #add_definitions(-D__MSW32__ -W0)
  #include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
  IF(NETFX_CORE)
    add_definitions(-DGRAPHICS_DISABLED)
  ENDIF()
ENDIF()
include_directories("${LEPTONICA_DIR}")
add_library(${PROJECT_NAME} STATIC ${tesseract_viewer_srcs} ${tesseract_viewer_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} libleptonica)
ENDIF()

#cutil
PROJECT(tesseract_cutil)
file(GLOB tesseract_cutil_srcs "${TESSERACT_SRC_DIR}/cutil/*.cpp")
file(GLOB tesseract_cutil_hdrs "${TESSERACT_SRC_DIR}/cutil/*.h")
source_group("Src" FILES ${tesseract_cutil_srcs})
source_group("Include" FILES ${tesseract_cutil_hdrs})
#IF(MSVC)
#  add_definitions(-D__MSW32__ -W0)
#  include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
#ENDIF()
include_directories("${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer")
add_library(${PROJECT_NAME} STATIC ${tesseract_cutil_srcs} ${tesseract_cutil_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_viewer)
ENDIF()

#opencl
PROJECT(tesseract_opencl)
file(GLOB tesseract_opencl_srcs "${TESSERACT_SRC_DIR}/opencl/*.cpp")
file(GLOB tesseract_opencl_hdrs "${TESSERACT_SRC_DIR}/opencl/*.h")
source_group("Src" FILES ${tesseract_opencl_srcs})
source_group("Include" FILES ${tesseract_opencl_hdrs})
add_definitions(-DHAVE_LIBLEPT)
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)

  IF(TESSERACT_OPENCL)
    add_definitions(-DUSE_OPENCL)
    #include_directories("${OPENCV_SUBFOLDER}/3rdparty/libtiff")
    include_directories("${CMAKE_SOURCE_DIR}/lib/opencl-icd-1.2.11.0/icd/inc")
  ENDIF()
  #include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${LEPTONICA_DIR}")
add_library(${PROJECT_NAME} STATIC ${tesseract_opencl_srcs} ${tesseract_opencl_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_viewer libleptonica)
ENDIF()

#ccstruct
PROJECT(tesseract_ccstruct)
file(GLOB tesseract_ccstruct_srcs "${TESSERACT_SRC_DIR}/ccstruct/*.cpp")
file(GLOB tesseract_ccstruct_hdrs "${TESSERACT_SRC_DIR}/ccstruct/*.h")
source_group("Src" FILES ${tesseract_ccstruct_srcs})
source_group("Include" FILES ${tesseract_ccstruct_hdrs})
#IF(MSVC)
#  add_definitions(-D__MSW32__ -W0)
#  include_directories("${TESSERACT_SRC_DIR}/vs2010/include" "${TESSERACT_SRC_DIR}/vs2010/port")
#ENDIF()
include_directories("${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/opencl" "${LEPTONICA_DIR}")
add_library(${PROJECT_NAME} STATIC ${tesseract_ccstruct_srcs} ${tesseract_ccstruct_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_cutil tesseract_viewer tesseract_opencl libleptonica)
ENDIF()

#dict
PROJECT(tesseract_dict)
file(GLOB tesseract_dict_srcs "${TESSERACT_SRC_DIR}/dict/*.cpp")
file(GLOB tesseract_dict_hdrs "${TESSERACT_SRC_DIR}/dict/*.h")
source_group("Src" FILES ${tesseract_dict_srcs})
source_group("Include" FILES ${tesseract_dict_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/opencl")
add_library(${PROJECT_NAME} STATIC ${tesseract_dict_srcs} ${tesseract_dict_hdrs})
SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})
IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl)
ENDIF()

#classify
PROJECT(tesseract_classify)
file(GLOB tesseract_classify_srcs "${TESSERACT_SRC_DIR}/classify/*.cpp")
file(GLOB tesseract_classify_hdrs "${TESSERACT_SRC_DIR}/classify/*.h")
source_group("Src" FILES ${tesseract_classify_srcs})
source_group("Include" FILES ${tesseract_classify_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/opencl" "${TESSERACT_SRC_DIR}/dict")
add_library(${PROJECT_NAME} STATIC ${tesseract_classify_srcs} ${tesseract_classify_hdrs})
SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})
IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict)
ENDIF()

#wordrec
PROJECT(tesseract_wordrec)
file(GLOB tesseract_wordrec_srcs "${TESSERACT_SRC_DIR}/wordrec/*.cpp")
file(GLOB tesseract_wordrec_hdrs "${TESSERACT_SRC_DIR}/wordrec/*.h")
source_group("Src" FILES ${tesseract_wordrec_srcs})
source_group("Include" FILES ${tesseract_wordrec_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/opencl" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify")
add_library(${PROJECT_NAME} STATIC ${tesseract_wordrec_srcs} ${tesseract_wordrec_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify)
ENDIF()

#textord
PROJECT(tesseract_textord)
file(GLOB tesseract_textord_srcs "${TESSERACT_SRC_DIR}/textord/*.cpp")
file(GLOB tesseract_textord_hdrs "${TESSERACT_SRC_DIR}/textord/*.h")
source_group("Src" FILES ${tesseract_textord_srcs})
source_group("Include" FILES ${tesseract_textord_hdrs})
add_definitions(-DHAVE_LIBLEPT)
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_SRC_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/opencl" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_SRC_DIR}/ccmain" "${TESSERACT_SRC_DIR}/wordrec")
add_library(${PROJECT_NAME} STATIC ${tesseract_textord_srcs} ${tesseract_textord_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify libleptonica tesseract_wordrec)
ENDIF()

#arch
PROJECT(tesseract_arch)
set(AVX_OPT OFF)
set(AVX2_OPT OFF)
set(FMA_OPT OFF)
set(SSE41_OPT OFF)
SET(tesseract_arch_srcs "${TESSERACT_SRC_DIR}/arch/dotproduct.cpp" "${TESSERACT_SRC_DIR}/arch/intsimdmatrix.cpp" "${TESSERACT_SRC_DIR}/arch/simddetect.cpp")
if(AVX_OPT)
   list(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/dotproductavx.cpp)
   set_source_files_properties(${TESSERACT_SRC_DIR}/arch/dotproductavx.cpp PROPERTIES COMPILE_FLAGS "-DAVX")
endif(AVX_OPT)
if(AVX2_OPT)
   list(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/intsimdmatrixavx2.cpp)
   set_source_files_properties(${TESSERACT_SRC_DIR}/arch/intsimdmatrixavx2.cpp PROPERTIES COMPILE_FLAGS "-DAVX2")
endif(AVX2_OPT)
if(FMA_OPT)
   list(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/dotproductfma.cpp)
   set_source_files_properties(${TESSERACT_SRC_DIR}/arch/dotproductfma.cpp PROPERTIES COMPILE_FLAGS "-mfma")
endif(FMA_OPT)
if(SSE41_OPT)
   list(APPEND tesseract_arch_srcs ${TESSERACT_SRC_DIR}/arch/dotproductsse.cpp ${TESSERACT_SRC_DIR}/arch/intsimdmatrixsse.cpp)
   set_source_files_properties(${TESSERACT_SRC_DIR}/arch/dotproductsse.cpp ${TESSERACT_SRC_DIR}/arch/intsimdmatrixsse.cpp PROPERTIES COMPILE_FLAGS "-DSSE4_1 -msse4.1")
endif(SSE41_OPT)
file(GLOB tesseract_arch_hdrs "${TESSERACT_SRC_DIR}/arch/*.h")
source_group("Src" FILES ${tesseract_arch_srcs})
source_group("Include" FILES ${tesseract_arch_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
#IF(MSVC)
#  LIST(APPEND tesseract_arch_srcs "${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/vs2010/port/gettimeofday.cpp")
#  add_definitions(-D__MSW32__ -W0)
#  include_directories("${TESSERACT_SRC_DIR}/vs2010/include" "${TESSERACT_SRC_DIR}/vs2010/port")
#ENDIF()
add_library(${PROJECT_NAME} STATIC ${tesseract_arch_srcs} ${tesseract_arch_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

#lstm
PROJECT(tesseract_lstm)
file(GLOB tesseract_lstm_srcs "${TESSERACT_SRC_DIR}/lstm/*.cpp")
file(GLOB tesseract_lstm_hdrs "${TESSERACT_SRC_DIR}/lstm/*.h")
source_group("Src" FILES ${tesseract_lstm_srcs})
source_group("Include" FILES ${tesseract_lstm_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
#IF(MSVC)
#  LIST(APPEND tesseract_lstm_srcs "${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/vs2010/port/gettimeofday.cpp")
#  add_definitions(-D__MSW32__ -W0)
#  include_directories("${TESSERACT_SRC_DIR}/vs2010/include" "${TESSERACT_SRC_DIR}/vs2010/port")
#ENDIF()
include_directories("${TESSERACT_SRC_DIR}/arch")
add_library(${PROJECT_NAME} STATIC ${tesseract_lstm_srcs} ${tesseract_lstm_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_arch)
ENDIF()

#ccmain
PROJECT(tesseract_ccmain)
file(GLOB tesseract_ccmain_srcs "${TESSERACT_SRC_DIR}/ccmain/*.cpp")
file(GLOB tesseract_ccmain_hdrs "${TESSERACT_SRC_DIR}/ccmain/*.h")
source_group("Src" FILES ${tesseract_ccmain_srcs})
source_group("Include" FILES ${tesseract_ccmain_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
#IF(MSVC)
#  LIST(APPEND tesseract_ccmain_srcs "${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/vs2010/port/gettimeofday.cpp")
#  add_definitions(-D__MSW32__ -W0)
#  include_directories("${TESSERACT_SRC_DIR}/vs2010/include" "${TESSERACT_SRC_DIR}/vs2010/port")
#ENDIF()
include_directories("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/opencl" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_SRC_DIR}/wordrec" "${TESSERACT_SRC_DIR}/textord" "${TESSERACT_SRC_DIR}/lstm")
add_library(${PROJECT_NAME} STATIC ${tesseract_ccmain_srcs} ${tesseract_ccmain_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify libleptonica tesseract_wordrec tesseract_textord tesseract_lstm)
ENDIF()

#api
PROJECT(tesseract_api)	
file(READ "${TESSERACT_DIR}/VERSION" TESSERACT_VERSION)
STRING(STRIP "${TESSERACT_VERSION}" TESSERACT_VERSION)
file(WRITE "${TESSERACT_SRC_DIR}/api/config_auto.h" "#define PACKAGE_VERSION \"${TESSERACT_VERSION}\"")
file(GLOB tesseract_api_srcs "${TESSERACT_SRC_DIR}/api/*.cpp")
file(GLOB tesseract_api_srcs_excludes "${TESSERACT_SRC_DIR}/api/tesseractmain.cpp" "${TESSERACT_SRC_DIR}/api/capi.cpp")
LIST(REMOVE_ITEM tesseract_api_srcs ${tesseract_api_srcs_excludes})
file(GLOB tesseract_api_hdrs "${TESSERACT_SRC_DIR}/api/*.h")
file(GLOB tesseract_api_hdrs_excludes "${TESSERACT_SRC_DIR}/api/capi.h")
LIST(REMOVE_ITEM tesseract_api_hdrs ${tesseract_api_hdrs_excludes})
#IF(ANDROID)
#  LIST(APPEND tesseract_api_hdrs "${CMAKE_CURRENT_SOURCE_DIR}/glob/android/glob.h")
#  LIST(APPEND tesseract_api_srcs "${CMAKE_CURRENT_SOURCE_DIR}/glob/android/glob.c")
#  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/glob/android")
#ENDIF()
source_group("Src" FILES ${tesseract_api_srcs})
source_group("Include" FILES ${tesseract_api_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
#IF(MSVC)
#  add_definitions(-D__MSW32__ -W0)
#  include_directories("${TESSERACT_SRC_DIR}/vs2008/include" "${TESSERACT_SRC_DIR}/vs2008/port")
#ENDIF()
include_directories("${TESSERACT_SRC_DIR}/cutil" "${TESSERACT_SRC_DIR}/ccstruct" "${TESSERACT_SRC_DIR}/ccutil" "${TESSERACT_SRC_DIR}/viewer" "${TESSERACT_SRC_DIR}/opencl" "${TESSERACT_SRC_DIR}/dict" "${TESSERACT_SRC_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_SRC_DIR}/ccmain" "${TESSERACT_SRC_DIR}/wordrec" "${TESSERACT_SRC_DIR}/textord")
SET_SOURCE_FILES_PROPERTIES(${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/baseapi.cpp  PROPERTIES COMPILE_DEFINITIONS PACKAGE_VERSION="${PACKAGE_VERSION}")
SET_SOURCE_FILES_PROPERTIES(${PROJECT_SOURCE_DIR}/${TESSERACT_SRC_DIR}/api/renderer.cpp  PROPERTIES COMPILE_DEFINITIONS PACKAGE_VERSION="${PACKAGE_VERSION}")

add_library(${PROJECT_NAME} STATIC ${tesseract_api_srcs} ${tesseract_api_hdrs})
IF(NOT MSVC)
  set_property(
  SOURCE "${TESSERACT_SRC_DIR}/api/baseapi.cpp"
  APPEND PROPERTY COMPILE_FLAGS -DVERSION=0
  )
ENDIF()

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF(NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccmain tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify libleptonica tesseract_wordrec tesseract_textord)
ENDIF()




