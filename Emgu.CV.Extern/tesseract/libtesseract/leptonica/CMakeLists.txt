# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(libleptonica)

SET(LEPTON_DIR "leptonlib-1.68")

file(GLOB lepton_srcs "${LEPTON_DIR}/src/*.c")
file(GLOB lepton_srcs_excludes "${LEPTON_DIR}/src/freetype.c" "${LEPTON_DIR}/src/endiantest.c")
LIST(REMOVE_ITEM lepton_srcs ${lepton_srcs_excludes})
file(GLOB lepton_hdrs "${LEPTON_DIR}/src/*.h")

source_group("Src" FILES ${lepton_srcs})
source_group("Include" FILES ${lepton_hdrs})
set(the_target ${PROJECT_NAME})
#add_definitions(-DLEPTONLIB_EXPORTS -DL_LITTLE_ENDIAN -DNO_CONSOLE_IO -D_CRT_SECURE_NO_WARNINGS)
add_definitions(-DL_LITTLE_ENDIAN -DXMD_H -DNO_CONSOLE_IO -W0)
add_library(${the_target} STATIC ${lepton_srcs} ${lepton_hdrs})

set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "tesseract")