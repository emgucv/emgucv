# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(tesseract_dict)
file(GLOB tesseract_dict_srcs "*.cpp")
#file(GLOB tesseract_ccutil_srcs_excludes "scanutils.cpp")
#LIST(REMOVE_ITEM tesseract_ccutil_srcs ${tesseract_ccutil_srcs_excludes})
file(GLOB tesseract_dict_hdrs "*.h")
source_group("Src" FILES ${tesseract_dict_srcs})
source_group("Include" FILES ${tesseract_dict_hdrs})
add_definitions(-D__MSW32__ -W0)
include_directories("../vs2008/include" "../cutil" "../ccstruct" "../ccutil" "../viewer" "../image")
add_library(${PROJECT_NAME} STATIC ${tesseract_dict_srcs} ${tesseract_dict_hdrs})
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "tesseract")
ADD_DEPENDENCIES(${PROJECT_NAME} tesseract_cutil tesseract_ccstruct tesseract_ccutil tesseract_viewer tesseract_image)

