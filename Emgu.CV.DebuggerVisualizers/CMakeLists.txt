# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32)

PROJECT(Emgu.CV.DebuggerVisualizers)

BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.DebuggerVisualizers.sln" "Emgu.CV.DebuggerVisualizers" "${EXTRA_BUILD_FLAGS}" ALL)	
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)

INSTALL(
   FILES 
   #${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
   ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll 
   DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
   COMPONENT emgucv_binary)
#INSTALL(
#  DIRECTORY
#  ${CMAKE_CURRENT_SOURCE_DIR}
#  DESTINATION .
#	COMPONENT emgucv_source
#  FILES_MATCHING 
#  PATTERN "*.cs"
#  PATTERN "*.csproj"
#  PATTERN "*.resx"
#  PATTERN ".svn" EXCLUDE
#  PATTERN "obj" EXCLUDE
#  PATTERN "CMakeFiles" EXCLUDE
#  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
#)


if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()


INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION .
    COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "*.csproj"
  PATTERN ".svn" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMakeFiles" EXCLUDE
  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
)
INSTALL(
    FILES 
    ${CMAKE_CURRENT_SOURCE_DIR}/../miscellaneous/vswhere.exe
    DESTINATION miscellaneous
    COMPONENT emgucv_binary)
ENDIF()
