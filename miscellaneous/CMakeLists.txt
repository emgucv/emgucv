# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

#MESSAGE(STATUS "MSBUILD_EXECUTABLE_40: ${MSBUILD_EXECUTABLE_40}")
#SET(MSBUILD_EXECUTABLE ${MSBUILD_EXECUTABLE_40})

IF(NOT ("${MSBUILD_EXECUTABLE}" STREQUAL "") AND ${EMGU_CV_DOCUMENTATION_BUILD})
#MESSAGE(STATUS "MSBUILD_EXECUTABLE: ${MSBUILD_EXECUTABLE}")
PROJECT(Emgu.CV.Document)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj"
   COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj" /property:PresentationStyle=OpenXml /property:HelpFileFormat=OpenXml
   SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework Emgu.CV.Platform.NetCore)

INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/../Help/Emgu.CV.Documentation.chm"
  DESTINATION ./
  COMPONENT emgucv_document
  )

set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")

ELSE()
MESSAGE(STATUS "MSBUILD_EXECUTABLE: NOT FOUND for Documentation project")
ENDIF()
