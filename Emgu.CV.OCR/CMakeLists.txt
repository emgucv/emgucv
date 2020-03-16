# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND EMGU_CV_WITH_TESSERACT)

IF(WIN32 OR APPLE OR ANDROID)
PROJECT(Emgu.CV.OCR)
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION .
    COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "*.shproj"
  PATTERN "*.projitems"
  PATTERN "*.resx"
  #PATTERN "tessdata/eng.*"
  PATTERN ".svn" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMakeFiles" EXCLUDE
  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
)

ENDIF()
ENDIF()
