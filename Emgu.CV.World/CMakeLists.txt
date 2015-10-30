# --------------------------------------------------------
#  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

PROJECT(Emgu.CV.World)

IF(NOT (ANDROID OR IOS OR NETFX_CORE))
BUILD_CSPROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.World.csproj")
INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll 
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_binary)
if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()
ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
ENDIF()
  
IF(WIN32 OR APPLE OR ANDROID)
IF(NETFX_CORE OR ANDROID)
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION .
    COMPONENT emgucv_source
  FILES_MATCHING 
	PATTERN "*.shproj"
	PATTERN "*.projitems"
	PATTERN "*.cs"
    PATTERN "*.resx"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )
ELSE()
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION .
    COMPONENT emgucv_source
  FILES_MATCHING 
    PATTERN "${PROJECT_NAME}.csproj"
    PATTERN "AssemblyInfo.cs"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
)
ENDIF()

ENDIF()  

ENDIF()
