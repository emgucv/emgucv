# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

#MESSAGE(STATUS "MSBUILD_EXECUTABLE_40: ${MSBUILD_EXECUTABLE_40}")
#SET(MSBUILD_EXECUTABLE ${MSBUILD_EXECUTABLE_40})

IF(NOT ("${MSBUILD_EXECUTABLE}" STREQUAL "") AND ${EMGU_CV_DOCUMENTATION_BUILD})
  #MESSAGE(STATUS "MSBUILD_EXECUTABLE: ${MSBUILD_EXECUTABLE}")
  PROJECT(Emgu.CV.Document)

  SET(EXTRA_MSBUILD_FLAGS "")
  IF(WIN32)
    SET(EXTRA_MSBUILD_FLAGS "/p:Platform=\"Any CPU\"")
  ENDIF()

  # Emgu.CV.OpenXml.shfbproj is a dedicated copy of Emgu.CV.html.shfbproj with
  # PresentationStyle/HelpFileFormat hardcoded to OpenXml, rather than overriding those two
  # properties at build time: SHFB's BuildHelp MSBuild task does not forward a plain
  # /property:PresentationStyle=.../property:HelpFileFormat=... global override to the
  # underlying SandcastleProject, so such an override is silently ignored and SHFB just
  # rebuilds the project's own default format again - meaning this never actually produced
  # OpenXml (Word) output. A single pass can't build HtmlHelp1/Website and OpenXml together
  # either way: SHFB ties HelpFileFormat to PresentationStyle, and no style supports all
  # three (confirmed via SHFB error BE0074 - VS2013 supports HtmlHelp1/MSHelpViewer/Website;
  # OpenXml supports only OpenXml), so a second, separate project file is required.
  ADD_CUSTOM_TARGET(
    ${PROJECT_NAME} ALL
    COMMAND "${MSBUILD_EXECUTABLE}" -t:restore "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.sln"
    COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} ${EXTRA_MSBUILD_FLAGS} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.sln"
    COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} ${EXTRA_MSBUILD_FLAGS} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj"
    COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} ${EXTRA_MSBUILD_FLAGS} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.OpenXml.shfbproj"
    COMMAND "${CMAKE_COMMAND}" -E tar "cfv" "${CMAKE_CURRENT_SOURCE_DIR}/../Help.zip" --format=zip "${CMAKE_CURRENT_SOURCE_DIR}/../Help"
    SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.OpenXml.shfbproj
    )
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.WinForms)

  INSTALL(
    FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/../Help/Emgu.CV.Documentation.chm"
    DESTINATION .
    COMPONENT emgucv_document
    )

  INSTALL(
    FILES
    "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj"
    DESTINATION miscellaneous
    COMPONENT emgucv_document
    )

  SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "emgu")

ELSE()
  MESSAGE(STATUS "MSBUILD_EXECUTABLE: NOT FOUND for Documentation project")
ENDIF()
