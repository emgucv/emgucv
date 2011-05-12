# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(tesseract_ccutil)
file(GLOB tesseract_ccutil_srcs "*.cpp")
file(GLOB tesseract_ccutil_srcs_excludes "scanutils.cpp")
LIST(REMOVE_ITEM tesseract_ccutil_srcs ${tesseract_ccutil_srcs_excludes})
file(GLOB tesseract_ccutil_hdrs "*.h")
source_group("Src" FILES ${tesseract_ccutil_srcs})
source_group("Include" FILES ${tesseract_ccutil_hdrs})
add_definitions(-D__MSW32__ -W0)
include_directories("../vs2008/include")
add_library(${PROJECT_NAME} STATIC ${tesseract_ccutil_srcs} ${tesseract_ccutil_hdrs})
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "tesseract")

