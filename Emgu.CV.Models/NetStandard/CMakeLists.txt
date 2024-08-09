# --------------------------------------------------------
#  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)
  #IF (DOTNET_FOUND AND NOT (HAVE_IOS_ONLY))
  #IF(WIN32 AND (MSVC_VERSION GREATER 1900)) #need MSVC to be at least 2017
  
  PROJECT(Emgu.CV.Models)
  
  #SET(EXTRA_MSBUILD_FLAGS "")
  #IF(WIN32)
  #  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"Any CPU\"")
  #ENDIF()
  
  IF(DOTNET_FOUND)
    #IF (APPLE AND NOT ${CMAKE_APPLE_SILICON_PROCESSOR})
    # Need to use dotnet to build the project.
    # dotnet msbuild seems to have a bug on Mac OS, it do not respect the assembly name.
    # It output Emgu.CV.Model.NetStandard.dll instead of Emgu.CV.Model.dll specified in
    # the csproj file. dotnet seems to work just fine.
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Models.csproj" "" ALL)
    #ELSEIF (DOTNET_FOUND)
    # BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/CrossPlatform/Emgu.CV.sln" "Emgu.CV.Models.NetStandard" "${EXTRA_BUILD_FLAGS}" ALL)
  ELSE()
    MESSAGE(FATAL_ERROR "dotnet command not found, unable to build Emgu.CV.Models project")
  ENDIF()
  
  INSTALL(
    DIRECTORY 
    ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.Models
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
    COMPONENT emgucv_binary)
    
  IF (TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)  
  ENDIF()
  
  #INSTALL(
  #  DIRECTORY
  #  ${CMAKE_CURRENT_SOURCE_DIR}
  #  DESTINATION Emgu.CV.Models
  #  COMPONENT emgucv_source
  #  FILES_MATCHING 
  #  PATTERN "Emgu.CV.Models.csproj"
  #  PATTERN "*.cs"
  #  PATTERN ".svn" EXCLUDE
  #  PATTERN "obj" EXCLUDE
  #  PATTERN "CMakeFiles" EXCLUDE
  #  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
  #)
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif() 
  #ENDIF()
ENDIF()
