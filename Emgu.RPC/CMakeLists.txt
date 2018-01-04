# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

PROJECT(Emgu.RPC)

FILE(GLOB_RECURSE SRC_RPC RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_RPC ${SRC_RPC} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)

GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
ADD_CS_REFERENCES("System.Xml.dll;System.Drawing.dll;System.Data.dll;System.Core.dll;System.ServiceModel.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_3RDPARTY_PATH}/zlib.net.dll")

IF(WIN32)
  ADD_CS_REFERENCES(System.Speech.dll)
ELSE(WIN32)
  SET(CS_FLAGS ${CS_FLAGS} "-d:LINUX")
ENDIF(WIN32)

COMPILE_CS(${PROJECT_NAME} "library" "${SRC_RPC}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML 
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_extra_binary)


