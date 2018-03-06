# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

PROJECT(libleptonica)

#IF(MSVC)
#enable_language(CXX)
#ENDIF()

SET(LEPTON_DIR ${LEPT_ROOT_DIR})
MESSAGE(STATUS "LEPTON_DIR: ${LEPTON_DIR}")
set(CMAKE_MODULE_PATH "${CMAKE_MODULE_PATH};${LEPT_ROOT_DIR}/cmake")

set(AUTOCONFIG_SRC ${LEPT_ROOT_DIR}/config_auto.h.in)
set(AUTOCONFIG ${LEPT_ROOT_DIR}/src/config_auto.h)
include(Configure)
 
file(GLOB lepton_srcs "${LEPTON_DIR}/src/*.c")
file(GLOB lepton_srcs_excludes "${LEPTON_DIR}/src/freetype.c" "${LEPTON_DIR}/src/endiantest.c" "${LEPTON_DIR}/src/xtractprotos.c" "${LEPTON_DIR}/src/leptwin.c"  )
LIST(REMOVE_ITEM lepton_srcs ${lepton_srcs_excludes})
file(GLOB lepton_hdrs "${LEPTON_DIR}/src/*.h")

IF(MSVC)
  SET_SOURCE_FILES_PROPERTIES( ${lepton_srcs} PROPERTIES LANGUAGE CXX)
ENDIF()

IF(NETFX_CORE)
	IF("8.1" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		add_definitions(-FI"${LEPTON_DIR}/../windows_runtime_macro_81.h")
	ELSEIF("10.0" STREQUAL "${CMAKE_SYSTEM_VERSION}")
		add_definitions(-FI"${LEPTON_DIR}/../windows_runtime_macro_100.h")
	ENDIF()
 
 add_definitions(-sdl- -UUNICODE -U_UNICODE )
ENDIF()

source_group("Src" FILES ${lepton_srcs})
source_group("Include" FILES ${lepton_hdrs})
set(the_target ${PROJECT_NAME})

add_definitions(-DL_LITTLE_ENDIAN -DXMD_H -DNO_CONSOLE_IO)
IF (MSVC)
  add_definitions(-W0)
ENDIF()

IF (ANDROID)
	add_definitions(-DANDROID_BUILD)
	#MESSAGE("Leptonica HAVE_FMEMOPEN: ${HAVE_FMEMOPEN}")
	#remove_definitions(-DHAVE_FMEMOPEN)
ELSEIF(IOS OR APPLE)
	add_definitions(-DOS_IOS)
ENDIF()

if (NOT (ANDROID OR APPLE))
  IF(WITH_JPEG AND BUILD_JPEG)
    SET(LEPT_USE_JPEG TRUE)
  ELSE()
    SET(LEPT_USE_JPEG FALSE)
  ENDIF()
  MESSAGE(STATUS "leptonica HAVE_LIBJPEG: ${LEPT_USE_JPEG}")
  if(LEPT_USE_JPEG)
    add_definitions(-DHAVE_LIBJPEG)
    include_directories(${OPENCV_SUBFOLDER}/3rdparty/libjpeg)
  endif()
endif()

IF(WITH_PNG AND BUILD_PNG) 	
  SET(LEPT_USE_PNG TRUE)
ELSE()
  SET(LEP_USE_PNG FALSE)
ENDIF()
MESSAGE(STATUS "leptonica HAVE_LIBPNG: ${LEPT_USE_PNG}")
if(LEPT_USE_PNG)
  add_definitions(-DHAVE_LIBPNG)
  #add_definitions(${PNG_DEFINITIONS})
  include_directories(${OPENCV_SUBFOLDER}/3rdparty/libpng)
endif()

IF(WITH_TIFF AND BUILD_TIFF)
  SET(LEPT_USE_TIFF TRUE)
ELSE()
  SET(LEPT_USE_TIFF FALSE)
ENDIF()
MESSAGE(STATUS "leptonica HAVE_LIBTIFF: ${LEPT_USE_TIFF}")
if(LEPT_USE_TIFF)
  add_definitions(-DHAVE_LIBTIFF)
  #add_definitions(${PNG_DEFINITIONS})
  include_directories(${OPENCV_SUBFOLDER}/3rdparty/libtiff)
  include_directories(${CMAKE_BINARY_DIR}/opencv/3rdparty/libtiff)
endif()


add_library(${the_target} STATIC ${lepton_srcs} ${lepton_hdrs})

IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "tesseract")
