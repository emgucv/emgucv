# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)
  IF (DOTNET_FOUND)
    #IF(WIN32 AND (MSVC_VERSION GREATER 1900)) #need MSVC to be at least 2017
    PROJECT(Emgu.CV)
	
	#generate nuget config file for Emgu.CV 
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
    IF(EMGU_SIGN_FOUND AND WIN32)
      CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignBinary.Targets)
    ENDIF()
    IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
      CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignPkg.Targets)
    ENDIF()
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)
    
    SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
    
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.csproj" "${EXTRA_BUILD_FLAGS}" ALL)

    INSTALL(
      DIRECTORY 
      ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary)	  

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)  
    
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
	endif() 
  ENDIF()
ENDIF()
