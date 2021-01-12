# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
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

ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND "${MSBUILD_EXECUTABLE}" -t:restore "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.sln"
   COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} ${EXTRA_MSBUILD_FLAGS} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.sln"
   COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} ${EXTRA_MSBUILD_FLAGS} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj"
   COMMAND "${MSBUILD_EXECUTABLE}" /p:Configuration=${DEFAULT_CS_CONFIG} ${EXTRA_MSBUILD_FLAGS} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj" /property:PresentationStyle=OpenXml /property:HelpFileFormat=OpenXml
   SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework Emgu.CV.Platform.NetCore)

INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/../Help/Emgu.CV.Documentation.chm"
  DESTINATION ./
  COMPONENT emgucv_document
  )

INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.html.shfbproj"
  DESTINATION miscellaneous
  COMPONENT emgucv_document
  )

set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")

ELSE()
MESSAGE(STATUS "MSBUILD_EXECUTABLE: NOT FOUND for Documentation project")
ENDIF()
