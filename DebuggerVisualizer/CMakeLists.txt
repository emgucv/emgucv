# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
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
  PATTERN "*.xaml"
  PATTERN "*.json"
  PATTERN "Directory.Build.props"
  PATTERN ".svn" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMakeFiles" EXCLUDE
  PATTERN "bin" EXCLUDE
)


PROJECT(DebuggerVisualizer)

  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
  IF(EMGU_SIGN_FOUND AND WIN32)
    CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignBinary.Targets)
	CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignVSIX.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignVSIX.Targets)
  ENDIF()

BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/DebuggerVisualizer.csproj" "${EXTRA_BUILD_FLAGS}" ALL)

IF (TARGET Emgu.CV.Platform.NetStandard)
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard)
ENDIF()

ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

IF (TARGET Emgu.CV.runtime.windows.nuget)
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.nuget)
ENDIF()

IF (TARGET Emgu.CV.Wpf)
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Wpf)
ENDIF()



if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()


ENDIF()
