# --------------------------------------------------------
#  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

PROJECT(libleptonica)

#IF(MSVC)
#enable_language(CXX)
#ENDIF()

SET(LEPTON_DIR ${LEPT_ROOT_DIR})
 
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

add_library(${the_target} STATIC ${lepton_srcs} ${lepton_hdrs})

IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "tesseract")
