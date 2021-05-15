# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)
  IF (DOTNET_FOUND)
    #IF(WIN32 AND (MSVC_VERSION GREATER 1900)) #need MSVC to be at least 2017
    PROJECT(Emgu.CV.Platform.NetStandard.IOS)
    SET(EXTRA_MSBUILD_FLAGS "")
    IF(WIN32)
      SET(EXTRA_MSBUILD_FLAGS "/p:Platform=\"Any CPU\"")
    ENDIF()
    BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/CrossPlatform/Emgu.CV.sln" "Emgu.CV.Platform.NetStandard.IOS" "${EXTRA_MSBUILD_FLAGS}" ALL)
    
    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/iOS/Emgu.CV.Platform.NetStandard.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/iOS/Emgu.CV.Platform.NetStandard.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/iOS
      COMPONENT emgucv_binary)	  

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard)  
    
    INSTALL(
      DIRECTORY
      ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION Emgu.CV.Platform
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Platform.NetStandard.IOS.csproj"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
	endif() 
  ENDIF()
ENDIF()
