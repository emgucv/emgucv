# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_IOS)

  INSTALL(
      DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION Emgu.CV.Platform/Maui
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Platform.Maui.IOS.csproj"
	  PATTERN "*.cs"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
	  
#  IF (DOTNET_FOUND)
  IF (APPLE)
    PROJECT(Emgu.CV.Platform.Maui.IOS)
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/${PROJECT_NAME}.csproj" "" ALL)
    #DOTNET_MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Solution/iOS/Emgu.CV.iOS.Maui.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)

    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/${PROJECT_NAME}.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/${PROJECT_NAME}.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)	  

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard.IOS)  
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif() 

  ENDIF()
#  ENDIF()
ENDIF()
