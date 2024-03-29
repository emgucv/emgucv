# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (HAVE_MACOS)

  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Platform/Xamarin
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "*.cs"
    PATTERN "*.csproj"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "bin" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )
	
  IF(APPLE AND (NOT ("${CMAKE_HOST_SYSTEM_PROCESSOR}" STREQUAL "arm64")))
  
    PROJECT(Emgu.CV.Platform.Xamarin.Mac)
    
    #SET(EXTRA_BUILD_FLAGS "/p:Platform=\"Any CPU\"")
    SET(EXTRA_BUILD_FLAGS "")
    MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Solution/Mac/Emgu.CV.Mac.Xamarin.sln" "Emgu.CV.Platform.Xamarin.Mac" "${EXTRA_BUILD_FLAGS}" ALL)	
        
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()
    
    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard)

  ENDIF()

ENDIF()

IF (HAVE_XAMARIN_MAC)
    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/Emgu.CV.Platform.Xamarin.Mac.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/Emgu.CV.Platform.Xamarin.Mac.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)
ENDIF()
