# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_ANDROID AND HAVE_MAUI_IOS AND HAVE_MAUI_MAC)
  PROJECT(Emgu.CV.Platform.Maui.UI)
  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Platform/Maui
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "*.cs"
    PATTERN "${PROJECT_NAME}.csproj"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\" /p:PublishReadyToRun=false")
  
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/${PROJECT_NAME}.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
  
  if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif() 
  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard)
  FOREACH (PLATFORMFORM_FOLDER IN ITEMS net7.0-android net7.0-ios net7.0-maccatalyst net7.0-windows10.0.22621.0)
    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/${PLATFORMFORM_FOLDER}/${PROJECT_NAME}.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/${PLATFORMFORM_FOLDER}/${PROJECT_NAME}.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/${PLATFORMFORM_FOLDER}
      COMPONENT emgucv_binary)
  ENDFOREACH ()

ENDIF()