# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)
  IF (DOTNET_FOUND)
    #IF(WIN32 AND (MSVC_VERSION GREATER 1900)) #need MSVC to be at least 2017
    PROJECT(Emgu.CV)
    #SET(EXTRA_MSBUILD_FLAGS "")
    
    SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
    
    #IF (NOT (WIN32 OR APPLE))
    #  #LINUX
    #  SET(EXTRA_BUILD_FLAGS ${EXTRA_BUILD_FLAGS} "/p:TargetFrameworks=\"netstandard2.0\"")
    #ENDIF()
    
    #IF (APPLE AND NOT ${CMAKE_APPLE_SILICON_PROCESSOR})
      BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
    #ELSE()
    #  BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/CrossPlatform/Emgu.CV.sln" "Emgu.CV.Platform.NetStandard" "${EXTRA_MSBUILD_FLAGS}" ALL)
    #ENDIF()

    INSTALL(
      DIRECTORY 
      ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary)	  

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)  
    
    #INSTALL(
    #  DIRECTORY
    #  ${CMAKE_CURRENT_SOURCE_DIR}
    #  DESTINATION Emgu.CV
    #  COMPONENT emgucv_source
    #  FILES_MATCHING 
    #  PATTERN "Emgu.CV.csproj"
    #  PATTERN ".svn" EXCLUDE
    #  PATTERN "obj" EXCLUDE
    #  PATTERN "CMakeFiles" EXCLUDE
    #  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    #  )
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
	endif() 
  ENDIF()
ENDIF()
