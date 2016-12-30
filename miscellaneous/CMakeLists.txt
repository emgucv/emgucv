# --------------------------------------------------------
#  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

#MESSAGE(STATUS "MSBUILD_EXECUTABLE_40: ${MSBUILD_EXECUTABLE_40}")
SET(MSBUILD_EXECUTABLE ${MSBUILD_EXECUTABLE_40})

IF(NOT ("${MSBUILD_EXECUTABLE}" STREQUAL ""))
#MESSAGE(STATUS "MSBUILD_EXECUTABLE: ${MSBUILD_EXECUTABLE}")
PROJECT(Emgu.CV.Document.Html)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME}
   COMMAND ${MSBUILD_EXECUTABLE} ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
   SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

PROJECT(Emgu.CV.Documentation.chm)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND ${MSBUILD_EXECUTABLE} ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj /property:OutputPath=. /property:HelpFileFormat=HtmlHelp1
   SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Documentation.chm"
  DESTINATION ./
  COMPONENT emgucv_document
  )

ELSE()
MESSAGE(STATUS "MSBUILD_EXECUTABLE: NOT FOUND")
ENDIF()
