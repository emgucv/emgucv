# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

cmake_minimum_required(VERSION 2.8)
SET(TESSERACT_DIR "tesseract-ocr.git")
SET(LEPTONICA_DIR "${LEPT_ROOT_DIR}/src")
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
  "${TESSERACT_DIR}/COPYING"
  DESTINATION "lib/3rdParty"
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
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include")
  
  IF(NETFX_CORE)
    add_definitions(-D_CRT_SECURE_NO_WARNINGS) 
	IF("8.1" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		add_definitions(-FI"${CMAKE_CURRENT_SOURCE_DIR}/${TESSERACT_DIR}/../windows_runtime_macro_81.h")
	ELSEIF("10.0" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		add_definitions(-FI"${CMAKE_CURRENT_SOURCE_DIR}/${TESSERACT_DIR}/../windows_runtime_macro_100.h")
	ENDIF()
  ENDIF()
ENDIF()

file(GLOB tesseract_ccutil_srcs "${TESSERACT_DIR}/ccutil/*.cpp")
#file(GLOB tesseract_ccutil_srcs_excludes "${TESSERACT_DIR}/ccutil/scanutils.cpp")
#LIST(REMOVE_ITEM tesseract_ccutil_srcs ${tesseract_ccutil_srcs_excludes})
file(GLOB tesseract_ccutil_hdrs "${TESSERACT_DIR}/ccutil/*.h")
source_group("Src" FILES ${tesseract_ccutil_srcs})
source_group("Include" FILES ${tesseract_ccutil_hdrs})

IF(MSVC)
  include_directories("${TESSERACT_DIR}/vs2010/include")
  add_definitions(-D__MSW32__ -W0 -DWINDLLNAME="\\"tesseract\\"")
ENDIF()

add_library(${PROJECT_NAME} STATIC ${tesseract_ccutil_srcs} ${tesseract_ccutil_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

#viewer
PROJECT(tesseract_viewer)
file(GLOB tesseract_viewer_srcs "${TESSERACT_DIR}/viewer/*.cpp")
file(GLOB tesseract_viewer_srcs_excludes "${TESSERACT_DIR}/viewer/svpaint.cpp")
LIST(REMOVE_ITEM tesseract_viewer_srcs ${tesseract_viewer_srcs_excludes})
file(GLOB tesseract_viewer_hdrs "${TESSERACT_DIR}/viewer/*.h")
source_group("Src" FILES ${tesseract_viewer_srcs})
source_group("Include" FILES ${tesseract_viewer_hdrs})
add_definitions(-DHAVE_LIBLEPT)

IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include")
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
file(GLOB tesseract_cutil_srcs "${TESSERACT_DIR}/cutil/*.cpp")
file(GLOB tesseract_cutil_hdrs "${TESSERACT_DIR}/cutil/*.h")
source_group("Src" FILES ${tesseract_cutil_srcs})
source_group("Include" FILES ${tesseract_cutil_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer")
add_library(${PROJECT_NAME} STATIC ${tesseract_cutil_srcs} ${tesseract_cutil_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_viewer)
ENDIF()

#opencl
PROJECT(tesseract_opencl)
file(GLOB tesseract_opencl_srcs "${TESSERACT_DIR}/opencl/*.cpp")
file(GLOB tesseract_opencl_hdrs "${TESSERACT_DIR}/opencl/*.h")
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
  include_directories("${TESSERACT_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${LEPTONICA_DIR}")
add_library(${PROJECT_NAME} STATIC ${tesseract_opencl_srcs} ${tesseract_opencl_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_viewer libleptonica)
ENDIF()

#ccstruct
PROJECT(tesseract_ccstruct)
file(GLOB tesseract_ccstruct_srcs "${TESSERACT_DIR}/ccstruct/*.cpp")
file(GLOB tesseract_ccstruct_hdrs "${TESSERACT_DIR}/ccstruct/*.h")
source_group("Src" FILES ${tesseract_ccstruct_srcs})
source_group("Include" FILES ${tesseract_ccstruct_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include" "${TESSERACT_DIR}/vs2010/port")
ENDIF()
include_directories("${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${TESSERACT_DIR}/cutil" "${TESSERACT_DIR}/opencl" "${LEPTONICA_DIR}")
add_library(${PROJECT_NAME} STATIC ${tesseract_ccstruct_srcs} ${tesseract_ccstruct_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_cutil tesseract_viewer tesseract_opencl libleptonica)
ENDIF()

#dict
PROJECT(tesseract_dict)
file(GLOB tesseract_dict_srcs "${TESSERACT_DIR}/dict/*.cpp")
file(GLOB tesseract_dict_hdrs "${TESSERACT_DIR}/dict/*.h")
source_group("Src" FILES ${tesseract_dict_srcs})
source_group("Include" FILES ${tesseract_dict_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_DIR}/cutil" "${TESSERACT_DIR}/ccstruct" "${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${TESSERACT_DIR}/opencl")
add_library(${PROJECT_NAME} STATIC ${tesseract_dict_srcs} ${tesseract_dict_hdrs})
SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})
IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl)
ENDIF()

#classify
PROJECT(tesseract_classify)
file(GLOB tesseract_classify_srcs "${TESSERACT_DIR}/classify/*.cpp")
file(GLOB tesseract_classify_hdrs "${TESSERACT_DIR}/classify/*.h")
source_group("Src" FILES ${tesseract_classify_srcs})
source_group("Include" FILES ${tesseract_classify_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_DIR}/cutil" "${TESSERACT_DIR}/ccstruct" "${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${TESSERACT_DIR}/opencl" "${TESSERACT_DIR}/dict")
add_library(${PROJECT_NAME} STATIC ${tesseract_classify_srcs} ${tesseract_classify_hdrs})
SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})
IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict)
ENDIF()

#wordrec
PROJECT(tesseract_wordrec)
file(GLOB tesseract_wordrec_srcs "${TESSERACT_DIR}/wordrec/*.cpp")
file(GLOB tesseract_wordrec_hdrs "${TESSERACT_DIR}/wordrec/*.h")
source_group("Src" FILES ${tesseract_wordrec_srcs})
source_group("Include" FILES ${tesseract_wordrec_hdrs})
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_DIR}/cutil" "${TESSERACT_DIR}/ccstruct" "${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${TESSERACT_DIR}/opencl" "${TESSERACT_DIR}/dict" "${TESSERACT_DIR}/classify")
add_library(${PROJECT_NAME} STATIC ${tesseract_wordrec_srcs} ${tesseract_wordrec_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify)
ENDIF()

#textord
PROJECT(tesseract_textord)
file(GLOB tesseract_textord_srcs "${TESSERACT_DIR}/textord/*.cpp")
file(GLOB tesseract_textord_hdrs "${TESSERACT_DIR}/textord/*.h")
source_group("Src" FILES ${tesseract_textord_srcs})
source_group("Include" FILES ${tesseract_textord_hdrs})
add_definitions(-DHAVE_LIBLEPT)
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include")
ENDIF()
include_directories("${TESSERACT_DIR}/cutil" "${TESSERACT_DIR}/ccstruct" "${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${TESSERACT_DIR}/opencl" "${TESSERACT_DIR}/dict" "${TESSERACT_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_DIR}/ccmain" "${TESSERACT_DIR}/wordrec")
add_library(${PROJECT_NAME} STATIC ${tesseract_textord_srcs} ${tesseract_textord_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify libleptonica tesseract_wordrec)
ENDIF()

#arch
PROJECT(tesseract_arch)
file(GLOB tesseract_arch_srcs "${TESSERACT_DIR}/arch/*.cpp")
file(GLOB tesseract_arch_hdrs "${TESSERACT_DIR}/arch/*.h")
source_group("Src" FILES ${tesseract_arch_srcs})
source_group("Include" FILES ${tesseract_arch_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
IF(MSVC)
  LIST(APPEND tesseract_arch_srcs "${PROJECT_SOURCE_DIR}/${TESSERACT_DIR}/vs2010/port/gettimeofday.cpp")
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include" "${TESSERACT_DIR}/vs2010/port")
ENDIF()
add_library(${PROJECT_NAME} STATIC ${tesseract_arch_srcs} ${tesseract_arch_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

#lstm
PROJECT(tesseract_lstm)
file(GLOB tesseract_lstm_srcs "${TESSERACT_DIR}/lstm/*.cpp")
file(GLOB tesseract_lstm_hdrs "${TESSERACT_DIR}/lstm/*.h")
source_group("Src" FILES ${tesseract_lstm_srcs})
source_group("Include" FILES ${tesseract_lstm_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
IF(MSVC)
  LIST(APPEND tesseract_lstm_srcs "${PROJECT_SOURCE_DIR}/${TESSERACT_DIR}/vs2010/port/gettimeofday.cpp")
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include" "${TESSERACT_DIR}/vs2010/port")
ENDIF()
include_directories("${TESSERACT_DIR}/arch")
add_library(${PROJECT_NAME} STATIC ${tesseract_lstm_srcs} ${tesseract_lstm_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_arch)
ENDIF()

#ccmain
PROJECT(tesseract_ccmain)
file(GLOB tesseract_ccmain_srcs "${TESSERACT_DIR}/ccmain/*.cpp")
file(GLOB tesseract_ccmain_hdrs "${TESSERACT_DIR}/ccmain/*.h")
source_group("Src" FILES ${tesseract_ccmain_srcs})
source_group("Include" FILES ${tesseract_ccmain_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
IF(MSVC)
  LIST(APPEND tesseract_ccmain_srcs "${PROJECT_SOURCE_DIR}/${TESSERACT_DIR}/vs2010/port/gettimeofday.cpp")
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2010/include" "${TESSERACT_DIR}/vs2010/port")
ENDIF()
include_directories("${TESSERACT_DIR}/cutil" "${TESSERACT_DIR}/ccstruct" "${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${TESSERACT_DIR}/opencl" "${TESSERACT_DIR}/dict" "${TESSERACT_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_DIR}/wordrec" "${TESSERACT_DIR}/textord" "${TESSERACT_DIR}/lstm")
add_library(${PROJECT_NAME} STATIC ${tesseract_ccmain_srcs} ${tesseract_ccmain_hdrs})

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF (NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify libleptonica tesseract_wordrec tesseract_textord tesseract_lstm)
ENDIF()

#api
PROJECT(tesseract_api)
file(GLOB tesseract_api_srcs "${TESSERACT_DIR}/api/*.cpp")
file(GLOB tesseract_api_srcs_excludes "${TESSERACT_DIR}/api/tesseractmain.cpp" "${TESSERACT_DIR}/api/capi.cpp")
LIST(REMOVE_ITEM tesseract_api_srcs ${tesseract_api_srcs_excludes})
file(GLOB tesseract_api_hdrs "${TESSERACT_DIR}/api/*.h")
file(GLOB tesseract_api_hdrs_excludes "${TESSERACT_DIR}/api/capi.h")
LIST(REMOVE_ITEM tesseract_api_hdrs ${tesseract_api_hdrs_excludes})
#IF(ANDROID)
#  LIST(APPEND tesseract_api_hdrs "${CMAKE_CURRENT_SOURCE_DIR}/glob/android/glob.h")
#  LIST(APPEND tesseract_api_srcs "${CMAKE_CURRENT_SOURCE_DIR}/glob/android/glob.c")
#  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/glob/android")
#ENDIF()
source_group("Src" FILES ${tesseract_api_srcs})
source_group("Include" FILES ${tesseract_api_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -DHAVE_LIBLEPT)
IF(MSVC)
  add_definitions(-D__MSW32__ -W0)
  include_directories("${TESSERACT_DIR}/vs2008/include" "${TESSERACT_DIR}/vs2008/port")
ENDIF()
include_directories("${TESSERACT_DIR}/cutil" "${TESSERACT_DIR}/ccstruct" "${TESSERACT_DIR}/ccutil" "${TESSERACT_DIR}/viewer" "${TESSERACT_DIR}/opencl" "${TESSERACT_DIR}/dict" "${TESSERACT_DIR}/classify" "${LEPTONICA_DIR}" "${TESSERACT_DIR}/ccmain" "${TESSERACT_DIR}/wordrec" "${TESSERACT_DIR}/textord")
add_library(${PROJECT_NAME} STATIC ${tesseract_api_srcs} ${tesseract_api_hdrs})
IF(NOT MSVC)
  set_property(
  SOURCE "${TESSERACT_DIR}/api/baseapi.cpp"
  APPEND PROPERTY COMPILE_FLAGS -DVERSION=0
  )
ENDIF()

SET_TESSERACT_PROJECT_PROPERTY(${PROJECT_NAME})

IF(NOT MSVC)
  ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccmain tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_opencl tesseract_dict tesseract_classify libleptonica tesseract_wordrec tesseract_textord)
ENDIF()




