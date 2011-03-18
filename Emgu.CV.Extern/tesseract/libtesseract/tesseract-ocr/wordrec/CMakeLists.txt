# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(tesseract_wordrec)
file(GLOB tesseract_wordrec_srcs "*.cpp")
#file(GLOB tesseract_ccutil_srcs_excludes "scanutils.cpp")
#LIST(REMOVE_ITEM tesseract_ccutil_srcs ${tesseract_ccutil_srcs_excludes})
file(GLOB tesseract_wordrec_hdrs "*.h")
source_group("Src" FILES ${tesseract_wordrec_srcs})
source_group("Include" FILES ${tesseract_wordrec_hdrs})
add_definitions(-D__MSW32__ -W0)
include_directories("../vs2008/include" "../cutil" "../ccstruct" "../ccutil" "../viewer" "../image" "../dict" "../classify")
add_library(${PROJECT_NAME} STATIC ${tesseract_wordrec_srcs} ${tesseract_wordrec_hdrs})
ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_image tesseract_dict tesseract_classify)

