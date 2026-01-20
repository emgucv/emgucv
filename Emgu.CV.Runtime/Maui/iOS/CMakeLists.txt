# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

SET(EMGUCV_MAUI_IOS_NUGET_TITLE "Emgu CV for MAUI iOS Native Runtime")	
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)  


IF(HAVE_MAUI)
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
	  
  IF (APPLE AND HAVE_MAUI_IOS)
    PROJECT(Emgu.CV.runtime.maui.ios)
		
    SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.ios.csproj" "${EXTRA_BUILD_FLAGS}" ALL)

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV)  
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif() 
  ENDIF()
  
  IF (HAVE_MAUI_IOS)
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

ENDIF()
