PROJECT(Emgu.CV)


SET(CS_FLAGS -doc:${LIBRARY_OUTPUT_PATH}/Emgu.CV.XML -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util -r:${LIBRARY_3RDPARTY_PATH}/zlib.net.dll -r:System.ServiceModel -r:System.Xml -r:System.Drawing -r:System.Data)
SET(SRC_CV *.cs Shape/*.cs Properties/*.cs PointAndLine/*.cs PInvoke/*.cs
PInvoke/CvType/*.cs Color/*.cs Capture/*.cs CameraCalibration/*.cs
Reflection/*.cs VideoSurveillance/*.cs)

ADD_CS_LIBRARY(Emgu.CV "${SRC_CV}")


ADD_DEPENDENCIES(Emgu.CV Emgu.Util)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll
${LIBRARY_OUTPUT_PATH}/Emgu.CV.XML DESTINATION bin
COMPONENT binary)

