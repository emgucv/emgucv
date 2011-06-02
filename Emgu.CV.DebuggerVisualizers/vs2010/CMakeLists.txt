# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32)

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

#build debugger visualizer for visual studio 2010
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../../Emgu.CV.snk)
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
ENDIF()
