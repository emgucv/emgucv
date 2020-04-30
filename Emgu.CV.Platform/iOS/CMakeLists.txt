# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_IOS)
#  IF (DOTNET_FOUND)
  IF (APPLE)
      PROJECT(Emgu.CV.Platform.IOS)
      
#      ADD_CUSTOM_TARGET(${PROJECT_NAME} ALL)
#      ADD_CUSTOM_COMMAND(
#        TARGET ${PROJECT_NAME}
#        #COMMAND "${MSBUILD_EXECUTABLE}" Emgu.CV.iOS.sln -p:TargetFrameworkRootPath="/Library/Frameworks/Xamarin.iOS.framework/Versions/Current" -p:MSBuildExtensionsPath="/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/lib/msbuild/iOS" -p:Configuration=Release -p:OutputPath="../libs/ios_build" -p:GeneratedSourcesDir="../libs/ios_build"
#	COMMAND env -i zsh "${MSBUILD_EXECUTABLE}" Emgu.CV.World.IOS.csproj -p:Configuration=Release -p:Platform=AnyCPU
#	#COMMAND "${VSTOOL_EXECUTABLE}" build -t:Build -c:"Release|iPhone" Emgu.CV.World.iOS.csproj
#        WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
#	#WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/iOS"
#        COMMENT "BUILDING ${PROJECT_NAME}")
      
    BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/iOS/Emgu.CV.iOS.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)
#    BUILD_CSPROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Platform.IOS.csproj" "" ALL)

    INSTALL(
      FILES 
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.xml
      ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)	  

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard.IOS)  
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif() 

  ENDIF()
  
  INSTALL(
      DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION Emgu.CV.Platform
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Platform.IOS.csproj"
	  PATTERN "*.cs"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
#  ENDIF()
ENDIF()
