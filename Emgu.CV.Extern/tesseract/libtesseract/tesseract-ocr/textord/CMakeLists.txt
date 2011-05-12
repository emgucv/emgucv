# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(tesseract_textord)
file(GLOB tesseract_textord_srcs "*.cpp")
#file(GLOB tesseract_ccutil_srcs_excludes "scanutils.cpp")
#LIST(REMOVE_ITEM tesseract_ccutil_srcs ${tesseract_ccutil_srcs_excludes})
file(GLOB tesseract_textord_hdrs "*.h")
source_group("Src" FILES ${tesseract_textord_srcs})
source_group("Include" FILES ${tesseract_textord_hdrs})
#add_definitions(-D__MSW32__ -DHAVE_LIBLEPT -DLEPTONLIB_IMPORTS)
add_definitions(-D__MSW32__ -DHAVE_LIBLEPT -W0)
include_directories("../vs2008/include" "../cutil" "../ccstruct" "../ccutil" "../viewer" "../image" "../dict" "../classify" "../../leptonica/leptonlib-1.68/src" "../ccmain" "../wordrec")
add_library(${PROJECT_NAME} STATIC ${tesseract_textord_srcs} ${tesseract_textord_hdrs})
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "tesseract")
ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_image tesseract_dict tesseract_classify libleptonica tesseract_wordrec)

