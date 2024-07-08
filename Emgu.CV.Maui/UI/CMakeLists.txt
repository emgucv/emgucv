# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_ANDROID AND HAVE_MAUI_IOS AND HAVE_MAUI_MAC)
  PROJECT(Emgu.CV.Maui.UI)
  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Maui
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "*.cs"
    PATTERN "${PROJECT_NAME}.csproj"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
  
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/${PROJECT_NAME}.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
  
  if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif() 
  
  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV)
  
  IF (TARGET Emgu.CV.runtime.maui.ios)
    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.runtime.maui.ios)
  ENDIF()
  
  IF (TARGET Emgu.CV.runtime.maui.macos)
    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.runtime.maui.macos)
  ENDIF()
  
  IF (TARGET Emgu.CV.runtime.maui.android)
    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.runtime.maui.android)
  ENDIF()
  
  INSTALL(
    DIRECTORY 
    ${LIBRARY_OUTPUT_PATH}/Build/Emgu.CV.runtime.maui.android
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
    COMPONENT emgucv_binary)	  
ENDIF()