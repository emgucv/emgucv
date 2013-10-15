# --------------------------------------------------------
#  Copyright (C) 2004-2013 by EMGU. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32)

FIND_FILE(DEBUGGER_VISUALIZER_LIB_VS2008
NAME Microsoft.VisualStudio.DebuggerVisualizers.dll
HINTS
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
"$ENV{ProgramFiles}/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
PATHS 
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
"$ENV{ProgramFiles}/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
)

#MESSAGE("DEBUGGER_VISUALIZER_LIB_VS2008: ${DEBUGGER_VISUALIZER_LIB_VS2008}")
IF(DEBUGGER_VISUALIZER_LIB_VS2008)
#MESSAGE("BUILDING DEBUGGER VISUALIZER 2008 with ${SRC_CV_DEBUGGER_VISUALIZER}")

#build debugger visualizer for visual studio 2008
PROJECT(Emgu.CV.DebuggerVisualizers.VS2008)
FILE(GLOB_RECURSE SRC_CV_DEBUGGER_VISUALIZER ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_CV_DEBUGGER_VISUALIZER ${SRC_CV_DEBUGGER_VISUALIZER} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
ADD_CS_REFERENCES("System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;${DEBUGGER_VISUALIZER_LIB_VS2008}")
COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV_DEBUGGER_VISUALIZER}" ALL)

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_binary)

ENDIF()

#ADD_SUBDIRECTORY(vs2010)
FIND_FILE(DEBUGGER_VISUALIZER_LIB_VS2010
NAME Microsoft.VisualStudio.DebuggerVisualizers.dll
HINTS
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 10.0/Common7/IDE/ReferenceAssemblies/v2.0"
"$ENV{ProgramFiles}/Microsoft Visual Studio 10.0/Common7/IDE/ReferenceAssemblies/v2.0"
PATHS 
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 10.0/Common7/IDE/ReferenceAssemblies/v2.0"
"$ENV{ProgramFiles}/Microsoft Visual Studio 10.0/Common7/IDE/ReferenceAssemblies/v2.0"
)

IF(DEBUGGER_VISUALIZER_LIB_VS2010)
PROJECT(Emgu.CV.DebuggerVisualizers.VS2010)
FILE(GLOB_RECURSE SRC_CV_DEBUGGER_VISUALIZER ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_CV_DEBUGGER_VISUALIZER ${SRC_CV_DEBUGGER_VISUALIZER} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)
#build debugger visualizer for visual studio 2010
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
ADD_CS_REFERENCES("System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;${DEBUGGER_VISUALIZER_LIB_VS2010}")
COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV_DEBUGGER_VISUALIZER}" ALL)

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_binary)

ENDIF()
#ADD_SUBDIRECTORY(vs2012)
FIND_FILE(DEBUGGER_VISUALIZER_LIB_VS2012
NAME Microsoft.VisualStudio.DebuggerVisualizers.dll
HINTS
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 11.0/Common7/IDE/ReferenceAssemblies/v2.0"
"$ENV{ProgramFiles}/Microsoft Visual Studio 11.0/Common7/IDE/ReferenceAssemblies/v2.0"
PATHS 
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 11.0/Common7/IDE/ReferenceAssemblies/v2.0"
"$ENV{ProgramFiles}/Microsoft Visual Studio 11.0/Common7/IDE/ReferenceAssemblies/v2.0"
)

IF(DEBUGGER_VISUALIZER_LIB_VS2012)
PROJECT(Emgu.CV.DebuggerVisualizers.VS2012)
FILE(GLOB_RECURSE SRC_CV_DEBUGGER_VISUALIZER ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_CV_DEBUGGER_VISUALIZER ${SRC_CV_DEBUGGER_VISUALIZER} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)
#build debugger visualizer for visual studio 2012
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
ADD_CS_REFERENCES("System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;${DEBUGGER_VISUALIZER_LIB_VS2012}")
COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV_DEBUGGER_VISUALIZER}" ALL)

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_binary)

ENDIF()

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
ENDIF()
