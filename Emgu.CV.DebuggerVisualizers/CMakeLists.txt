IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32)

PROJECT(Emgu.CV.DebuggerVisualizers)

FIND_FILE(DEBUGGER_VISUALIZER_LIB 
NAME Microsoft.VisualStudio.DebuggerVisualizers.dll
HINTS
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
"$ENV{ProgramFiles}/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
PATHS 
"$ENV{ProgramFiles} (x86)/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
"$ENV{ProgramFiles}/Microsoft Visual Studio 9.0/Common7/IDE/PublicAssemblies"
)

FILE(GLOB_RECURSE SRC_CV_DEBUGGER_VISUALIZER RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_CV_DEBUGGER_VISUALIZER ${SRC_CV_DEBUGGER_VISUALIZER} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
ADD_CS_REFERENCES("System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;${DEBUGGER_VISUALIZER_LIB}")
COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV_DEBUGGER_VISUALIZER}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_binary)

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION ./
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
