# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_MAUI_MAC)

  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Runtime/Maui
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "*.cs"
    PATTERN "*.csproj"
	PATTERN "Directory.Build.props"
	PATTERN "README.md"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "bin" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    )
	
  INSTALL(
      DIRECTORY 
      ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.runtime.maui.macos
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary	
	  FILES_MATCHING 
        PATTERN "*.dll"
        PATTERN "*.pdb"
        PATTERN "*.xml")	
		
  IF(APPLE)	
    PROJECT(Emgu.CV.runtime.maui.macos)
	SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.macos.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
        
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV)
    ENDIF()
ENDIF()


