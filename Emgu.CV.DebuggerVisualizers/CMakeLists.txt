# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32 AND (NOT IS_WINDESKTOP_ARM64_BUILD))

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
#SET(EXTRA_BUILD_FLAGS "/p:Platform=\"x86\"")
SET(EXTRA_BUILD_FLAGS "/p:Platform=\"Any CPU\"")

MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.DebuggerVisualizers.sln" "Emgu.CV.DebuggerVisualizers" "${EXTRA_BUILD_FLAGS}" ALL)	
IF (EMGU_SIGN_FOUND)
  EMGU_SIGN_BINARY(${PROJECT_NAME} ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll)
ENDIF()

IF (TARGET Emgu.CV.Platform.NetStandard)
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard)
ENDIF()
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
