# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_ANDROID AND HAVE_MAUI_IOS AND HAVE_MAUI_MAC)

  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Runtime/Maui
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "*.cs"
    PATTERN "*.csproj"
	PATTERN "Directory.Build.props"
	PATTERN "README.md"
	#PATTERN "*.png"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
	PATTERN "bin" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
	
  PROJECT(Emgu.CV.runtime.maui)
  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
  
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
  
  if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif() 
  
  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV)
  
  IF (TARGET Emgu.CV.runtime.maui.ios)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.ios)
  ENDIF()
  
  IF (TARGET Emgu.CV.runtime.maui.macos)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.macos)
  ENDIF()
  
  IF (TARGET Emgu.CV.runtime.maui.android)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.android)
  ENDIF()
  
  IF (TARGET Emgu.CV.runtime.windows.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.nuget)
  ENDIF()
  
  INSTALL(
    DIRECTORY 
    ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.runtime.maui
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
    COMPONENT emgucv_binary
	FILES_MATCHING 
      PATTERN "*.dll"
      PATTERN "*.pdb"
      PATTERN "*.xml")
ENDIF()