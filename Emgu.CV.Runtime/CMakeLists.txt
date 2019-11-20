# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
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

IF(HAVE_ANDROID)
  PROJECT(Emgu.CV.Runtime.Android)
  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/Android
    DESTINATION Emgu.CV.Runtime
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "${PROJECT_NAME}.csproj"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
  BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Android/Emgu.CV.Android.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)
  ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} 
	PRE_BUILD
	COMMAND ${DOTNET_EXECUTABLE} restore ${PROJECT_NAME}.csproj
	WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/Android")
  if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif() 
  INSTALL(
    FILES 
    ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
    ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll 
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
    COMPONENT emgutf_binary)
ENDIF()
