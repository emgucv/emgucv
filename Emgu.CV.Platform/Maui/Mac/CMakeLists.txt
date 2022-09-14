# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND HAVE_MAC_CATALYST )
    PROJECT(Emgu.CV.Platform.Maui.Mac)
    
	BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/${PROJECT_NAME}.csproj" "${EXTRA_MSBUILD_FLAGS}" ALL)
    #DOTNET_MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Solution/Mac/Emgu.CV.Mac.Example.sln" "Emgu.CV.Platform.Maui.Mac" "${EXTRA_BUILD_FLAGS}" ALL)	
    
    INSTALL(
      FILES 
      ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
      ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)
    
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()
    
    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV.Platform.NetStandard.IOS)

ENDIF()

IF (HAVE_MAC_CATALYST)
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Platform/Maui/Mac
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "*.cs"
    PATTERN "*.csproj"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "bin" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )
ENDIF()
