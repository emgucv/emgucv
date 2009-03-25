PROJECT(Emgu.CV)

SET(SRC_CV *.cs Shape/*.cs PointAndLine/*.cs PInvoke/*.cs
PInvoke/CvType/*.cs Color/*.cs Capture/*.cs CameraCalibration/*.cs
Reflection/*.cs VideoSurveillance/*.cs Properties/*.cs)

SET(CS_FLAGS ${CS_FLAGS} -doc:${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML -r:${LIBRARY_3RDPARTY_PATH}/zlib.net.dll -r:System.Xml.dll -r:System.Drawing.dll -r:System.Data.dll -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll)

IF(WIN32)
  SET(CS_FLAGS ${CS_FLAGS} -r:C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.0/System.ServiceModel.dll )
ELSE(WIN32)
  SET(CS_FLAGS ${CS_FLAGS} -r:System.ServiceModel.dll)
ENDIF(WIN32)

ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.Util)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML DESTINATION bin
COMPONENT binary)
