IF(EMGU_CV_EXAMPLE_BUILD)

SET(EMGU_CV_REFERENCE 
-r:"${LIBRARY_3RDPARTY_PATH}/zlib.net.dll"
-r:System.Xml.dll 
-r:System.Drawing.dll 
-r:System.Data.dll 
-r:System.Windows.Forms.dll
-r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll" 
-r:"${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll" 
-r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll")

add_subdirectory(HelloWorld)
add_subdirectory(CameraCapture)
add_subdirectory(PlanarSubdivision)
add_subdirectory(ShapeDetection)
add_subdirectory(SURFFeature)
add_subdirectory(TrafficSignRecognition)
add_subdirectory(MotionDetection)
add_subdirectory(VideoSurveilance)
add_subdirectory(FaceDetection)
add_subdirectory(Webservice_Host)
add_subdirectory(Webservice_Client)
IF(WIN32)
add_subdirectory(WebDynamicImage)
ENDIF()

ENDIF()