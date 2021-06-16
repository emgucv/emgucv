# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_ANDROID)
  PROJECT(Emgu.CV.Platform.Android)
  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Platform
    COMPONENT emgucv_source
    FILES_MATCHING 
	PATTERN "*.cs"
    PATTERN "${PROJECT_NAME}.csproj"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"Any CPU\"")
  MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/Android/Emgu.CV.Android.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)
  IF (EMGU_SIGN_FOUND)
    EMGU_SIGN_BINARY(${PROJECT_NAME} ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.dll)
  ENDIF()
  #ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} 
#	PRE_BUILD
#	COMMAND ${DOTNET_EXECUTABLE} restore ${PROJECT_NAME}.csproj
#	WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}")
  if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif() 
  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard)
  INSTALL(
    FILES 
    ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.xml
    ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.dll 
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
    COMPONENT emgucv_binary)
ENDIF()

