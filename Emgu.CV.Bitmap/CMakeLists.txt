# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (TARGET Emgu.CV)
  #MESSAGE(STATUS "......................... ANDROID: ${ANDROID};APPLE: ${APPLE}")
  IF (DOTNET_FOUND AND (NOT ANDROID) AND (NOT APPLE))
    #DO NOT build Emgu.CV.Bitmap for Android or APPLE
    PROJECT(Emgu.CV.Bitmap)

      #SET(EMGUCV_BITMAP_NUGET_TITLE "Bitmap extension for Emgu CV")
      #SET(EMGUCV_BITMAP_NUGET_SUMMARY "This package add extension method to convert Bitmap to and from Emgu objects (e.g. Mat, UMat, GpuMat)")

	  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
	  IF(EMGU_SIGN_FOUND AND WIN32)
		CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignBinary.Targets)
	  ENDIF()
	  IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
        CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignPkg.Targets)
	  ENDIF()
	  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)
	  
      SET(EXTRA_BUILD_FLAGS "")
      IF(WIN32)
        SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
      ENDIF()

    #BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/CrossPlatform/Emgu.CV.sln" "Emgu.CV.Bitmap" "${EXTRA_MSBUILD_FLAGS}" ALL)
    #IF (EMGU_SIGN_FOUND)
    #  EMGU_SIGN_BINARY(${PROJECT_NAME} ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.dll)
    #ENDIF()
	BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Bitmap.csproj" "${EXTRA_BUILD_FLAGS}" ALL)

    INSTALL(
      DIRECTORY 
      ${LIBRARY_OUTPUT_PATH}/Build/Emgu.CV.Bitmap
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary)	
    #INSTALL(
    #  FILES 
    #  ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.xml
    #  ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.dll 
    #  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
    #  COMPONENT emgucv_binary)
  
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()
  
  INSTALL(
      DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION .
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Bitmap.csproj"
	  PATTERN "*.cs"
	  PATTERN "Directory.Build.props"
	  PATTERN "README.md"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
	  
ENDIF()
