# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

   IF(HAVE_IOS)
      #APPLE
      INSTALL(
	DIRECTORY
	${CMAKE_CURRENT_SOURCE_DIR}
	DESTINATION .
	COMPONENT emgucv_source
	FILES_MATCHING 
	PATTERN "${PROJECT_NAME}.IOS.csproj"
	PATTERN ".svn" EXCLUDE
	PATTERN "obj" EXCLUDE
	PATTERN "CMakeFiles" EXCLUDE
	PATTERN "${PROJECT_NAME}.dir" EXCLUDE
	)     
   ENDIF()

ADD_SUBDIRECTORY(Netstandard)
ADD_SUBDIRECTORY(iOS)
ENDIF()
