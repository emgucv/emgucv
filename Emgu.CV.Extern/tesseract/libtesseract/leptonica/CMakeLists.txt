# --------------------------------------------------------
#  Copyright (C) 2004-2013 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(libleptonica)

SET(LEPTON_DIR "leptonica-1.69")

file(GLOB lepton_srcs "${LEPTON_DIR}/src/*.c")
file(GLOB lepton_srcs_excludes "${LEPTON_DIR}/src/freetype.c" "${LEPTON_DIR}/src/endiantest.c" "${LEPTON_DIR}/src/xtractprotos.c")
LIST(REMOVE_ITEM lepton_srcs ${lepton_srcs_excludes})
file(GLOB lepton_hdrs "${LEPTON_DIR}/src/*.h")

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
