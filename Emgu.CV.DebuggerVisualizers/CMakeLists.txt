# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32)

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

PROJECT(Emgu.CV.DebuggerVisualizers)

BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.DebuggerVisualizers.sln" "Emgu.CV.DebuggerVisualizers" "${EXTRA_BUILD_FLAGS}" ALL)	
#BUILD_CSPROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.DebuggerVisualizers/Emgu.CV.DebuggerVisualizers.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} 
  PRE_BUILD
  COMMAND ${MSBUILD_EXECUTABLE} -t:restore Emgu.CV.DebuggerVisualizers.sln
  WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/")

ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

INSTALL(
   FILES 
   ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll 
   ${LIBRARY_OUTPUT_PATH}/System.Runtime.InteropServices.RuntimeInformation.dll
   DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
   COMPONENT emgucv_binary)

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()


ENDIF()
