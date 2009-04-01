PROJECT(Emgu.CV)

SET(SRC_CV 
   *.cs 
   Shape/*.cs 
   PointAndLine/*.cs 
   PInvoke/*.cs
   PInvoke/CvType/*.cs 
   Color/*.cs 
   Capture/*.cs 
   CameraCalibration/*.cs
   Reflection/*.cs 
   VideoSurveillance/*.cs 
   Properties/*.cs)

ADD_CS_REFERENCES("System.Xml.dll;System.Drawing.dll;System.Data.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_3RDPARTY_PATH}/zlib.net.dll")
IF(WIN32)
  ADD_CS_REFERENCES(C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.0/System.ServiceModel.dll)
ELSE(WIN32)
  ADD_CS_REFERENCES(System.ServiceModel.dll)
ENDIF(WIN32)

GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})
SIGN_ASSEMBLY(CV.snk)

ADD_CS_RESOURCES(${CMAKE_CURRENT_SOURCE_DIR}/Properties/StringTable.resx ${PROJECT_NAME}.Properties.StringTable.resources)

#FOR Linux, copy the dll mapping file such that mono can find the unmanaged libraries
IF (WIN32)
ELSE (WIN32)
   ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/${PROJECT_NAME}.dll.config)
ENDIF(WIN32)

ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.Util cvextern)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML DESTINATION bin
COMPONENT binary)
