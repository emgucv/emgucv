# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(TARGET Emgu.CV.Bitmap) # Only build Emgu.CV.Wpf if Emgu.CV.Bitmap target exist
  IF(DOTNET_FOUND AND WIN32)
    # Only build Emgu.CV.Wpf for Windows
    PROJECT(Emgu.CV.Wpf)

    SET(EXTRA_BUILD_FLAGS "")
    #generate nuget config file for Emgu.CV.Wpf
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
    IF(EMGU_SIGN_FOUND AND WIN32)
      CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignBinary.Targets)
    ENDIF()
    IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
      CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${CMAKE_CURRENT_SOURCE_DIR}/SignPkg.Targets)
    ENDIF()
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)

    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Wpf.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
    INSTALL(
      DIRECTORY
      ${LIBRARY_OUTPUT_PATH}/Build/Emgu.CV.Wpf
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary)

    IF(ENABLE_SOLUTION_FOLDERS)
      SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    ENDIF()
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.Bitmap)
  ENDIF()

  INSTALL(
    DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    DESTINATION .
    COMPONENT emgucv_source
    FILES_MATCHING
    PATTERN "Emgu.CV.Wpf.csproj"
    PATTERN "Directory.Build.props"
    PATTERN "README.md"
    PATTERN "*.cs"
    PATTERN ".svn" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMakeFiles" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )

ENDIF()
