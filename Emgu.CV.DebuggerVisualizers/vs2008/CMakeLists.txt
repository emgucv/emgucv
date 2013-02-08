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

IF(DEBUGGER_VISUALIZER_LIB_VS2008)

#build debugger visualizer for visual studio 2008
PROJECT(Emgu.CV.DebuggerVisualizers.VS2008)
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../../Emgu.CV.snk)
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
ENDIF()
