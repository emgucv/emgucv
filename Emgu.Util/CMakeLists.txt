# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

PROJECT(Emgu.Util)
 
IF(WIN32 OR APPLE OR ANDROID)
#IF(NETFX_CORE OR ANDROID)
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION .
    COMPONENT emgucv_source
  FILES_MATCHING 
	PATTERN "*.shproj"
	PATTERN "*.projitems"
	PATTERN "*.cs"
    PATTERN "*.resx"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )

ENDIF()  

ENDIF()
