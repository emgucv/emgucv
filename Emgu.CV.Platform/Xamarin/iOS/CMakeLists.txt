# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_IOS)

  INSTALL(
      DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION Emgu.CV.Platform/Xamarin
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Platform.Xamarin.IOS.csproj"
	  PATTERN "*.cs"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
	  
  IF (APPLE)
    PROJECT(Emgu.CV.Platform.Xamarin.IOS)
       
    MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Solution/iOS/Emgu.CV.iOS.Xamarin.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard.IOS)  
	
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif() 
  ENDIF()

ENDIF()

IF(HAVE_XAMARIN_IOS)
    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/Emgu.CV.Platform.Xamarin.IOS.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../../libs/Emgu.CV.Platform.Xamarin.IOS.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)	  
ENDIF()