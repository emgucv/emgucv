# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_ANDROID)

  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Runtime/Maui
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "*.cs"
    PATTERN "*.csproj"
	PATTERN "Directory.Build.props"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
	
  PROJECT(Emgu.CV.runtime.maui.android)

  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
  
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.android.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
  
  if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif() 
  
  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV)
  
  INSTALL(
      DIRECTORY 
      ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.runtime.maui.android
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary
	  FILES_MATCHING 
        PATTERN "*.dll"
        PATTERN "*.pdb"
        PATTERN "*.xml")  
ENDIF()