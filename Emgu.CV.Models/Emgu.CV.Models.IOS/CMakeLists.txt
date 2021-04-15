# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF( (EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND HAVE_IOS )
  
    #IF(WIN32 AND (MSVC_VERSION GREATER 1900)) #need MSVC to be at least 2017
    PROJECT(Emgu.CV.Models.IOS)
    SET(EXTRA_MSBUILD_FLAGS "")
    
    IF(WIN32)
      SET(EXTRA_MSBUILD_FLAGS "/p:Platform=\"Any CPU\"")
    ENDIF()
    
    IF (APPLE)
      # Need to use MSBUILD instead of dotnet to build the project.
      # dotnet seems to have a bug on Mac OS, it do not respect the assembly name.
      # It output Emgu.CV.Model.IOS.dll instead of Emgu.CV.Model.dll specified in
      # the csproj file. MSBUILD seems to work just fine.
      MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/CrossPlatform/Emgu.CV.sln" "Emgu.CV.Models.IOS" "${EXTRA_MSBUILD_FLAGS}" ALL)
    ELSEIF (DOTNET_FOUND)
      BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/CrossPlatform/Emgu.CV.sln" "Emgu.CV.Models.IOS" "${EXTRA_MSBUILD_FLAGS}" ALL)
    ENDIF()
    
    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/iOS/Emgu.CV.Models.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/iOS/Emgu.CV.Models.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/iOS
      COMPONENT emgucv_binary)	  

    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard.IOS)  
    
    INSTALL(
      DIRECTORY
      ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION Emgu.CV.Models
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Models.IOS.csproj"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
	endif() 
  
ENDIF()
