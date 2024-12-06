# --------------------------------------------------------
#  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_IOS)

  INSTALL(
      DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION Emgu.CV.Runtime/Maui
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "*.csproj"
	  PATTERN "*.cs"
	  PATTERN "Directory.Build.props"
	  PATTERN "README.md"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
	  
  IF (APPLE)
    PROJECT(Emgu.CV.runtime.maui.ios)
	SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.ios.csproj" "${EXTRA_BUILD_FLAGS}" ALL)

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV)  
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif() 
  ENDIF()
  
  INSTALL(
      DIRECTORY 
      ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.runtime.maui.ios
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary
	  FILES_MATCHING 
        PATTERN "*.dll"
        PATTERN "*.pdb"
        PATTERN "*.xml")
ENDIF()
