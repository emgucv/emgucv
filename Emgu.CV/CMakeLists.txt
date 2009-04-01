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

SET(CS_FLAGS ${CS_FLAGS} -r:${LIBRARY_3RDPARTY_PATH}/zlib.net.dll -r:System.Xml.dll -r:System.Drawing.dll -r:System.Data.dll -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll)

IF(WIN32)
  SET(CS_FLAGS ${CS_FLAGS} -r:C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.0/System.ServiceModel.dll )
ELSE(WIN32)
  SET(CS_FLAGS ${CS_FLAGS} -r:System.ServiceModel.dll)
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
