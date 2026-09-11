# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

SET(EMGUCV_MAUI_NUGET_ID "${EMGUCV_NUGET_ID}.runtime.maui${EMGUCV_RUNTIME_EXTRA_TAG}")
SET(EMGUCV_MAUI_NUGET_TITLE "Emgu CV for MAUI Native Runtime")

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
IF(EMGU_SIGN_FOUND AND WIN32)
  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignBinary.Targets)
ENDIF()
IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignPkg.Targets)
ENDIF()
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)


IF(HAVE_MAUI)

  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION Emgu.CV.Runtime/Maui
    COMPONENT emgucv_source
    FILES_MATCHING
    PATTERN "*.cs"
    PATTERN "*.csproj"
    PATTERN "Directory.Build.props"
    PATTERN "README.md"
    #PATTERN "*.png"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "bin" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )

  PROJECT(Emgu.CV.runtime.maui)

  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
  IF(EMGU_SIGN_FOUND AND WIN32)
    # dotnet build parallelizes this project's target frameworks (netstandard2.0,
    # net10.0-android, net10.0-ios, net10.0-maccatalyst, net10.0-windows...) by
    # default, so their PostBuildEvent signing steps hit signtool.exe
    # concurrently. The hardware-backed signing token can't service concurrent
    # SignerSign() calls and fails with "An unexpected internal error has
    # occurred" on whichever framework loses the race.
    SET(EXTRA_BUILD_FLAGS "${EXTRA_BUILD_FLAGS} /p:BuildInParallel=false")
  ENDIF()

  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.csproj" "${EXTRA_BUILD_FLAGS}" ALL)

  TARGET_SOURCES(${PROJECT_NAME} PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md"
    "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.csproj"
  )

  SOURCE_GROUP("Template" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in"
    "${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
  )

  SOURCE_GROUP("Source" FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
    "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.maui.csproj"
    "${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md"
  )

  IF(ENABLE_SOLUTION_FOLDERS)
    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  ENDIF()

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern Emgu.CV)

  IF(TARGET Emgu.CV.runtime.maui.ios)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.ios)
  ENDIF()

  IF(TARGET Emgu.CV.runtime.maui.macos)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.macos)
  ENDIF()

  IF(TARGET Emgu.CV.runtime.maui.android)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.android)
  ENDIF()

  IF(TARGET Emgu.CV.runtime.windows.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.nuget)
  ENDIF()

  INSTALL(
    DIRECTORY
    ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.runtime.maui
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
    COMPONENT emgucv_binary
    FILES_MATCHING
      PATTERN "*.dll"
      PATTERN "*.pdb"
      PATTERN "*.xml")
ENDIF()
