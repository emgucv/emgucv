# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(tesseract_ccmain)

file(GLOB tesseract_ccmain_srcs "*.cpp")
file(GLOB tesseract_ccmain_srcs_excludes "tessembedded.cpp")
LIST(REMOVE_ITEM tesseract_ccmain_srcs ${tesseract_ccmain_srcs_excludes})
LIST(APPEND tesseract_ccmain_srcs "${PROJECT_SOURCE_DIR}/../vs2008/port/gettimeofday.cpp")
file(GLOB tesseract_ccmain_hdrs "*.h")
source_group("Src" FILES ${tesseract_ccmain_srcs})
source_group("Include" FILES ${tesseract_ccmain_hdrs})
#add_definitions(-D__MSW32__ -DUSE_STD_NAMESPACE -DHAVE_LIBLEPT -DLEPTONLIB_IMPORTS)
add_definitions(-D__MSW32__ -DUSE_STD_NAMESPACE -DHAVE_LIBLEPT  -W0)
include_directories("../vs2008/include" "../vs2008/port" "../cutil" "../ccstruct" "../ccutil" "../viewer" "../image" "../dict" "../classify" "../../leptonica/leptonlib-1.68/src" "../neural_networks/runtime" "../wordrec" "../textord" "../cube")
add_library(${PROJECT_NAME} STATIC ${tesseract_ccmain_srcs} ${tesseract_ccmain_hdrs})
ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_image tesseract_dict tesseract_classify libleptonica tesseract_neural_networks tesseract_wordrec tesseract_textord tesseract_cube)

