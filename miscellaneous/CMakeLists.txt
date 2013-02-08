# --------------------------------------------------------
#  Copyright (C) 2004-2013 by EMGU. All rights reserved.
# --------------------------------------------------------

IF(${MSBUILD_PROGRAM} MATCHES "MSBuild") 
MESSAGE(STATUS "MSBUILD_PROGRAM: ${MSBUILD_PROGRAM}")
PROJECT(Emgu.CV.Document.Html)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME}
   COMMAND ${MSBUILD_PROGRAM} ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
   SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.Util Emgu.CV Emgu.CV.UI Emgu.CV.ML Emgu.CV.GPU Emgu.CV.OCR)

PROJECT(Emgu.CV.Documentation.chm)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND ${MSBUILD_PROGRAM} ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj /property:OutputPath=. /property:HelpFileFormat=HtmlHelp1
   SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.Util Emgu.CV Emgu.CV.UI Emgu.CV.ML Emgu.CV.GPU Emgu.CV.OCR)

INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Documentation.chm"
  DESTINATION ./
  COMPONENT emgucv_document
  )

ELSE()
MESSAGE(STATUS "MSBUILD_PROGRAM: NOT FOUND")
ENDIF()
