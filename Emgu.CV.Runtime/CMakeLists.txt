# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Mac
    DESTINATION Emgu.CV.Runtime
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "Emgu.CV.Runtime.Mac.shproj"
    PATTERN "Emgu.CV.Runtime.Mac.projitems"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )

  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Windows
    DESTINATION Emgu.CV.Runtime
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "Emgu.CV.Runtime.Windows.shproj"
	PATTERN "Emgu.CV.Runtime.Windows.projitems"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )
	
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/UWP
    DESTINATION Emgu.CV.Runtime
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "Emgu.CV.Runtime.UWP.shproj"
	PATTERN "Emgu.CV.Runtime.UWP.projitems"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )
	
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Ubuntu
    DESTINATION Emgu.CV.Runtime
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "Emgu.CV.Runtime.Ubuntu.shproj"
	PATTERN "Emgu.CV.Runtime.Ubuntu.projitems"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )
	
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Raspbian
    DESTINATION Emgu.CV.Runtime
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "Emgu.CV.Runtime.Raspbian.shproj"
	PATTERN "Emgu.CV.Runtime.Raspbian.projitems"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )


