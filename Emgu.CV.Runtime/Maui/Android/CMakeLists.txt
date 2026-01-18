# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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
	PATTERN "README.md"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
	
  PROJECT(Emgu.CV.runtime.maui.android)

  SET(EMGUCV_MAUI_ANDROID_NUGET_TITLE "Emgu CV for MAUI Android Native Runtime")
  
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
  IF(EMGU_SIGN_FOUND AND WIN32)
    CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignBinary.Targets)
  ENDIF()
  IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
    CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignPkg.Targets)
  ENDIF()
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)

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