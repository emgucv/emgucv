# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)
  IF (DOTNET_FOUND)
    #IF(WIN32 AND (MSVC_VERSION GREATER 1900)) #need MSVC to be at least 2017
    PROJECT(Emgu.CV.Models.IOS)
    SET(EXTRA_MSBUILD_FLAGS "")
    IF(WIN32)
      SET(EXTRA_MSBUILD_FLAGS "/p:Platform=\"Any CPU\"")
    ENDIF()
    BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/CrossPlatform/Emgu.CV.sln" "Emgu.CV.Models.IOS" "${EXTRA_MSBUILD_FLAGS}" ALL)

    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../libs/iOS/Emgu.CV.Models.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../libs/iOS/Emgu.CV.Models.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)	  

    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard.IOS)  
    
    INSTALL(
      DIRECTORY
      ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION .
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
ENDIF()
