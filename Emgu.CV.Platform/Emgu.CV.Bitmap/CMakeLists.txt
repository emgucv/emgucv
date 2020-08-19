# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (TARGET Emgu.CV.Platform.NetStandard)

  IF (DOTNET_FOUND)
    PROJECT(Emgu.CV.Bitmap)
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/${PROJECT_NAME}.csproj" "" ALL)  
  
    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)
  
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard)
  ENDIF()
  
  INSTALL(
      DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION Emgu.CV.Platform
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Bitmap.csproj"
	  PATTERN "*.cs"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
	  
ENDIF()
