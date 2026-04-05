# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

SET(EMGUCV_WEBASSEMBLY_NUGET_ID "${EMGUCV_NUGET_ID}.runtime${EMGUCV_RUNTIME_EXTRA_TAG}.webassembly")
SET(EMGUCV_WEBASSEMBLY_NUGET_TITLE "Emgu CV WebAssembly Native Runtime")

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.webassembly.targets.in ${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_WEBASSEMBLY_NUGET_ID}.targets)
IF(EMGU_SIGN_FOUND AND WIN32)
  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignBinary.Targets)
ENDIF()
IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignPkg.Targets)
ENDIF()
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)

IF(HAVE_EMSCRIPTEN)

  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Runtime/WebAssembly
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
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )

  PROJECT(Emgu.CV.runtime.webassembly)

  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")

  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Runtime.WebAssembly.csproj" "${EXTRA_BUILD_FLAGS}" ALL)

  TARGET_SOURCES(${PROJECT_NAME} PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md"
    "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.webassembly.targets.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_WEBASSEMBLY_NUGET_ID}.targets"
    "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Runtime.WebAssembly.csproj"
  )

  source_group("Template" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.webassembly.targets.in"
  )

  source_group("Source" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
    "${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_WEBASSEMBLY_NUGET_ID}.targets"
    "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Runtime.WebAssembly.csproj"
    "${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md"
  )

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif()

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)

  INSTALL(
    DIRECTORY
    ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.runtime.webassembly
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
    COMPONENT emgucv_binary
    FILES_MATCHING
      PATTERN "*.dll"
      PATTERN "*.pdb"
      PATTERN "*.xml")

ENDIF()
