# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(tesseract_ccstruct)
file(GLOB tesseract_ccstruct_srcs "*.cpp")
file(GLOB tesseract_ccstruct_hdrs "*.h")
source_group("Src" FILES ${tesseract_ccstruct_srcs})
source_group("Include" FILES ${tesseract_ccstruct_hdrs})
add_definitions(-D__MSW32__ -W0)
include_directories("../vs2008/include" "../vs2008/port" "../ccutil" "../viewer" "../cutil" "../image" "../../leptonica/leptonlib-1.67/src")

add_library(${PROJECT_NAME} STATIC ${tesseract_ccstruct_srcs} ${tesseract_ccstruct_hdrs})
ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_ccutil tesseract_cutil tesseract_viewer tesseract_image libleptonica)