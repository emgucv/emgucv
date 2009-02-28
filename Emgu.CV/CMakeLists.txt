PROJECT(Emgu.CV)

IF(WIN32)
  SET(CS_FLAGS -doc:${LIBRARY_OUTPUT_PATH}/Emgu.CV.XML -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll -r:${LIBRARY_3RDPARTY_PATH}/zlib.net.dll -r:C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.0/System.ServiceModel.dll -r:System.Xml.dll -r:System.Drawing.dll -r:System.Data.dll)
ELSE(WIN32)
  SET(CS_FLAGS -doc:${LIBRARY_OUTPUT_PATH}/Emgu.CV.XML -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll -r:${LIBRARY_3RDPARTY_PATH}/zlib.net.dll -r:System.ServiceModel.dll -r:System.Xml.dll -r:System.Drawing.dll -r:System.Data.dll)
ENDIF(WIN32)

SET(SRC_CV *.cs Shape/*.cs Properties/*.cs PointAndLine/*.cs PInvoke/*.cs
PInvoke/CvType/*.cs Color/*.cs Capture/*.cs CameraCalibration/*.cs
Reflection/*.cs VideoSurveillance/*.cs)

ADD_CS_LIBRARY(Emgu.CV "${SRC_CV}")


ADD_DEPENDENCIES(Emgu.CV Emgu.Util)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll
${LIBRARY_OUTPUT_PATH}/Emgu.CV.XML DESTINATION bin
COMPONENT binary)

