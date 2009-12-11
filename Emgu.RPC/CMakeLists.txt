PROJECT(Emgu.RPC)

SET(SRC_RPC
  ../CommonAssemblyInfo.cs
  Capture/*.cs
  Serial/*.cs
  Speech/*.cs
  *.cs 
  Properties/*.cs)

GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
ADD_CS_REFERENCES("System.Xml.dll;System.Drawing.dll;System.Data.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_3RDPARTY_PATH}/zlib.net.dll")
IF(WIN32)
  ADD_CS_REFERENCES(C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.0/System.ServiceModel.dll)
  ADD_CS_REFERENCES(C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.0/System.Speech.dll)
  ADD_CS_REFERENCES(C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.5/System.Core.dll)
ELSE(WIN32)
  ADD_CS_REFERENCES(System.ServiceModel.dll)
  ADD_CS_REFERENCES(System.Core.dll)
ENDIF(WIN32)

IF(NOT WIN32)
  SET(CS_FLAGS ${CS_FLAGS} "-d:LINUX")
ENDIF()

ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_RPC}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML 
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT binary)


